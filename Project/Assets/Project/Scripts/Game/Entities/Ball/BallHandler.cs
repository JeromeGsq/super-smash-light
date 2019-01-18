using System;
using Prime31;
using Root.DesignPatterns;
using UnityEngine;
using static GamepadInput.ip_GamePad;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BallHandler : SceneSingleton<BallHandler>
{
	private Rigidbody2D rigidbody;
	private Collider2D collider;

	private bool isGrabbed = false;
	private Index index = Index.Any;

	[SerializeField]
	private TrailRenderer trail;

	[SerializeField]
	private Material blue;
	[SerializeField]
	private Material red;
	[SerializeField]
	private Material yellow;
	[SerializeField]
	private Material white;


	public Index Index
	{
		get
		{
			return this.index;
		}
	}

	private void Awake()
	{
		this.rigidbody = this.GetComponent<Rigidbody2D>();
		this.collider = this.GetComponent<Collider2D>();
	}

	private void Update()
	{
		this.rigidbody.bodyType = this.isGrabbed ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
	}

	public void SetGrabbed(Transform ballAnchor, Index index)
	{
		this.index = index;
		this.transform.SetParent(ballAnchor);
		this.transform.localPosition = Vector3.zero;
		this.isGrabbed = true;
		this.trail.material = (int)this.index % 2 == 0 ? this.red : this.blue;
	}

	public void Shoot(Transform target, float power, ShootType shootType)
	{
		this.transform.SetParent(null);
		Vector3 dir = (target.position - this.transform.position).normalized;

		Debug.DrawLine(this.transform.position, this.transform.position + dir * 10, Color.red, Mathf.Infinity);
		Debug.Log($"BallHandler : {dir}");

		this.rigidbody.bodyType = RigidbodyType2D.Dynamic;
		this.rigidbody.AddForce(dir * power, ForceMode2D.Impulse);

		switch(shootType)
		{
			case ShootType.Pass:
				this.trail.material = (int)this.index%2 == 0 ? this.red : this.blue;
				break;
			case ShootType.Shoot:
				this.trail.material = this.yellow;

				break;
			case ShootType.Loose:
				this.trail.material = this.white;
				break;
		}

		this.isGrabbed = false;
		this.index = Index.Any;
	}
}
