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

		transform.position = Vector3.SmoothDamp(transform.position - Vector3.forward, center, ref _smoothDampVelocity, smoothDampTime);
	}
}
