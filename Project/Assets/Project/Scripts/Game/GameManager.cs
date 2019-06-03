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
    [Space(20)]
    [SerializeField]
    private GameObject endPanel;

    [Space(20)]
    [SerializeField]
    private GameManagerView gamemanagerview;

    [Space(10)]
    [SerializeField]
    private float timer;

    private float minutes;
    private float seconds;

    [Space(10)]
    [SerializeField]
    public bool timerStart;


    public Team team1;
	public Team team2;

    public bool forTest;

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
    [Binding]
    public string theTime {
        get => $" {this.minutes} : {this.seconds}";
    }
    private Index team1Player1;
    private Index team1Player2;
    private Index team2Player1;
    private Index team2Player2;

    private void Awake()
	{
        if(!forTest) {

            if((GameMenuManager2.gamepad1team + GameMenuManager2.gamepad2team) == 2) {
                team1Player1 = GamepadInput.ip_GamePad.Index.One;
                team1Player2 = GamepadInput.ip_GamePad.Index.Two;
            }
            if((GameMenuManager2.gamepad1team + GameMenuManager2.gamepad2team) == 4) {
                team2Player1 = GamepadInput.ip_GamePad.Index.One;
                team2Player2 = GamepadInput.ip_GamePad.Index.Two;
            }
            if((GameMenuManager2.gamepad1team + GameMenuManager2.gamepad3team) == 2) {
                team1Player1 = GamepadInput.ip_GamePad.Index.One;
                team1Player2 = GamepadInput.ip_GamePad.Index.Three;
            }
            if((GameMenuManager2.gamepad1team + GameMenuManager2.gamepad3team) == 4) {
                team2Player1 = GamepadInput.ip_GamePad.Index.One;
                team2Player2 = GamepadInput.ip_GamePad.Index.Three;
            }
            if((GameMenuManager2.gamepad1team + GameMenuManager2.gamepad4team) == 2) {
                team1Player1 = GamepadInput.ip_GamePad.Index.One;
                team1Player2 = GamepadInput.ip_GamePad.Index.Four;
            }
            if((GameMenuManager2.gamepad1team + GameMenuManager2.gamepad4team) == 4) {
                team2Player1 = GamepadInput.ip_GamePad.Index.One;
                team2Player2 = GamepadInput.ip_GamePad.Index.Four;
            }
            if((GameMenuManager2.gamepad2team + GameMenuManager2.gamepad3team) == 2) {
                team1Player1 = GamepadInput.ip_GamePad.Index.Two;
                team1Player2 = GamepadInput.ip_GamePad.Index.Three;
            }
            if((GameMenuManager2.gamepad2team + GameMenuManager2.gamepad3team) == 4) {
                team2Player1 = GamepadInput.ip_GamePad.Index.Two;
                team2Player2 = GamepadInput.ip_GamePad.Index.Three;
            }
            if((GameMenuManager2.gamepad2team + GameMenuManager2.gamepad4team) == 2) {
                team1Player1 = GamepadInput.ip_GamePad.Index.Two;
                team1Player2 = GamepadInput.ip_GamePad.Index.Four;
            }
            if((GameMenuManager2.gamepad2team + GameMenuManager2.gamepad4team) == 4) {
                team2Player1 = GamepadInput.ip_GamePad.Index.Two;
                team2Player2 = GamepadInput.ip_GamePad.Index.Four;
            }
            if((GameMenuManager2.gamepad3team + GameMenuManager2.gamepad4team) == 2) {
                team1Player1 = GamepadInput.ip_GamePad.Index.Three;
                team1Player2 = GamepadInput.ip_GamePad.Index.Four;
            }
            if((GameMenuManager2.gamepad3team + GameMenuManager2.gamepad4team) == 4) {
                team2Player1 = GamepadInput.ip_GamePad.Index.Three;
                team2Player2 = GamepadInput.ip_GamePad.Index.Four;
            }

            this.Team1 = new Team() {

                FirstPlayerIndex = team1Player1,
                SecondPlayerIndex = team1Player2,
            };

            this.Team2 = new Team() {
                FirstPlayerIndex = team2Player1,
                SecondPlayerIndex = team2Player2,
            };
        }
    
        if(forTest) 
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
        }

            this.team1.BarLevel = 0f;
		this.team2.BarLevel = 0f;

		this.RaisePropertyChanged(nameof(this.Team1BarLevel));
		this.RaisePropertyChanged(nameof(this.Team2BarLevel));
	}

    private void Update()
    {

        //this.team1.BarLevel = 1;
        //this.team2.BarLevel = 1;
        if(timerStart && timer > 1) {

            timer -= Time.deltaTime;
        }
        this.RaisePropertyChanged(nameof(this.theTime));

        minutes = Mathf.Floor(timer / 60);
        seconds = Mathf.Floor(timer % 60);

        if(timer <= 1) {
            endPanel.SetActive(true);
            gamemanagerview.player1.SetActive(false);
            gamemanagerview.player2.SetActive(false);
            gamemanagerview.player3.SetActive(false);
            gamemanagerview.player4.SetActive(false);
        }
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
                Debug.Log("Teamadd1");

                break;

			case 2:
				this.Team2.Score++;
                Debug.Log("Teamadd1");
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
