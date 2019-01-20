using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerMovementHandler))]
public class PlayerHandler : MonoBehaviour
{
	private const string BallTag = "Ball";

	private PlayerMovementHandler playerMovementHandler;

	private bool canGrab = true;


	[SerializeField]
	private Collider2D trigger2D;

	[Space(20)]

	[SerializeField]
	private float deltaTimeGrab = 0.2f;

	[Space(20)]

	[SerializeField]
	private float passPower = 50f;

	[SerializeField]
	private float shootPower = 50f;

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
			if(this.canGrab)
			{
				collision.GetComponent<BallHandler>()?.SetGrabbed(this.ballAnchor, this.playerMovementHandler.Index);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// Debug.Log($"PlayerHandler : OnTriggerExit2D() : {collision.name}");
	}

	private void Update()
	{
		if(BallHandler.Instance.Index == this.playerMovementHandler.Index)
		{
			// Pass control
			if(this.playerMovementHandler.GamepadState.APressed)
			{
				if(this.playerMovementHandler.IsTargeting)
				{
					BallHandler.Instance.Shoot(this.playerMovementHandler.Sight, this.passPower, ShootType.Pass);
				}
				else
				{
					BallHandler.Instance.Shoot(this.playerMovementHandler.FriendTransform, this.passPower, ShootType.Pass);
				}

				this.canGrab = false;
				StartCoroutine(CoroutineUtils.DelaySeconds(() =>
				{
					this.canGrab = true;
				}, this.deltaTimeGrab));
			}

			// Shoot control
			if(this.playerMovementHandler.GamepadState.BPressed)
			{
				if(this.playerMovementHandler.IsTargeting)
				{
					BallHandler.Instance.Shoot(this.playerMovementHandler.Sight, this.shootPower, ShootType.Shoot);
				}

				this.canGrab = false;
				StartCoroutine(CoroutineUtils.DelaySeconds(() =>
				{
					this.canGrab = true;
				}, this.deltaTimeGrab));
			}
		}

		// Hit control
		if(this.playerMovementHandler.GamepadState.XPressed)
		{

		}
	}
}
