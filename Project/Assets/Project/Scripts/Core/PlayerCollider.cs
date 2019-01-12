using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
	public GameObject Ball;
	public GameObject Target;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Ball"))
		{
			// Do you GetComponent once and store Ball and Player in local var
			if(other.gameObject.GetComponent<Ball>().IsSmashing() && other.gameObject.GetComponent<Ball>().LastOwner != transform.parent.GetComponent<Player>().Team)
			{
				Debug.Log("-- Touched : " + other.name);
			}

			other.gameObject.GetComponent<Ball>().GetBall(this.gameObject);
		}
	}
}
