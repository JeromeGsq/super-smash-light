using System;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class GameManager : BaseViewModel
{
	#region Singleton
	private static GameManager instance;

	public static GameManager Get
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}
	#endregion

	private float barLevel = 0;

	[Binding]
	public float BarLevel
	{
		get => this.barLevel;
		set
		{
			this.Set(ref this.barLevel, value, nameof(this.BarLevel));
			this.RaisePropertyChanged(nameof(this.BarLevelText));
		}
	}

	[Binding]
	public string BarLevelText
	{
		get => $"Niveau : {this.BarLevel * 100}%";
	}

	private void Start()
	{
		this.BarLevel = 0f;
	}

	public void AddBarLevel(float amount)
	{
		amount = (float)Math.Round(amount, 2);
		Debug.Log($"GameManager : AddBarLevel() : {amount})");
		this.barLevel += amount;

		this.BarLevel = (float)Math.Round(this.BarLevel, 2);

		if(this.BarLevel > 1)
		{
			this.BarLevel = 1;
		}

	}

	public void ResetBarLevel()
	{
		this.BarLevel = 0;
	}

	public bool CanShoot()
	{
		return this.BarLevel == 1;
	}
}
