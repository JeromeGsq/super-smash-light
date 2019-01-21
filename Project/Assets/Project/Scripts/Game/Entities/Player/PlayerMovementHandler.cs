using UnityEngine;
using Prime31;
using GamepadInput;
using static GamepadInput.ip_GamePad;
using System;
using DG.Tweening;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovementHandler : MonoBehaviour
{
	private CharacterController2D controller;

	private Vector3 velocity;

	private bool canGrab = true;

	private int jumpsCount;

	private bool isPushed;
	private float pushPower;
	private Transform pushSender;

	private Coroutine dashCoroutine;
	private bool isDashing;
	private bool canDash = true;

	private float savedGravity;
	private float normalizedHorizontalSpeed;

	private GamepadState gamepadState;

	[Tooltip("Cet index permet de choisir via quelle manette ce joueur va être controllé")]
	[SerializeField]
	private Index index = Index.One;

	[Space(20)]

	[Tooltip("Le transform visuel du joueur qui sera flippé selon l'axe 'LocalScaleX'. Ce n'est que visuel")]
	[SerializeField]
	private Transform player;

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

	public bool IsTargeting
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.controller = GetComponent<CharacterController2D>();
		this.gamepadState = new GamepadState();

		this.savedGravity = this.gravity;

		// listen to some events for illustration purposes
		this.controller.onControllerCollidedEvent += this.OnControllerCollider;
		this.controller.onTriggerEnterEvent += this.OnTriggerEnterEvent;
		this.controller.onTriggerExitEvent += this.OnTriggerExitEvent;

		if(this.trigger2D == null)
		{
			this.trigger2D = this.GetComponent<Collider2D>();
			Debug.Log("PlayerHandler : Awake() : trigger2D is null, try to GetComponent<Collider2D>() on it");
		}

		this.trigger2D.isTrigger = true;
	}

	private void OnControllerCollider(RaycastHit2D cast)
	{
		if(this.isDashing && this.isPushed == false && (cast.collider.CompareTag(Tags.Player2) || cast.collider.CompareTag(Tags.Player4)))
		{
			var enemy = cast.collider;
			var enemyPlayerHandler = enemy.GetComponent<PlayerMovementHandler>();

			// Get ball from enemy
			if(BallHandler.Instance.Index == enemyPlayerHandler.index)
			{
				BallHandler.Instance.SetGrabbed(this.ballAnchor, this.index);
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
		if(collision.CompareTag(Tags.Ball))
		{
			if(this.canGrab && BallHandler.Instance.Index == Index.Any)
			{
				BallHandler.Instance.SetGrabbed(this.ballAnchor, this.index);
			}
		}
	}

	// Messages
	public void Collision(Transform sender, float power)
	{
		if(this.isPushed == false)
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
		ip_GamePad.GetState(ref this.gamepadState, this.index);

		if(BallHandler.Instance.Index == this.index)
		{
			// Pass control
			if(this.gamepadState.APressed)
			{
				if(this.IsTargeting)
				{
					BallHandler.Instance.Shoot(this.sight, this.passPower, ShootType.Pass);
				}
				else
				{
					BallHandler.Instance.Shoot(this.friendTransform, this.passPower, ShootType.Pass);
				}

				this.canGrab = false;
				StartCoroutine(CoroutineUtils.DelaySeconds(() =>
				{
					this.canGrab = true;
				}, this.deltaTimeGrab));
			}

			// Shoot control
			if(this.gamepadState.BPressed)
			{
				if(this.IsTargeting)
				{
					BallHandler.Instance.Shoot(this.sight, this.shootPower, ShootType.Shoot);
				}

				this.canGrab = false;
				StartCoroutine(CoroutineUtils.DelaySeconds(() =>
				{
					this.canGrab = true;
				}, this.deltaTimeGrab));
			}
		}

		// Sight control
		this.IsTargeting = this.gamepadState.LT > 0;
		this.sight.transform.localPosition = (Vector3.right * this.gamepadState.LT) * 2.5f;

		if(this.IsTargeting)
		{
			this.sightAnchor.gameObject.SetActive(true);
			this.sightAnchor.eulerAngles = new Vector3(0, 0, Mathf.Atan2(this.gamepadState.LeftStickAxis.y, this.gamepadState.LeftStickAxis.x) * 180 / Mathf.PI);
		}
		else
		{
			this.sightAnchor.gameObject.SetActive(false);
		}

		// Movement & Jump control
		if(this.controller.isGrounded)
		{
			this.jumpsCount = 0;
			this.velocity.y = 0;
		}

		if(this.gamepadState.Right || this.gamepadState.LeftStickAxis.x > 0.1f)
		{
			this.normalizedHorizontalSpeed = 1;
			if(this.player.transform.localScale.x < 0f)
			{
				this.player.transform.localScale = new Vector3(-this.player.transform.localScale.x, this.player.transform.localScale.y, this.player.transform.localScale.z);
			}
		}
		else if(this.gamepadState.Left || this.gamepadState.LeftStickAxis.x < -0.1f)
		{
			this.normalizedHorizontalSpeed = -1;
			if(this.player.transform.localScale.x > 0f)
			{
				this.player.transform.localScale = new Vector3(-this.player.transform.localScale.x, this.player.transform.localScale.y, this.player.transform.localScale.z);
			}
		}
		else
		{
			this.normalizedHorizontalSpeed = 0;
		}

		// Jump control
		if((this.controller.isGrounded || this.jumpsCount < this.maxJumps)
			&& this.gamepadState.YPressed)
		{
			this.jumpsCount++;
			this.velocity.y = Mathf.Sqrt(2f * this.jumpHeight * -this.gravity);
		}

		// Dash control
		if(BallHandler.Instance.Index != this.index
			&& this.isDashing == false
			&& this.gamepadState.RightStickAxis.magnitude > 0.5f
			&& this.canDash)
		{
			this.canDash = false;
			this.isDashing = true;

			var direction = new Vector2(this.gamepadState.RightStickAxis.x, this.gamepadState.RightStickAxis.y);
			this.velocity = (direction.normalized * this.dashDirectionOverdrive) * this.dashSpeed;

			this.savedGravity = this.gravity;
			this.gravity = 0;

			this.player.DOLocalRotate(new Vector3(0, 0, -Mathf.Sign(direction.x) * 360), this.dashDuration * 2, RotateMode.FastBeyond360);

			StartCoroutine(CoroutineUtils.DelaySeconds(() =>
			{
				// Reset gravity and stop dashing
				this.gravity = this.savedGravity;
				this.isDashing = false;
			}, this.dashDuration));

			this.dashCoroutine = StartCoroutine(CoroutineUtils.DelaySeconds(() =>
			{
				if(this.gamepadState.RightStickAxis.magnitude == 0
					&& this.canDash == false
					&& this.isDashing == false)
				{
					// Allow to do a dash
					this.canDash = true;
				}
				this.dashCoroutine = null;
			}, this.dashCoolDown));
		}

		if(this.gamepadState.RightStickAxis.magnitude == 0
			&& this.canDash == false
			&& this.isDashing == false
			&& this.dashCoroutine == null)
		{
			this.canDash = true;
		}

		// Is Pushed
		if(this.isPushed == true && this.pushSender != null)
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

		if(this.gravity == 0)
		{
			// Smooth dash on Y axis without gravity applied
			this.velocity.y = Mathf.Lerp(this.velocity.y, 0, Time.deltaTime * smoothedMovementFactor);
		}
		else
		{
			// Use gravity as normal situation
			this.velocity.y += this.gravity * Time.deltaTime;
		}

		if(this.controller.isGrounded && this.gamepadState.LeftStickAxis.y < 0)
		{
			this.controller.ignoreOneWayPlatformsThisFrame = true;
		}

		this.controller.move(this.velocity * Time.deltaTime);

		this.velocity = this.controller.velocity;
	}
}
