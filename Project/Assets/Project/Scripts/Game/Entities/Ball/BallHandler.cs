using Prime31;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class BallHandler : MonoBehaviour
{
	private CharacterController2D controller;

	private Vector3 velocity;
	private float normalizedHorizontalSpeed;

	[SerializeField]
	private float gravity = -25f;

	private void Awake()
	{
		this.controller = this.GetComponent<CharacterController2D>();
	}

	private void Update()
	{
		this.velocity.x = Mathf.Lerp(this.velocity.x, normalizedHorizontalSpeed, Time.deltaTime);
		this.velocity.y += this.gravity * Time.deltaTime;

		this.controller.move(this.velocity * Time.deltaTime);
	}
}
