﻿using UnityEngine;
using Prime31;
using XInputDotNetPure;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovementHandler : MovementHandler
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
    internal PlayerIndex index = PlayerIndex.One;

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
        get => index;
        set
        {
            index = value;
            SetColors();
        }
    }

    public Vector3 MainPosition
    {
        get => mainPosition;
        set => mainPosition = value;
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

        controller = GetComponent<CharacterController2D>();
        gamepadState = GamePad.GetState(Index);

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

            // Get ball from enemy
            if (BallHandler.Get.Index == enemyPlayerHandler.index)
            {
                BallHandler.Get.SetGrabbed(ballAnchor, index, myteam);
            }

            // Apply collision on enemy
            enemyPlayerHandler.Collision(transform, pushPowerOnEnemy);

            // Apply collision on ourselves
            Collision(enemy.transform, pushPowerOnMe);
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
                BallHandler.Get.SetGrabbed(ballAnchor, index, myteam);
            }
        }
    }

    // Messages
    public void Collision(Transform sender, float power)
    {
        if (isPushed == false)
        {
            Debug.Log($"PlayerMovementHandler : Push() : pushed by {sender.name}");

            pushSender = sender;
            pushPower = power;
            isPushed = true;
            gravity = savedGravity;
        }
    }

    private void Update() {

        //myteam = Team.GetTeam(Index);

        if(gamepadState.Buttons.LeftShoulder == ButtonState.Pressed) {
            LBhold = true;
            LBtime += 0.1f;
        }
        if(LBhold & gamepadState.Buttons.LeftShoulder == ButtonState.Released) {
            LBreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                LBhold = false;
                LBreleased = false;
                LBtime = 0;
            }, 0.00001f));
        }

        if(gamepadState.Triggers.Left > 0) {
            LThold = true;
            LTtime += 0.1f;
        }
        if(LThold & gamepadState.Triggers.Left == 0) {
            LTreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                LThold = false;
                LTreleased = false;
                LTtime = 0;
            }, 0.00001f));
        }
        if(gamepadState.Buttons.RightShoulder == ButtonState.Pressed) {
            RBhold = true;
            RBtime += 0.1f;
        }
        if(RBhold & gamepadState.Buttons.RightShoulder == ButtonState.Released) {
            RBreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                RBhold = false;
                RBreleased = false;
                RBtime = 0;
            }, 0.00001f));
        }
        if(gamepadState.Triggers.Right > 0) {
            RThold = true;
            RTtime += 0.1f;
        }
        if(RThold & gamepadState.Triggers.Left == 0) {
            RTreleased = true;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                RThold = false;
                RTreleased = false;
                RTtime = 0;
            }, 0.00001f));
        }
        if(isDestroyed) {
            return;
        }

        gamepadState = GamePad.GetState(Index);

        if(BallHandler.Get.isGrabbed && BallHandler.Get.Index == index) {
            // Pass controlset
            if(RBreleased || RTreleased) {
                if(RBreleased && RBtime > 1.5f) {
                    BallHandler.Get.Shoot(sight, passPower, ShootType.Pass);
                } else if(RTreleased && RTtime > 1.5f) {
                    BallHandler.Get.Shoot(sight, passPower, ShootType.Pass);
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
                    BallHandler.Get.Shoot(sight, shootPower, ShootType.Shoot);
                }

                canGrab = false;
                StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                    canGrab = true;
                }, deltaTimeGrab));
            }
        }

        // Sight control
        IsTargeting = gamepadState.Triggers.Left > 0 || gamepadState.Buttons.LeftShoulder == ButtonState.Pressed || gamepadState.Triggers.Right > 0 || gamepadState.Buttons.RightShoulder == ButtonState.Pressed;

        if(IsTargeting) {
            if(gamepadState.Buttons.LeftShoulder == ButtonState.Pressed) {

                if(LBtime < 1.2f && player.transform.localScale.x < 0f) {
                    sight.transform.localPosition = Vector3.left * 2.5f;
                } else {
                    sight.transform.localPosition = Vector3.right * 2.5f;
                }
            }
            if(gamepadState.Triggers.Left > 0) {

                if(LTtime < 1.2f && player.transform.localScale.x < 0f) {
                    sight.transform.localPosition = Vector3.left * 2.5f;
                } else {
                    sight.transform.localPosition = Vector3.right * 2.5f;
                }
            }
        }


        if(IsTargeting) {
            sightAnchor.gameObject.SetActive(true);
            if((Mathf.Atan2(gamepadState.ThumbSticks.Right.Y, gamepadState.ThumbSticks.Right.X) > 0) || (Mathf.Atan2(gamepadState.ThumbSticks.Right.Y, gamepadState.ThumbSticks.Right.X) < 0)) {

                stickY = gamepadState.ThumbSticks.Right.Y;
                stickX = gamepadState.ThumbSticks.Right.X;
            }
            sightAnchor.eulerAngles = new Vector3(0, 0, Mathf.Atan2(stickY, stickX) * 180 / Mathf.PI);
        } else {
            stickY = 0;
            stickX = 0;
            StartCoroutine(CoroutineUtils.DelaySeconds(() => {
                sightAnchor.gameObject.SetActive(false);
            }, 0.0001f));
        }

        // Movement & Jump control
        if(controller.isGrounded) {
            jumpsCount = 0;
            velocity.y = 0;
        }

        if(gamepadState.ThumbSticks.Left.X > 0.1f) {
            normalizedHorizontalSpeed = 1;
            if(controller.isGrounded) {
                animator.Play(Animator.StringToHash("Run"));
            }
            if(player.transform.localScale.x < 0f) {
                player.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
            }
        } else if(gamepadState.ThumbSticks.Left.X < -0.1f) {
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
            && gamepadState.Buttons.A == ButtonState.Pressed
            && oldpadForJump == false
            )
        {
            jumpsCount++;
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            animator.Play(Animator.StringToHash("Jump"));
            oldpadForJump = true;
        }

        if(gamepadState.Buttons.A == ButtonState.Released) 
            {
            oldpadForJump = false;
            }
        // Dash control
        if (BallHandler.Get.Index != Index
            && isDashing == false
            && gamepadState.Buttons.B == ButtonState.Pressed
            && new Vector2(gamepadState.ThumbSticks.Left.X, gamepadState.ThumbSticks.Left.Y).magnitude > 0.5f
            && canDash)
        {
            canDash = false;
            isDashing = true;

            var direction = new Vector2(gamepadState.ThumbSticks.Left.X, gamepadState.ThumbSticks.Left.Y);
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
                if (new Vector2(gamepadState.ThumbSticks.Left.X, gamepadState.ThumbSticks.Left.Y).magnitude == 0
                    && canDash == false
                    && isDashing == false)
                {
                    // Allow to do a dash
                    canDash = true;
                }
                dashCoroutine = null;
            }, dashCoolDown));
        }

        if (new Vector2(gamepadState.ThumbSticks.Left.X, gamepadState.ThumbSticks.Left.Y).magnitude == 0
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

        if (controller.isGrounded && gamepadState.ThumbSticks.Left.Y < 0)
        {
            controller.ignoreOneWayPlatformsThisFrame = true;
        }

        controller.move(velocity * Time.deltaTime);

        velocity = controller.velocity;


    }

    private void SetColors()
    {
        var color = player.GetComponent<Animator>().runtimeAnimatorController;
        switch (Index)
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

        player.GetComponent<Animator>().runtimeAnimatorController = color;
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
