using UnityEngine;
using System.Collections;
using Prime31;
using System.Collections.Generic;

public class SmoothFollow : MonoBehaviour
{
	public List<Transform> targets;

	public float smoothDampTime = 0.2f;

	private Vector3 _smoothDampVelocity;
	private	Vector2 center = new Vector3();

	private void FixedUpdate()
	{
		center = Vector2.zero;
		foreach(var target in targets)
		{
			center += (Vector2)target.transform.position;
		}
		center = center / targets.Count;

		float interpolation = smoothDampTime * Time.deltaTime;

		Vector3 position = this.transform.position;
		position.y = Mathf.Lerp(this.transform.position.y, center.y, interpolation);
		position.x = Mathf.Lerp(this.transform.position.x, center.x, interpolation);
		this.transform.position = position;
	}
}
