using System;
using Root.DesignPatterns;
using UnityEngine;
using static GamepadInput.ip_GamePad;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BallHandler : SceneSingleton<BallHandler>
{
	private Rigidbody2D rigidbody;
	private Collider2D collider;

	private Index index = Index.Any;

	private bool isGrabbed = false;
	private bool engagePass = false;
	private bool engageShoot = false;

	private Vector3 lastKnownShootPosition;
	private Index lastShooter = Index.Any;

	[Space(20)]

	[SerializeField]
	private TrailRenderer trail;

	[Space(20)]

	[SerializeField]
	private Material blue;
	[SerializeField]
	private Material red;
	[SerializeField]
	private Material yellow;
	[SerializeField]
	private Material white;

	[Space(20)]

	[Tooltip("Augmenter cette valeur pour gagner plus de % à chaque passes (defaut : 1)")]
	[SerializeField]
	private int barLevelAddScale = 1;

	public Index Index
	{
		get => this.index;
		set {
			this.index = value;
			this.UpdateGameManagerBallIndex();
		}
	}

	public bool EngagePass
	{
		get => this.engagePass;
		set => this.engagePass = value;
	}

	public bool EngageShoot
	{
		get => this.engageShoot;
		set => this.engageShoot = value;
	}

	public Index LastShooter => this.lastShooter;

	public bool IsGrabbed => this.isGrabbed;

	private void Awake()
	{
		this.rigidbody = this.GetComponent<Rigidbody2D>();
		this.collider = this.GetComponent<Collider2D>();

		this.lastKnownShootPosition = this.transform.position;
	}

	private void Update()
	{
		this.rigidbody.bodyType = this.isGrabbed ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
	}

	public void SetGrabbed(Transform ballAnchor, Index index)
	{
		this.collider.isTrigger = true;
		this.Index = index;
		this.transform.SetParent(ballAnchor);
		this.transform.localPosition = Vector3.zero;
		this.isGrabbed = true;
		this.trail.material = Team.GetTeam(this.Index) == 1 ? this.blue : this.red;

		// if the last shooter is my teammate
		if(Team.GetTeam(this.Index) == Team.GetTeam(this.lastShooter)
			&& this.engagePass == true)
		{
			GameManager.Get.AddBarLevel(
				Mathf.Abs(Vector3.Distance(this.lastKnownShootPosition, ballAnchor.position)) * ((float)this.barLevelAddScale / 100),
				Team.GetTeam(this.Index)
			);

			this.engagePass = false;
		}

		this.lastShooter = index;
	}

	public void Shoot(Transform target, float power, ShootType shootType)
	{
		if(target == null)
		{
			Debug.LogError("BallHandler : Shoot() : Unable to find target. Stop Shoot()");
			return;
		}

		this.lastKnownShootPosition = this.transform.position;

		this.transform.SetParent(null);
		Vector3 dir = (target.position - this.transform.position).normalized;

		switch(shootType)
		{
			case ShootType.Pass:
				this.trail.material = Team.GetTeam(this.Index) == 1 ? this.blue : this.red;
				this.rigidbody.gravityScale = 0;
				// this.Index = Index.Any;
				this.engagePass = true;
				break;

			case ShootType.Shoot:
				this.trail.material = this.yellow;
				this.rigidbody.gravityScale = 0;
				GameManager.Get.ResetBarLevel(Team.GetTeam(this.Index));

				this.engageShoot = true;
				break;

			case ShootType.Loose:
				this.trail.material = this.white;
				this.Index = Index.Any;
				break;
		}

		this.collider.isTrigger = false;

		this.rigidbody.bodyType = RigidbodyType2D.Dynamic;
		this.rigidbody.AddForce(dir * power, ForceMode2D.Impulse);

		this.isGrabbed = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.collider.CompareTag(Tags.Wall))
		{
			this.rigidbody.gravityScale = 1;
			this.engagePass = false;
			this.engageShoot = false;
			this.Index = Index.Any;
			this.trail.material = this.white;
		}
	}

	private void UpdateGameManagerBallIndex()
	{
		GameManager.Get.SetBallIndex(this.Index);
	}
}
