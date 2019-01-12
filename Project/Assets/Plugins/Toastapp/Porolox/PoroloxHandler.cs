using Root.DesignPatterns;
using System.Collections.Generic;
using UnityEngine;

public class PoroloxHandler : GlobalSingleton<PoroloxHandler> {

	private Vector2 deltaPosition;
	public float deltaBoost = 1f;

	public List<Transform> backPlans;
	public List<Transform> frontPlans;
	public List<Transform> mainPlans;

	[Space(20)]

	public Camera MainCamera;

	[Space(20)]

	[Range(0.0f, 10.0f)]
	public float ParalaxMultiplyX = 5;
	[Range(0.0f, 10.0f)]
	public float ParalaxMultiplyY = 5;

	[Space(20)]

	public List<Transform> TransformsToFollow;

	private void Awake()
	{
		this.InitCamera();
		this.LoadPlans();
	}

	private void InitCamera()
	{
		this.transform.position = new Vector3(0, 0, 0);
		this.MainCamera.transform.position = new Vector3(0, 0, -1);
	}

	private void LoadPlans()
	{
		this.backPlans = new List<Transform>(this.transform.Find("Back").GetComponentsInChildren<Transform>());
		this.frontPlans = new List<Transform>(this.transform.Find("Front").GetComponentsInChildren<Transform>());
		this.mainPlans = new List<Transform>(this.transform.Find("Main").GetComponentsInChildren<Transform>());
	}

	private void LateUpdate()
	{
		this.deltaPosition = TransformExtension.CenterOfVectors(this.TransformsToFollow);

		for(int i = 0; i < this.backPlans.Count; i++)
		{
			this.backPlans[i].transform.localPosition = new Vector2(this.deltaPosition.x * this.ParalaxMultiplyX, this.deltaPosition.y * this.ParalaxMultiplyY) * (i+1) * this.deltaBoost;
		}

		for(int i = 0; i < this.frontPlans.Count; i++)
		{
			this.frontPlans[i].transform.localPosition = -(new Vector2(this.deltaPosition.x * this.ParalaxMultiplyX, this.deltaPosition.y * this.ParalaxMultiplyY) * (i + 1)) * this.deltaBoost;
		}
	}
}

