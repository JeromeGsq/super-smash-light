using UnityEngine;
using Prime31;
using GamepadInput;
using static GamepadInput.ip_GamePad;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovementHandler : MonoBehaviour
{
	private CharacterController2D controller;
	private Vector3 velocity;

	private int jumpsCount;
	private bool canDash = true;
	private float normalizedHorizontalSpeed;

	private GamepadState gamepadState;

	[SerializeField]
	private Index controllerIndex = Index.One;

	[Space(20)]

	[SerializeField]
	private Transform player;

	[SerializeField]
	private Transform friendTransform;

	[SerializeField]
	private Transform sightAnchor;

	[SerializeField]
	private Transform sight;

	[Space(20)]

	[SerializeField]
	private float gravity = -60f;
	[SerializeField]
	private float runSpeed = 13f;
	[SerializeField]
	private float dashSpeed = 16f;
	[SerializeField]
	private float dashCoolDown = 1f;

	[Space(20)]

	[SerializeField]
	private float groundDamping = 10f;
	[SerializeField]
	private float inAirDamping = 6f;

	[Space(20)]

	[SerializeField]
	private float jumpHeight = 4f;
	[SerializeField]
	private int maxJumps = 2;

	public GamepadState GamepadState
	{
		get
		{
			return this.gamepadState;
		}
	}

	public Transform FriendTransform
	{
		get
		{
			return this.friendTransform;
		}
	}

	public Transform Sight
	{
		get
		{
			return this.sight;
		}
	}

	public bool IsTargeting
	{
		get;
		private set;
	}


	public Index Index
	{
		get
		{
			return this.controllerIndex;
		}
	}

	private void Awake()
	{
		this.controller = GetComponent<CharacterController2D>();

		this.gamepadState = new GamepadState();

		this.controller.onControllerCollidedEvent += this.OnControllerCollider;
		this.controller.onTriggerEnterEvent += this.OnTriggerEnterEvent;
		this.controller.onTriggerExitEvent += this.OnTriggerExitEvent;
	}

	private void OnDestroy()
	{
		this.controller.onControllerCollidedEvent -= this.OnControllerCollider;
		this.controller.onTriggerEnterEvent -= this.OnTriggerEnterEvent;
		this.controller.onTriggerExitEvent -= this.OnTriggerExitEvent;
	}

	#region Event Listeners Methods
	private void OnControllerCollider(RaycastHit2D hit)
	{
		// bail out on plain old ground hits cause they arent very interesting
		if(hit.normal.y == 1f)
		{
			return;
		}

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}

	private void OnTriggerEnterEvent(Collider2D col)
	{
		// Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
	}

	private void OnTriggerExitEvent(Collider2D col)
	{
		// Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
	}
	#endregion

	private void Update()
	{
		ip_GamePad.GetState(ref this.gamepadState, this.controllerIndex);

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

		if(this.gamepadState.Right || this.gamepadState.LeftStickAxis.x > 0)
		{
			this.normalizedHorizontalSpeed = 1;
			if(this.player.transform.localScale.x < 0f)
			{
				this.player.transform.localScale = new Vector3(-this.player.transform.localScale.x, this.player.transform.localScale.y, this.player.transform.localScale.z);
			}
		}
		else if(this.gamepadState.Left || this.gamepadState.LeftStickAxis.x < 0)
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

		var smoothedMovementFactor = this.controller.isGrounded ? this.groundDamping : this.inAirDamping;
		this.velocity.x = Mathf.Lerp(this.velocity.x, normalizedHorizontalSpeed * this.runSpeed, Time.deltaTime * smoothedMovementFactor);

		this.velocity.y += this.gravity * Time.deltaTime;

		// Dash control
		if(this.gamepadState.XPressed && this.canDash == true)
		{
			this.canDash = false;
			var direction = new Vector2(this.gamepadState.LeftStickAxis.x, this.gamepadState.LeftStickAxis.y + this.gravity);
			// Fully override velocity
			this.velocity = direction * this.dashSpeed;
			StartCoroutine(CoroutineUtils.DelaySeconds(() =>
			{
				this.canDash = true;
			}, this.dashCoolDown));
		}

		if(this.controller.isGrounded && this.gamepadState.LeftStickAxis.y < 0)
		{
			this.controller.ignoreOneWayPlatformsThisFrame = true;
		}

		this.controller.move(this.velocity * Time.deltaTime);

		this.velocity = this.controller.velocity;
	}
}
