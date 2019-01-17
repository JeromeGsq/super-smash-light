using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerMovementHandler))]
public class PlayerHandler : MonoBehaviour
{
	private const string BallTag = "Ball";

	private PlayerMovementHandler playerMovementHandler;

	[SerializeField]
	private Collider2D trigger2D;

	[Space(20)]

	[SerializeField]
	private Transform ballAnchor;

	private void Awake()
	{
		this.playerMovementHandler = this.GetComponent<PlayerMovementHandler>();

		if(this.trigger2D == null)
		{
			this.trigger2D = this.GetComponent<Collider2D>();
			Debug.Log("PlayerHandler : Awake() : trigger2D is null, try to GetComponent<Collider2D>() on it");
		}

		this.trigger2D.isTrigger = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag(BallTag))
		{
			Debug.Log($"PlayerHandler : OnTriggerEnter2D() : {collision.name}");
			collision.GetComponent<BallHandler>()?.SetGrabbed(this.ballAnchor, this.playerMovementHandler.Index);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// Debug.Log($"PlayerHandler : OnTriggerExit2D() : {collision.name}");
	}

	private void Update()
	{
		// Pass control
		if(this.playerMovementHandler.GamepadState.APressed &&
			BallHandler.Instance.Index == this.playerMovementHandler.Index)
		{
			BallHandler.Instance.Shoot(this.playerMovementHandler.FriendTransform, 30);
		}

		// Shoot control
		if(this.playerMovementHandler.GamepadState.BPressed)
		{

		}

		// Hit control
		if(this.playerMovementHandler.GamepadState.XPressed)
		{

		}
	}
}
