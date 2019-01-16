using UnityEngine;
using Prime31;
using GamepadInput;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerHandler : MonoBehaviour
{
	private CharacterController2D controller;
	private Vector3 velocity;

	private int jumpsCount;
	private float normalizedHorizontalSpeed;

	private GamepadState gamepadState;

	[SerializeField]
	private ip_GamePad.Index ControllerIndex = ip_GamePad.Index.One;

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
		Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
	}

	private void OnTriggerExitEvent(Collider2D col)
	{
		Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
	}
	#endregion

	private void Update()
	{
		ip_GamePad.GetState(ref this.gamepadState, this.ControllerIndex);

		if(this.controller.isGrounded)
		{
			this.jumpsCount = 0;
			this.velocity.y = 0;
		}

		if(this.gamepadState.Right || this.gamepadState.LeftStickAxis.x > 0)
		{
			this.normalizedHorizontalSpeed = 1;
			if(this.transform.localScale.x < 0f)
			{
				this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
			}
		}
		else if(this.gamepadState.Left || this.gamepadState.LeftStickAxis.x < 0)
		{
			this.normalizedHorizontalSpeed = -1;
			if(this.transform.localScale.x > 0f)
			{
				this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
			}
		}
		else
		{
			this.normalizedHorizontalSpeed = 0;
		}

		if( (this.controller.isGrounded || this.jumpsCount < this.maxJumps)
			&& this.gamepadState.APressed)
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
