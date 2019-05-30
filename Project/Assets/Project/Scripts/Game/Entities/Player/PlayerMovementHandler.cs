using UnityEngine;
using Prime31;
using GamepadInput;
using static GamepadInput.ip_GamePad;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovementHandler : MonoBehaviour
{
    private CharacterController2D controller;
    private Vector3 mainPosition;

    private Vector3 velocity;

    private bool canGrab = true;

    public int jumpsCount;

    private bool isPushed;
    private float pushPower;
    private Transform pushSender;

    private Coroutine dashCoroutine;
    private bool isDashing;
    private bool canDash = true;

    private float savedGravity;
    private float normalizedHorizontalSpeed;

    private bool isDestroyed;

    public bool RBhold = false;
    public bool RThold = false;
    public float RBtime = 0;
    public float RTtime = 0;

    public bool LBhold = false;
    public bool LThold = false;
    public float LBtime = 0;
    public float LTtime = 0;

    public bool RBreleased = false;
    public bool RTreleased = false;

    public bool LBreleased = false;
    public bool LTreleased = false;

    public float stickY;
    public float stickX;

    private GamepadState gamepadState;

    [SerializeField]
    private GameObject deathPrefab;

    [Space(20)]

    [Tooltip("Cet index permet de choisir via quelle manette ce joueur va être controllé")]
    [SerializeField]
    private Index index = Index.One;

    [Space(20)]

    [Tooltip("Le transform visuel du joueur qui sera flippé selon l'axe 'LocalScaleX'. Ce n'est que visuel")]
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Animator animator;

    [Tooltip("Le collider du player (capsule)")]
    [SerializeField]
    private Collider2D trigger2D;

    [Tooltip("Le point d'ancrage de la balle quand le joueur l'aura sur lui")]
    [SerializeField]
    private Transform ballAnchor;

    [Tooltip("Le transform du pote de ce joueur")]
    [SerializeField]
    private Transform friendTransform;

    [Tooltip("Le transform de l'ancre de la mire. Ce transform sera tourné en fonction du stick. Il ne sera pas déplacé et sert juste d'ancre")]
    [SerializeField]
    private Transform sightAnchor;

    [Tooltip("Le transform de la mire. Ce transform sera déplacé selon l'axe X en fonction de la position du trigger L. Il doit être enfant de sightAnchor")]
    [SerializeField]
    private Transform sight;

    [Space(20)]

    [Tooltip("Permettra de faire descendre plus radipedement ou de faire planner plus longtemps le joueur")]
    [SerializeField]
    private float gravity = -60f;
    [Tooltip("Vitesse du joueur")]
    [SerializeField]
    private float runSpeed = 13f;

    [Space(20)]

    [Tooltip("Vitesse du dash. Une plus grande valeur fera parcourir une plus grand distance et plus rapidement")]
    [SerializeField]
    private float dashSpeed = 16f;
    [Tooltip("Delta de temps avant que le joueur puisse refaire un Dash")]
    [SerializeField]
    private float dashCoolDown = 0.5f;
    [Tooltip("Durée du dash. Une plus grande valeur fera parcourir une plus grand distance")]
    [SerializeField]
    private float dashDuration = 0.1f;
    [Tooltip("Permet de parametrer le dash sur l'axe X et Y. La gravité agit sur le Y, il faut donc ajuster Y")]
    [SerializeField]
    private Vector2 dashDirectionOverdrive = Vector2.one;

    [Space(20)]

    [Tooltip("Une plus petite valeur fera glisser le joueur au sol")]
    [SerializeField]
    private float groundDamping = 10f;
    [Tooltip("Une plus petite valeur fera glisser le joueur dans les air : Air control")]
    [SerializeField]
    private float inAirDamping = 6f;

    [Space(20)]

    [Tooltip("Hauteur d'un saut")]
    [SerializeField]
    private float jumpHeight = 4f;
    [Tooltip("Nombre de saut autorisés avant de devoir revenir sur le sol")]
    [SerializeField]
    private int maxJumps = 2;

    [Space(20)]

    [Tooltip("Delay avant de pouvoir re-grab une ball. Ne pas toucher")]
    [SerializeField]
    private float deltaTimeGrab = 0.2f;

    [Space(20)]

    [Tooltip("Puissance de la balle lors d'une passe")]
    [SerializeField]
    private float passPower = 50f;

    [Tooltip("Puissance de la balle lors d'un tir")]
    [SerializeField]
    private float shootPower = 50f;

    [Space(20)]

    [Tooltip("Puissance appliquée quand on pousse un joueur sur le joueur")]
    [SerializeField]
    private float pushPowerOnEnemy = 50f;

    [Tooltip("Puissance appliquée quand on pousse un joueur sur nous")]
    [SerializeField]
    private float pushPowerOnMe = 50f;

    public RuntimeAnimatorController ColorBlue;
    public RuntimeAnimatorController ColorOrange;
    public RuntimeAnimatorController ColorYellow;
    public RuntimeAnimatorController ColorGreen;

    public bool IsTargeting
    {
        get;
        private set;
    }

    public Index Index
    {
        get => this.index;
        set
        {
            this.index = value;
            this.SetColors();
        }
    }

    public Transform FriendTransform
    {
        get => this.friendTransform;
        set => this.friendTransform = value;
    }

    public Vector3 MainPosition
    {
        get => this.mainPosition;
        set => this.mainPosition = value;
    }

    private void Awake()
        
    {
        ColorBlue = Resources.Load<RuntimeAnimatorController>("Animations/PlayerBlue");
        ColorOrange = Resources.Load<RuntimeAnimatorController>("Animations/PlayerOrange");
        ColorYellow = Resources.Load<RuntimeAnimatorController>("Animations/PlayerYellow");
        ColorGreen = Resources.Load<RuntimeAnimatorController>("Animations/PlayerGreen");

        this.controller = GetComponent<CharacterController2D>();
        this.gamepadState = new GamepadState();

        this.savedGravity = this.gravity;

        // listen to some events for illustration purposes
        this.controller.onControllerCollidedEvent += this.OnControllerCollider;
        this.controller.onTriggerEnterEvent += this.OnTriggerEnterEvent;
        this.controller.onTriggerExitEvent += this.OnTriggerExitEvent;

        if (this.trigger2D == null)
        {
            this.trigger2D = this.GetComponent<Collider2D>();
            Debug.Log("PlayerHandler : Awake() : trigger2D is null, try to GetComponent<Collider2D>() on it");
        }

        this.trigger2D.isTrigger = true;
    }

    private void OnEnable()

    {

        this.mainPosition = this.transform.position;
    }

    private void OnControllerCollider(RaycastHit2D cast)
    {
        if (this.isDashing && this.isPushed == false &&
            (Team.GetTeam(this.index) == 2 ?
                (cast.collider.CompareTag(Tags.Player1) || cast.collider.CompareTag(Tags.Player3)) :
                (cast.collider.CompareTag(Tags.Player2) || cast.collider.CompareTag(Tags.Player4)))
           )
        {
            var enemy = cast.collider;
            var enemyPlayerHandler = enemy.GetComponent<PlayerMovementHandler>();

            // Get ball from enemy
            if (BallHandler.Get.Index == enemyPlayerHandler.index)
            {
                BallHandler.Get.SetGrabbed(this.ballAnchor, this.index);
            }

            // Apply collision on enemy
            enemyPlayerHandler.Collision(this.transform, this.pushPowerOnEnemy);

            // Apply collision on ourselves
            this.Collision(enemy.transform, this.pushPowerOnMe);
        }
    }

    private void OnTriggerEnterEvent(Collider2D collider)
    {
    }

    private void OnTriggerExitEvent(Collider2D collider)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Ball))
        {
            if (BallHandler.Get.EngageShoot == true && BallHandler.Get.LastShooter != this.index)
            {
                this.SetDestroyed();
            }
            else if (this.canGrab && BallHandler.Get.IsGrabbed == false)
            {
                BallHandler.Get.SetGrabbed(this.ballAnchor, this.index);
            }
        }
    }

    // Messages
    public void Collision(Transform sender, float power)
    {
        if (this.isPushed == false)
        {
            Debug.Log($"PlayerMovementHandler : Push() : pushed by {sender.name}");

            this.pushSender = sender;
            this.pushPower = power;
            this.isPushed = true;
            this.gravity = this.savedGravity;
        }
    }

    private void Update()
    {
        if(this.gamepadState.LB) 
        {
            this.LBhold = true;
            this.LBtime += 0.1f;
        }
        if(this.LBhold & !this.gamepadState.LB) 
        {
            this.LBreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                this.LBhold =  false;
                this.LBreleased = false;
                this.LBtime = 0;
            }, 0.00001f));
        }

        if(this.gamepadState.LT >0) 
        {
            this.LThold = true;
            this.LTtime += 0.1f;
        }
        if(this.LThold & this.gamepadState.LT == 0) {
            this.LTreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => 
            {
                this.LThold = false;
                this.LTreleased = false;
                this.LTtime = 0;
            }, 0.00001f));
        }
        if(this.gamepadState.RB)
        {
            this.RBhold = true;
            this.RBtime += 0.1f;
        }
        if(this.RBhold & !this.gamepadState.RB)
        {
            this.RBreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => 
            {
                this.RBhold = false;
                this.RBreleased = false;
                this.RBtime = 0;
            }, 0.00001f));
        }
        if(this.gamepadState.RT > 0) 
        {
            this.RThold = true;
            this.RTtime += 0.1f;
        }
        if(this.RThold & this.gamepadState.RT==0) 
        {
            this.RTreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => 
            {
                this.RThold = false;
                this.RTreleased = false;
                this.RTtime = 0;
            }, 0.00001f));
        }
        if(this.isDestroyed)
        {
            return;
        }

        ip_GamePad.GetState(ref this.gamepadState, this.index);

        if (BallHandler.Get.Index == this.index)
        {
            // Pass control
            if (this.RBreleased || this.RTreleased)
            {
                if (this.RBreleased && RBtime > 1.5f)
                {
                    BallHandler.Get.Shoot(this.sight, this.passPower, ShootType.Pass);
                } 
                else if(this.RTreleased && RTtime > 1.5f) 
                {
                    BallHandler.Get.Shoot(this.sight, this.passPower, ShootType.Pass);
                }
                else
                {
                    BallHandler.Get.Shoot(this.FriendTransform, this.passPower, ShootType.Pass);
                }

                this.canGrab = false;
                StartCoroutine(CoroutineUtils.DelaySeconds(() =>
                {
                    this.canGrab = true;
                }, this.deltaTimeGrab));
            }
            

            // Shoot control
            if (this.LBreleased || this.LTreleased)
            {
                if (GameManager.Get.CanShoot(Team.GetTeam(this.index)))
                {
                    BallHandler.Get.Shoot(this.sight, this.shootPower, ShootType.Shoot);
                }

                this.canGrab = false;
                StartCoroutine(CoroutineUtils.DelaySeconds(() =>
                {
                    this.canGrab = true;
                }, this.deltaTimeGrab));
            }
        }

        // Sight control
        this.IsTargeting = this.gamepadState.LT > 0 || this.gamepadState.LB || this.gamepadState.RT > 0 || this.gamepadState.RB;

        if (this.IsTargeting )
        {
            if(this.gamepadState.LB) 
                {

                if(LBtime < 1.2f && this.player.transform.localScale.x < 0f) 
                {
                    this.sight.transform.localPosition = Vector3.left * 2.5f;
                }
                else 
                {
                    this.sight.transform.localPosition = Vector3.right * 2.5f;
                }
            }
            if(this.gamepadState.LT>0) 
                {

                if(LTtime < 1.2f && this.player.transform.localScale.x < 0f) 
                {
                    this.sight.transform.localPosition = Vector3.left * 2.5f;
                } 
                else
                {
                    this.sight.transform.localPosition = Vector3.right * 2.5f;
                }
            }
        }


        if (this.IsTargeting)
        {
            this.sightAnchor.gameObject.SetActive(true);
            if((Mathf.Atan2(this.gamepadState.RightStickAxis.y, this.gamepadState.RightStickAxis.x) > 0) || (Mathf.Atan2(this.gamepadState.RightStickAxis.y, this.gamepadState.RightStickAxis.x) < 0)) {

                stickY = this.gamepadState.RightStickAxis.y;
                stickX = this.gamepadState.RightStickAxis.x;
            }
            this.sightAnchor.eulerAngles = new Vector3(0, 0, Mathf.Atan2(stickY, stickX) * 180 / Mathf.PI);
        }
        else
        {
            stickY = 0;
            stickX = 0;
            StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                this.sightAnchor.gameObject.SetActive(false);
            }, 0.0001f));
    }

        // Movement & Jump control
        if (this.controller.isGrounded)
        {
            this.jumpsCount = 0;
            this.velocity.y = 0;
        }

        if (this.gamepadState.LeftStickAxis.x > 0.1f )
        {
            this.normalizedHorizontalSpeed = 1;
            if (this.controller.isGrounded)
            {
                this.animator.Play(Animator.StringToHash("Run"));
            }
            if (this.player.transform.localScale.x < 0f)
            {
                this.player.transform.localScale = new Vector3(-this.player.transform.localScale.x, this.player.transform.localScale.y, this.player.transform.localScale.z);
            }
        }
        else if (this.gamepadState.LeftStickAxis.x < -0.1f )
        {
            this.normalizedHorizontalSpeed = -1;
            if (this.controller.isGrounded)
            {
                this.animator.Play(Animator.StringToHash("Run"));
            }
            if (this.player.transform.localScale.x > 0f)
            {
                this.player.transform.localScale = new Vector3(-this.player.transform.localScale.x, this.player.transform.localScale.y, this.player.transform.localScale.z);
            }
        }
        else
        {
            this.normalizedHorizontalSpeed = 0;
            if (this.controller.isGrounded)
            {
                this.animator.Play(Animator.StringToHash("Idle"));
            }
        }

        // Jump control
        if ((this.controller.isGrounded || this.jumpsCount < this.maxJumps)
            && this.gamepadState.APressed)
        {
            this.jumpsCount++;
            this.velocity.y = Mathf.Sqrt(2f * this.jumpHeight * -this.gravity);
            this.animator.Play(Animator.StringToHash("Jump"));
        }

        // Dash control
        if (BallHandler.Get.Index != this.index
            && this.isDashing == false
            && this.gamepadState.B
            && this.gamepadState.LeftStickAxis.magnitude > 0.5f
            && this.canDash)
        {
            this.canDash = false;
            this.isDashing = true;

            var direction = new Vector2(this.gamepadState.LeftStickAxis.x, this.gamepadState.LeftStickAxis.y);
            this.velocity = (direction.normalized * this.dashDirectionOverdrive) * this.dashSpeed;

            this.savedGravity = this.gravity;
            this.gravity = 0;

            this.animator.Play(Animator.StringToHash("Dash"));

            StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                // Reset gravity and stop dashing
                this.gravity = this.savedGravity;
                this.isDashing = false;
            }, this.dashDuration));

            this.dashCoroutine = StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                if (this.gamepadState.LeftStickAxis.magnitude == 0
                    && this.canDash == false
                    && this.isDashing == false)
                {
                    // Allow to do a dash
                    this.canDash = true;
                }
                this.dashCoroutine = null;
            }, this.dashCoolDown));
        }

        if (this.gamepadState.LeftStickAxis.magnitude == 0
            && this.canDash == false
            && this.isDashing == false
            && this.dashCoroutine == null)
        {
            this.canDash = true;
        }

        // Is Pushed
        if (this.isPushed == true && this.pushSender != null)
        {
            var direction = (this.transform.position - this.pushSender.position).normalized;

            this.velocity.y = Mathf.Sqrt(2f * 2 * -this.gravity);
            this.velocity.x = direction.x * this.pushPower;

            this.pushPower = 0;
            this.pushSender = null;

            this.player.DOLocalRotate(new Vector3(0, 0, -Mathf.Sign(direction.x) * 360), this.dashDuration * 2, RotateMode.FastBeyond360);

            StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                // Allow to do a dash
                this.isPushed = false;
            }, 1f));
        }

        var smoothedMovementFactor = this.controller.isGrounded ? this.groundDamping : this.inAirDamping;
        this.velocity.x = Mathf.Lerp(this.velocity.x, normalizedHorizontalSpeed * this.runSpeed, Time.deltaTime * smoothedMovementFactor);

        if (this.gravity == 0)
        {
            // Smooth dash on Y axis without gravity applied
            this.velocity.y = Mathf.Lerp(this.velocity.y, 0, Time.deltaTime * smoothedMovementFactor);
        }
        else
        {
            // Use gravity as normal situation
            this.velocity.y += this.gravity * Time.deltaTime;
        }

        if (this.controller.isGrounded && this.gamepadState.LeftStickAxis.y < 0)
        {
            this.controller.ignoreOneWayPlatformsThisFrame = true;
        }

        this.controller.move(this.velocity * Time.deltaTime);

        this.velocity = this.controller.velocity;


    }

    private void SetColors()
    {
        var color = this.player.GetComponent<Animator>().runtimeAnimatorController;
        switch (this.Index)
        {
            case Index.One:
                color = ColorBlue;
                break;
            case Index.Two:
                color = ColorOrange;
                break;
            case Index.Three:
                color = ColorGreen;
                break;
            case Index.Four:
                color = ColorYellow;
                break;
        }

        this.player.GetComponent<Animator>().runtimeAnimatorController = color;
    }

    private void SetDestroyed()
    {
        BallHandler.Get.EngageShoot = false;
        this.isDestroyed = false;

        CameraHandler.Get.Rumble();

        // Add point to the other team
        if(Team.GetTeam(BallHandler.Get.LastShooter) != Team.GetTeam(this.index))
        {
            var otherTeamIndex = Team.GetTeam(this.index);
            Debug.Log("Teamadd1");
            GameManager.Get.AddPoint(otherTeamIndex);
        }

        var particles = Instantiate(this.deathPrefab);
        particles.transform.position = this.transform.position;
        particles.transform.SetParent(null);

        StartCoroutine(CoroutineUtils.DelaySeconds(() =>
        {
            Destroy(particles);
        }, 2f));

        // Respawn
        StartCoroutine(CoroutineUtils.DelaySeconds(() =>
        {
            this.transform.position = this.mainPosition;
            this.player.gameObject.SetActive(true);
            this.isDestroyed = false;
            this.trigger2D.enabled = true;
            this.controller.boxCollider.enabled = true;
            this.canGrab = true;
        }, 1f));

        GameManager.Get.ResetBarLevel(Team.GetTeam(this.Index));
        this.player.gameObject.SetActive(false);
        this.trigger2D.enabled = false;
        this.controller.boxCollider.enabled = false;
        this.canGrab = false;
    }
}
