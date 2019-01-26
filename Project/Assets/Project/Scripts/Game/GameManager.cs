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

	private Team team1;
	private Team team2;

	private float barLevel;

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

	[Binding]
	public string Team1Score
	{
		get => $"Team bleu : {this.Team1.Score}";
	}

	[Binding]
	public string Team2Score
	{
		get => $"Team rouge : {this.Team2.Score}";
	}

	public Team Team1
	{
		get => this.team1;
		set => this.Set(ref this.team1, value, nameof(this.Team1));
	}

	public Team Team2
	{
		get => this.team2;
		set => this.Set(ref this.team2, value, nameof(this.Team2));
	}

	private void Awake()
	{
		this.Team1 = new Team()
		{
			TeamIndex = 1,
			FirstPlayerIndex = GamepadInput.ip_GamePad.Index.One,
			SecondPlayerIndex = GamepadInput.ip_GamePad.Index.Three,
		};

		this.Team2 = new Team()
		{
			TeamIndex = 2,
			FirstPlayerIndex = GamepadInput.ip_GamePad.Index.Two,
			SecondPlayerIndex = GamepadInput.ip_GamePad.Index.Four,
		};

		this.BarLevel = 1f;
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

	public void AddPoint(int teamIndex)
	{
		switch(teamIndex)
		{
			case 1:
				this.Team1.Score++;
				break;

			case 2:
				this.Team2.Score++;
				break;
		}

		this.RaisePropertyChanged(nameof(this.Team1Score));
		this.RaisePropertyChanged(nameof(this.Team2Score));
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
