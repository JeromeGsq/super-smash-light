using System;
using GamepadInput;
using UnityEngine;
using UnityWeld.Binding;
using static GamepadInput.ip_GamePad;

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

	private Index ballIndex;

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

	[Binding]
	public Index BallIndex
	{
		get => this.ballIndex;
		set => this.Set(ref this.ballIndex, value, nameof(this.BallIndex));
	}

	[Binding]
	public float Team1BarLevel
	{
		get => this.team1?.BarLevel ?? 0;
	}

	[Binding]
	public float Team2BarLevel
	{
		get => this.team2?.BarLevel ?? 0;
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

	private void Awake()
	{
		this.Team1 = new Team()
		{
			FirstPlayerIndex = GamepadInput.ip_GamePad.Index.One,
			SecondPlayerIndex = GamepadInput.ip_GamePad.Index.Three,
		};

		this.Team2 = new Team()
		{
			FirstPlayerIndex = GamepadInput.ip_GamePad.Index.Two,
			SecondPlayerIndex = GamepadInput.ip_GamePad.Index.Four,
		};

		this.team1.BarLevel = 0f;
		this.team2.BarLevel = 0f;

		this.RaisePropertyChanged(nameof(this.Team1BarLevel));
		this.RaisePropertyChanged(nameof(this.Team2BarLevel));
	}

	public void AddBarLevel(float amount, int teamIndex)
	{
		amount = (float)Math.Round(amount, 2);
		Debug.Log($"GameManager : AddBarLevel() : {amount})");

		if(teamIndex == 1)
		{
			this.Team1.BarLevel += amount;
			this.Team1.BarLevel = (float)Math.Round(this.Team1.BarLevel, 2);

			if(this.Team1.BarLevel > 1)
			{
				this.Team1.BarLevel = 1;
			}
		}
		else
		{
			this.Team2.BarLevel += amount;
			this.Team2.BarLevel = (float)Math.Round(this.Team2.BarLevel, 2);

			if(this.Team2.BarLevel > 1)
			{
				this.Team2.BarLevel = 1;
			}
		}

		this.RaisePropertyChanged(nameof(this.Team1BarLevel));
		this.RaisePropertyChanged(nameof(this.Team2BarLevel));
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

	public void ResetBarLevel(int teamIndex)
	{
		if(teamIndex == 1)
		{
			this.Team1.BarLevel = 0;
		}
		else
		{
			this.Team2.BarLevel = 0;
		}

		this.RaisePropertyChanged(nameof(this.Team1BarLevel));
		this.RaisePropertyChanged(nameof(this.Team2BarLevel));
	}

	public bool CanShoot(int teamIndex)
	{
		if(teamIndex == 1)
		{
			return this.Team1.BarLevel == 1;
		}
		else
		{
			return this.Team2.BarLevel == 1;
		}
	}

	public void SetBallIndex(ip_GamePad.Index index)
	{
		this.BallIndex = index;
	}
}
