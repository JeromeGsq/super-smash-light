using UnityEngine;
using System.Collections.Generic;
using Root.DesignPatterns;

public class SmoothFollow : SceneSingleton<SmoothFollow>
{
	public List<Transform> Targets
	{
		get; set;
	} = new List<Transform>();

	[SerializeField]
	private float smoothDampTime = 0.2f;

	private Vector2 center = new Vector3();

	private void FixedUpdate()
	{
		this.center = Vector2.zero;
		foreach(var target in this.Targets)
		{
			this.center += (Vector2)target.transform.position;
		}

		this.center = this.center / this.Targets.Count;

		float interpolation = this.smoothDampTime * Time.deltaTime;

		Vector3 position = this.transform.position;
		position.y = Mathf.Lerp(this.transform.position.y, this.center.y, interpolation);
		position.x = Mathf.Lerp(this.transform.position.x, this.center.x, interpolation);
		this.transform.position = position + new Vector3(0,0.25f,0);
    }
}
