using UnityEngine;
using Prime31;
using XInputDotNetPure;
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

    public bool oldpadForJump;
    public int myteam;



    private GamePadState gamepadState;

    [SerializeField]
    private GameObject deathPrefab;

    [Space(20)]

    [Tooltip("Cet index permet de choisir via quelle manette ce joueur va être controllé")]
    [SerializeField]
    private PlayerIndex index = PlayerIndex.One;

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
    public RuntimeAnimatorController ColorBlue2;
    public RuntimeAnimatorController ColorViolet;
    public RuntimeAnimatorController ColorGreen;
    public RuntimeAnimatorController ColorOrange;
    public RuntimeAnimatorController ColorRed;
    public RuntimeAnimatorController ColorYellow;
    public RuntimeAnimatorController ColorPurple;


    public bool IsTargeting
    {
        get;
        private set;
    }

    public PlayerIndex Index
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

    private RuntimeAnimatorController color1;
    private RuntimeAnimatorController color2;
    private RuntimeAnimatorController color3;
    private RuntimeAnimatorController color4;

    private void Awake()
        
    {
        ColorBlue = Resources.Load<RuntimeAnimatorController>("Animations/PlayerBlue");
        ColorBlue2 = Resources.Load<RuntimeAnimatorController>("Animations/PlayerBlue2");
        ColorViolet = Resources.Load<RuntimeAnimatorController>("Animations/PlayerViolet");
        ColorGreen = Resources.Load<RuntimeAnimatorController>("Animations/PlayerGreen");
        ColorOrange = Resources.Load<RuntimeAnimatorController>("Animations/PlayerOrange");
        ColorRed = Resources.Load<RuntimeAnimatorController>("Animations/PlayerRed");
        ColorYellow = Resources.Load<RuntimeAnimatorController>("Animations/PlayerYellow");
        ColorPurple = Resources.Load<RuntimeAnimatorController>("Animations/PlayerPurple");

        if(GameMenuManager2.gamepad1color == 1) {color1 = ColorRed;}
        if(GameMenuManager2.gamepad1color == 2) {color1 = ColorOrange;}
        if(GameMenuManager2.gamepad1color == 3) {color1 = ColorYellow;}
        if(GameMenuManager2.gamepad1color == 4) {color1 = ColorPurple;}
        if(GameMenuManager2.gamepad1color == 21) {color1 = ColorBlue;}
        if(GameMenuManager2.gamepad1color == 22) {color1 = ColorBlue2;}
        if(GameMenuManager2.gamepad1color == 23) {color1 = ColorGreen;}
        if(GameMenuManager2.gamepad1color == 24) {color1 = ColorViolet;}

        if(GameMenuManager2.gamepad2color == 1) {color2 = ColorRed;}
        if(GameMenuManager2.gamepad2color == 2) {color2 = ColorOrange;}
        if(GameMenuManager2.gamepad2color == 3) {color2 = ColorYellow;}
        if(GameMenuManager2.gamepad2color == 4) {color2 = ColorPurple;}
        if(GameMenuManager2.gamepad2color == 21) {color2 = ColorBlue;}
        if(GameMenuManager2.gamepad2color == 22) {color2 = ColorBlue2;}
        if(GameMenuManager2.gamepad2color == 23) {color2 = ColorGreen;}
        if(GameMenuManager2.gamepad2color == 24) {color2 = ColorViolet;}

        if(GameMenuManager2.gamepad3color == 1) {color3 = ColorRed;}
        if(GameMenuManager2.gamepad3color == 2) {color3 = ColorOrange;}
        if(GameMenuManager2.gamepad3color == 3) {color3 = ColorYellow;}
        if(GameMenuManager2.gamepad3color == 4) {color3 = ColorPurple;}
        if(GameMenuManager2.gamepad3color == 21) {color3 = ColorBlue;}
        if(GameMenuManager2.gamepad3color == 22) {color3 = ColorBlue2;}
        if(GameMenuManager2.gamepad3color == 23) {color3 = ColorGreen;}
        if(GameMenuManager2.gamepad3color == 24) {color3 = ColorViolet;}

        if(GameMenuManager2.gamepad4color == 1) {color4 = ColorRed;}
        if(GameMenuManager2.gamepad4color == 2) {color4 = ColorOrange;}
        if(GameMenuManager2.gamepad4color == 3) {color4 = ColorYellow;}
        if(GameMenuManager2.gamepad4color == 4) {color4 = ColorPurple;}
        if(GameMenuManager2.gamepad4color == 21) {color4 = ColorBlue;}
        if(GameMenuManager2.gamepad4color == 22) {color4 = ColorBlue2;}
        if(GameMenuManager2.gamepad4color == 23) {color4 = ColorGreen;}
        if(GameMenuManager2.gamepad4color == 24) {color4 = ColorViolet;}

        this.controller = GetComponent<CharacterController2D>();
        this.gamepadState = GamePad.GetState(Index);

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
            (myteam == 2 ?
                (cast.collider.CompareTag(Tags.Player1) || cast.collider.CompareTag(Tags.Player2)) :
                (cast.collider.CompareTag(Tags.Player3) || cast.collider.CompareTag(Tags.Player4)))
           )
        {
            var enemy = cast.collider;
            var enemyPlayerHandler = enemy.GetComponent<PlayerMovementHandler>();

            // Get ball from enemy
            if (BallHandler.Get.Index == enemyPlayerHandler.index)
            {
                BallHandler.Get.SetGrabbed(this.ballAnchor, this.index, myteam);
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
            if (BallHandler.Get.EngageShoot == true && BallHandler.Get.LastShooter != this.Index)
            {
                this.SetDestroyed();
            }
            else if (this.canGrab && BallHandler.Get.IsGrabbed == false)
            {
                BallHandler.Get.SetGrabbed(this.ballAnchor, this.index, myteam);
                this.canGrab = false;
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

    private void Update() {

        //this.myteam = Team.GetTeam(this.Index);

        if(this.gamepadState.Buttons.LeftShoulder == ButtonState.Pressed) {
            this.LBhold = true;
            this.LBtime += 0.1f;
        }
        if(this.LBhold & this.gamepadState.Buttons.LeftShoulder == ButtonState.Released) {
            this.LBreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                this.LBhold = false;
                this.LBreleased = false;
                this.LBtime = 0;
            }, 0.00001f));
        }

        if(this.gamepadState.Triggers.Left > 0) {
            this.LThold = true;
            this.LTtime += 0.1f;
        }
        if(this.LThold & this.gamepadState.Triggers.Left == 0) {
            this.LTreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                this.LThold = false;
                this.LTreleased = false;
                this.LTtime = 0;
            }, 0.00001f));
        }
        if(this.gamepadState.Buttons.RightShoulder == ButtonState.Pressed) {
            this.RBhold = true;
            this.RBtime += 0.1f;
        }
        if(this.RBhold & this.gamepadState.Buttons.RightShoulder == ButtonState.Released) {
            this.RBreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                this.RBhold = false;
                this.RBreleased = false;
                this.RBtime = 0;
            }, 0.00001f));
        }
        if(this.gamepadState.Triggers.Right > 0) {
            this.RThold = true;
            this.RTtime += 0.1f;
        }
        if(this.RThold & this.gamepadState.Triggers.Left == 0) {
            this.RTreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                this.RThold = false;
                this.RTreleased = false;
                this.RTtime = 0;
            }, 0.00001f));
        }
        if(this.isDestroyed) {
            return;
        }

        this.gamepadState = GamePad.GetState(Index);

        if(BallHandler.Get.isGrabbed && BallHandler.Get.Index == this.index) {
            // Pass controlset
            if(this.RBreleased || this.RTreleased) {
                if(this.RBreleased && RBtime > 3f) {
                    BallHandler.Get.Shoot(this.sight, this.passPower, ShootType.Pass);
                } else if(this.RTreleased && RTtime > 3f) {
                    BallHandler.Get.Shoot(this.sight, this.passPower, ShootType.Pass);
                } else {
                    BallHandler.Get.Shoot(this.FriendTransform, this.passPower, ShootType.Pass);
                }

                this.canGrab = false;
                StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                    this.canGrab = true;
                }, this.deltaTimeGrab));
            }


            // Shoot control
            if(this.LBreleased || this.LTreleased) {
                if(GameManager.Get.CanShoot(myteam)) {
                    BallHandler.Get.Shoot(this.sight, this.shootPower, ShootType.Shoot);
                }

                this.canGrab = false;
                StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                    this.canGrab = true;
                }, this.deltaTimeGrab));
            }
        }

        // Sight control
        this.IsTargeting = this.RTtime >= 3 || this.RBtime >= 3 || this.LTtime >= 3 || this.LBtime >= 3;

        if(this.IsTargeting) {
            if(this.gamepadState.Buttons.LeftShoulder == ButtonState.Pressed) {

                if(LBtime < 1.2f && this.player.transform.localScale.x < 0f) {
                    this.sight.transform.localPosition = Vector3.left * 2.5f;
                } else {
                    this.sight.transform.localPosition = Vector3.right * 2.5f;
                }
            }
            if(this.gamepadState.Triggers.Left > 0) {

                if(LTtime < 1.2f && this.player.transform.localScale.x < 0f) {
                    this.sight.transform.localPosition = Vector3.left * 2.5f;
                } else {
                    this.sight.transform.localPosition = Vector3.right * 2.5f;
                }
            }
        }


        if(this.IsTargeting) {
            this.sightAnchor.gameObject.SetActive(true);
            if((Mathf.Atan2(this.gamepadState.ThumbSticks.Right.Y, this.gamepadState.ThumbSticks.Right.X) > 0) || (Mathf.Atan2(this.gamepadState.ThumbSticks.Right.Y, this.gamepadState.ThumbSticks.Right.X) < 0)) {

                stickY = this.gamepadState.ThumbSticks.Right.Y;
                stickX = this.gamepadState.ThumbSticks.Right.X;
            }
            this.sightAnchor.eulerAngles = new Vector3(0, 0, Mathf.Atan2(stickY, stickX) * 180 / Mathf.PI);
        } else {
            stickY = 0;
            stickX = 0;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                this.sightAnchor.gameObject.SetActive(false);
            }, 0.0001f));
        }

        // Movement & Jump control
        if(this.controller.isGrounded) {
            this.jumpsCount = 0;
            this.velocity.y = 0;
        }

        if(this.gamepadState.ThumbSticks.Left.X > 0.1f) {
            this.normalizedHorizontalSpeed = 1;
            if(this.controller.isGrounded) {
                this.animator.Play(Animator.StringToHash("Run"));
            }
            if(this.player.transform.localScale.x < 0f) {
                this.player.transform.localScale = new Vector3(-this.player.transform.localScale.x, this.player.transform.localScale.y, this.player.transform.localScale.z);
            }
        } else if(this.gamepadState.ThumbSticks.Left.X < -0.1f) {
            this.normalizedHorizontalSpeed = -1;
            if(this.controller.isGrounded) {
                this.animator.Play(Animator.StringToHash("Run"));
            }
            if(this.player.transform.localScale.x > 0f) {
                this.player.transform.localScale = new Vector3(-this.player.transform.localScale.x, this.player.transform.localScale.y, this.player.transform.localScale.z);
            }
        } else {
            this.normalizedHorizontalSpeed = 0;
            if(this.controller.isGrounded) {
                this.animator.Play(Animator.StringToHash("Idle"));
            }
        }

        // Jump control
        if((this.controller.isGrounded || this.jumpsCount < this.maxJumps)
            && this.gamepadState.Buttons.A == ButtonState.Pressed
            && this.oldpadForJump == false
            ) {
            this.jumpsCount++;
            this.velocity.y = Mathf.Sqrt(2f * this.jumpHeight * -this.gravity);
            this.animator.Play(Animator.StringToHash("Jump"));
            oldpadForJump = true;
        }

        if(this.gamepadState.Buttons.A == ButtonState.Released) {
            oldpadForJump = false;
        }
        // Dash control

        if(this.canGrab == true

            && this.isDashing == false
            && this.gamepadState.Buttons.B == ButtonState.Pressed
            && new Vector2(this.gamepadState.ThumbSticks.Left.X, this.gamepadState.ThumbSticks.Left.Y).magnitude > 0.5f
            && this.canDash) {
                this.canDash = false;
                this.isDashing = true;

                var direction = new Vector2(this.gamepadState.ThumbSticks.Left.X, this.gamepadState.ThumbSticks.Left.Y);
                this.velocity = (direction.normalized * this.dashDirectionOverdrive) * this.dashSpeed;

                this.savedGravity = this.gravity;
                this.gravity = 0;

                this.animator.Play(Animator.StringToHash("Dash"));

                StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                    // Reset gravity and stop dashing
                    this.gravity = this.savedGravity;
                    this.isDashing = false;
                }, this.dashDuration));

                this.dashCoroutine = StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                    if(new Vector2(this.gamepadState.ThumbSticks.Left.X, this.gamepadState.ThumbSticks.Left.Y).magnitude == 0
                        && this.canDash == false
                        && this.isDashing == false) {
                        // Allow to do a dash
                        this.canDash = true;
                    }
                    this.dashCoroutine = null;
                }, this.dashCoolDown));
            }
    

        if (new Vector2(this.gamepadState.ThumbSticks.Left.X, this.gamepadState.ThumbSticks.Left.Y).magnitude == 0
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

        if (this.controller.isGrounded && this.gamepadState.ThumbSticks.Left.Y < 0)
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
            case PlayerIndex.One:
                color = color1;
                break;
            case PlayerIndex.Two:
                color = color2;
                break;
            case PlayerIndex.Three:
                color = color3;
                break;
            case PlayerIndex.Four:
                color = color4;
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
        if(BallHandler.Get.lastShooterTeam != myteam)
        {
            var otherTeamIndex = myteam;
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

        GameManager.Get.ResetBarLevel(myteam);
        this.player.gameObject.SetActive(false);
        this.trigger2D.enabled = false;
        this.controller.boxCollider.enabled = false;
        this.canGrab = false;
    }
}
