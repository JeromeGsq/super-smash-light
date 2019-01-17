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

	[Space(20)]

	[SerializeField]
	private float gravity = -25f;
	[SerializeField]
	private float runSpeed = 8f;

	[Space(20)]

	[SerializeField]
	private float groundDamping = 20f;
	[SerializeField]
	private float inAirDamping = 5f;

	[Space(20)]

	[SerializeField]
	private float jumpHeight = 3f;
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
		if(this.gamepadState.LeftStickAxis.sqrMagnitude != 0)
		{
			this.sightAnchor.gameObject.SetActive(true);
			this.sightAnchor.eulerAngles = new Vector3(0, 0, Mathf.Atan2(this.gamepadState.LeftStickAxis.y, this.gamepadState.LeftStickAxis.x) * 180 / Mathf.PI);
		}
		else
		{
			this.sightAnchor.gameObject.SetActive(false);
		}


		// Movement control
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

		if((this.controller.isGrounded || this.jumpsCount < this.maxJumps)
			&& this.gamepadState.YPressed)
		{
			this.jumpsCount++;
			this.velocity.y = Mathf.Sqrt(2f * this.jumpHeight * -this.gravity);
		}

		var smoothedMovementFactor = this.controller.isGrounded ? this.groundDamping : this.inAirDamping;
		this.velocity.x = Mathf.Lerp(this.velocity.x, normalizedHorizontalSpeed * this.runSpeed, Time.deltaTime * smoothedMovementFactor);

		this.velocity.y += this.gravity * Time.deltaTime;

		if(this.controller.isGrounded && Input.GetKey(KeyCode.DownArrow))
		{
			this.velocity.y *= 3f;
			this.controller.ignoreOneWayPlatformsThisFrame = true;
		}

		this.controller.move(this.velocity * Time.deltaTime);

		this.velocity = this.controller.velocity;
	}
}
