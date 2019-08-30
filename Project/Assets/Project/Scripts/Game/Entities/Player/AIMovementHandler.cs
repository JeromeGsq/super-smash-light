using UnityEngine;
using Prime31;
using XInputDotNetPure;
using DG.Tweening;

public enum AIControllerType
{
    Random = 0,
    JPS = 1
}

[RequireComponent(typeof(CharacterController2D))]
public class AIMovementHandler : MovementHandler
{
    public AIControllerType chosenAiType;
    private AgentAI agent;

    private CharacterController2D controller;
    private Vector3 mainPosition;

    private Vector3 velocity;

    private bool canGrab = true;

    public int jumpsCount;

    private Coroutine dashCoroutine;
    private bool isDashing;
    private bool canDash = true;

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

    [SerializeField]
    private GameObject deathPrefab;

    [Space(20)]

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

    [Space(20)]

    
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

    public Transform virtualSight;

    private GridMaker maker;


    public bool IsTargeting
    {
        get;
        private set;
    }

    public Vector3 MainPosition
    {
        get => mainPosition;
        set => mainPosition = value;
    }


    protected override void Awake()
        
    {
        base.Awake();

        controller = GetComponent<CharacterController2D>();

        savedGravity = gravity;

        // listen to some events for illustration purposes
        controller.onControllerCollidedEvent += OnControllerCollider;
        controller.onTriggerEnterEvent += OnTriggerEnterEvent;
        controller.onTriggerExitEvent += OnTriggerExitEvent;

        if (trigger2D == null)
        {
            trigger2D = GetComponent<Collider2D>();
            Debug.Log("PlayerHandler : Awake() : trigger2D is null, try to GetComponent<Collider2D>() on it");
        }

        trigger2D.isTrigger = true;

        switch(chosenAiType)
        {
            case AIControllerType.Random:
                agent = new RandomAgent();
                break;
            case AIControllerType.JPS:
                agent = new JPSAgent();
                break;
            default:
                Debug.LogError("Bad AI type chosen");
                break;
        }

        maker = FindObjectOfType<GridMaker>();
    }

    private void OnEnable()

    {

        mainPosition = transform.position;
    }

    private void OnControllerCollider(RaycastHit2D cast)
    {
        if (isDashing && isPushed == false &&
            (myteam == 2 ?
                (cast.collider.CompareTag(Tags.Player1) || cast.collider.CompareTag(Tags.Player2)) :
                (cast.collider.CompareTag(Tags.Player3) || cast.collider.CompareTag(Tags.Player4)))
           )
        {
            var enemy = cast.collider;
            var enemyPlayerHandler = enemy.GetComponent<PlayerMovementHandler>();
            var enemyAIHandler = enemy.GetComponent<AIMovementHandler>();

            if (enemyPlayerHandler != null)
            {
                // Get ball from enemy
                if (BallHandler.Get.Index == enemyPlayerHandler.Index)
                {
                    BallHandler.Get.SetGrabbed(ballAnchor, Index, myteam);
                }

                // Apply collision on enemy
                enemyPlayerHandler.Collision(transform, pushPowerOnEnemy);

                // Apply collision on ourselves
                Collision(enemy.transform, pushPowerOnMe);
            }
            else if (enemyAIHandler != null)
            {
                // Get ball from enemy
                if (BallHandler.Get.Index == enemyAIHandler.Index)
                {
                    BallHandler.Get.SetGrabbed(ballAnchor, Index, myteam);
                }

                // Apply collision on enemy
                enemyAIHandler.Collision(transform, pushPowerOnEnemy);

                // Apply collision on ourselves
                Collision(enemy.transform, pushPowerOnMe);
            }
            else Debug.LogError("Enemy Without Movement Handler Detected");
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
            if (BallHandler.Get.EngageShoot == true && BallHandler.Get.LastShooter != Index)
            {
                SetDestroyed();
            }
            else if (canGrab && BallHandler.Get.IsGrabbed == false)
            {
                BallHandler.Get.SetGrabbed(ballAnchor, Index, myteam);
            }
        }
    }

    private void Update() {

        agent.Update(maker, gameObject);

        //myteam = Team.GetTeam(Index);

        if(agent.shootOn) {
            LBhold = true;
            LBtime += 0.1f;
        }
        if(LBhold & !agent.shootOn) {
            LBreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                LBhold = false;
                LBreleased = false;
                LBtime = 0;
            }, 0.00001f));
        }

        if(agent.passOn) {
            RBhold = true;
            RBtime += 0.1f;
        }
        if(RBhold & !agent.passOn) {
            RBreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                RBhold = false;
                RBreleased = false;
                RBtime = 0;
            }, 0.00001f));
        }

        if(isDestroyed) {
            return;
        }

        //gamepadState = GamePad.GetState(Index);

        if(BallHandler.Get.isGrabbed && BallHandler.Get.Index == Index) {
            // Pass controlset
            if(RBreleased || RTreleased) {
                if(RBreleased && RBtime > 1.5f) {
                    BallHandler.Get.Shoot(virtualSight, passPower, ShootType.Pass);
                } else if(RTreleased && RTtime > 1.5f) {
                    BallHandler.Get.Shoot(virtualSight, passPower, ShootType.Pass);
                } else {
                    BallHandler.Get.Shoot(FriendTransform, passPower, ShootType.Pass);
                }

                canGrab = false;
                StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                    canGrab = true;
                }, deltaTimeGrab));
            }


            // Shoot control
            if(LBreleased || LTreleased) {
                if(GameManager.Get.CanShoot(myteam)) {
                    BallHandler.Get.Shoot(virtualSight, shootPower, ShootType.Shoot);
                }

                canGrab = false;
                StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                    canGrab = true;
                }, deltaTimeGrab));
            }
        }

        // Sight control
        IsTargeting = 
            agent.shootOn || agent.passOn;

        if(IsTargeting) {
            if(agent.shootOn) {

                if(LBtime < 1.2f && player.transform.localScale.x < 0f) {
                    virtualSight.localPosition = Vector3.left * 2.5f;
                } else {
                    virtualSight.transform.localPosition = Vector3.right * 2.5f;
                }
            }
        }


        if(IsTargeting) {
            if((Mathf.Atan2(agent.targetingX, agent.targetingY) > 0) || (Mathf.Atan2(agent.targetingY, agent.targetingX) < 0)) {

                stickY = agent.targetingY;
                stickX = agent.targetingX;
            }
            virtualSight.eulerAngles = new Vector3(0, 0, Mathf.Atan2(stickY, stickX) * 180 / Mathf.PI);
        } else {
            stickY = 0;
            stickX = 0;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
            }, 0.0001f));
        }

        // Movement & Jump control
        if(controller.isGrounded) {
            jumpsCount = 0;
            velocity.y = 0;
        }

        if(agent.horizontalSpeed > 0.1f) {
            normalizedHorizontalSpeed = 1;
            if(controller.isGrounded) {
                animator.Play(Animator.StringToHash("Run"));
            }
            if(player.transform.localScale.x < 0f) {
                player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
            }
        } else if(agent.horizontalSpeed < -0.1f) {
            normalizedHorizontalSpeed = -1;
            if(controller.isGrounded) {
                animator.Play(Animator.StringToHash("Run"));
            }
            if(player.transform.localScale.x > 0f) {
                player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
            }
        } else {
            normalizedHorizontalSpeed = 0;
            if(controller.isGrounded) {
                animator.Play(Animator.StringToHash("Idle"));
            }
        }

        // Jump control
        if((controller.isGrounded || jumpsCount < maxJumps)
            && agent.jumpOn
            && oldpadForJump == false
            )
        {
            jumpsCount++;
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            animator.Play(Animator.StringToHash("Jump"));
            oldpadForJump = true;
        }

        if(!agent.jumpOn) 
            {
            oldpadForJump = false;
            }
        // Dash control

        if (BallHandler.Get.Index != Index
            && isDashing == false
            && agent.dashOn
            && canDash)
        {
            canDash = false;
            isDashing = true;

            var direction = new Vector2(agent.horizontalSpeed, 0);
            velocity = (direction.normalized * dashDirectionOverdrive) * dashSpeed;

            savedGravity = gravity;
            gravity = 0;

            animator.Play(Animator.StringToHash("Dash"));

            StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                // Reset gravity and stop dashing
                gravity = savedGravity;
                isDashing = false;
            }, dashDuration));

            dashCoroutine = StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                canDash = true;
                //dashCoroutine = null;
            }, dashCoolDown));
        }

        if (new Vector2(agent.horizontalSpeed, 0).magnitude == 0
            && canDash == false
            && isDashing == false
            && dashCoroutine == null)
        {
            canDash = true;
        }

        // Is Pushed
        if (isPushed == true && pushSender != null)
        {
            var direction = (transform.position - pushSender.position).normalized;

            velocity.y = Mathf.Sqrt(2f * 2 * -gravity);
            velocity.x = direction.x * pushPower;

            pushPower = 0;
            pushSender = null;

            player.DOLocalRotate(new Vector3(0, 0, -Mathf.Sign(direction.x) * 360), dashDuration * 2, RotateMode.FastBeyond360);

            StartCoroutine(CoroutineUtils.DelaySeconds(() =>
            {
                // Allow to do a dash
                isPushed = false;
            }, 1f));
        }

        var smoothedMovementFactor = controller.isGrounded ? groundDamping : inAirDamping;
        velocity.x = Mathf.Lerp(velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

        if (gravity == 0)
        {
            // Smooth dash on Y axis without gravity applied
            velocity.y = Mathf.Lerp(velocity.y, 0, Time.deltaTime * smoothedMovementFactor);
        }
        else
        {
            // Use gravity as normal situation
            velocity.y += gravity * Time.deltaTime;
        }

        if (controller.isGrounded && agent.diveOn)
        {
            controller.ignoreOneWayPlatformsThisFrame = true;
        }

        controller.move(velocity * Time.deltaTime);

        velocity = controller.velocity;


    }

    private void SetDestroyed()
    {
        BallHandler.Get.EngageShoot = false;
        isDestroyed = false;

        CameraHandler.Get.Rumble();

        // Add point to the other team
        if(BallHandler.Get.lastShooterTeam != myteam)
        {
            var otherTeamIndex = myteam;
            Debug.Log("Teamadd1");
            GameManager.Get.AddPoint(otherTeamIndex);
        }

        var particles = Instantiate(deathPrefab);
        particles.transform.position = transform.position;
        particles.transform.SetParent(null);

        StartCoroutine(CoroutineUtils.DelaySeconds(() =>
        {
            Destroy(particles);
        }, 2f));

        // Respawn
        StartCoroutine(CoroutineUtils.DelaySeconds(() =>
        {
            transform.position = mainPosition;
            player.gameObject.SetActive(true);
            isDestroyed = false;
            trigger2D.enabled = true;
            controller.boxCollider.enabled = true;
            canGrab = true;
        }, 1f));

        GameManager.Get.ResetBarLevel(myteam);
        player.gameObject.SetActive(false);
        trigger2D.enabled = false;
        controller.boxCollider.enabled = false;
        canGrab = false;
    }
}
