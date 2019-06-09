using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;
using XInputDotNetPure;

[Binding]

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GameManager))]
public class GameManagerView : BaseView<GameManager>
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    [Space(20)]
    [SerializeField]
    private GameManager gamemanager;

	[Space(20)]
    [SerializeField]
    private GameObject countdownPrefab;


    [Space(10)]
    [SerializeField]
	private GameObject ballPrefab;

	[Space(10)]

	[SerializeField]
	private GameObject playerPrefab;

	[Space(20)]

	[SerializeField]
	private Transform spawnBallAnchor;

	[Space(10)]

	[SerializeField]
	private Transform spanwnPlayer1Anchor;

	[SerializeField]
	private Transform spanwnPlayer3Anchor;

	[Space(10)]

	[SerializeField]
	private Transform spanwnPlayer2Anchor;

	[SerializeField]
	private Transform spanwnPlayer4Anchor;

	[Space(20)]

	[SerializeField]
	private Image ballImage;

	private List<GameObject> players;


    public override void Awake()
	{
		base.Awake();

		this.players = new List<GameObject>();
	}

	public override void Start()
	{
        
		base.Start();
        this.InitBall();
        GameObject countdown = Instantiate(this.countdownPrefab);
        StartCoroutine(CoroutineUtils.DelaySeconds(() =>
        {
            gamemanager.timerStart = true;
            this.InitTeam(this.ViewModel?.Team1, 1);
            this.InitTeam(this.ViewModel?.Team2, 2);
            Destroy(countdown);
        }, 4));
        
	}
    public override void OnPropertyChanged(object sender, PropertyChangedEventArgs property)
	{
		base.OnPropertyChanged(sender, property);

		if(property.PropertyName.Equals(nameof(this.ViewModel.Team1)))
		{
			this.InitTeam(this.ViewModel?.Team1, 1);
		}
		else if(property.PropertyName.Equals(nameof(this.ViewModel.Team2)))
		{
			this.InitTeam(this.ViewModel?.Team2, 2);
		}
		else if(property.PropertyName.Equals(nameof(this.ViewModel.BallIndex)))
		{
			//this.UpdateBallColor(this.ViewModel.BallIndex);
		}
	}
	
	private void InitBall()
	{
		var ball = Instantiate(this.ballPrefab);
		ball.transform.position = this.spawnBallAnchor.position;

		SmoothFollow.Get.Targets.Add(ball.transform);
	}

	private void InitTeam(Team team, int index)
	{
		if(team == null)
		{
			Debug.Log("GameManagerView : InitTeam() : team is null");
			return;
		}

		if(index == 1)
		{
			var player1 = InitPlayer(this.spanwnPlayer1Anchor, team.FirstPlayerIndex, Tags.Player1,1);
			var player2 = InitPlayer(this.spanwnPlayer2Anchor, team.SecondPlayerIndex, Tags.Player2,1);

			player1.FriendTransform = player2.gameObject.transform;
			player2.FriendTransform = player1.gameObject.transform;

			this.players.Add(player1.gameObject);
			this.players.Add(player2.gameObject);
            this.player1 = player1.gameObject;
            this.player2 = player2.gameObject;
        }

		if(index == 2)
		{
			var player3 = InitPlayer(this.spanwnPlayer3Anchor, team.FirstPlayerIndex, Tags.Player3,2);
			var player4 = InitPlayer(this.spanwnPlayer4Anchor, team.SecondPlayerIndex, Tags.Player4,2);

			player3.FriendTransform = player4.gameObject.transform;
			player4.FriendTransform = player3.gameObject.transform;

			this.players.Add(player3.gameObject);
			this.players.Add(player4.gameObject);
            this.player3 = player3.gameObject;
            this.player4 = player4.gameObject;
        }
        
	}

	private PlayerMovementHandler InitPlayer(Transform anchor, PlayerIndex index, string tag,int team)
	{
		GameObject player = Instantiate(this.playerPrefab, null);
		player.transform.position = anchor.position;

		var playerHandler = player.GetComponent<PlayerMovementHandler>();
		playerHandler.Index = index;
        playerHandler.myteam = team;
        player.tag = tag;
        


        playerHandler.MainPosition = player.transform.position;

		SmoothFollow.Get.Targets.Add(player.transform);

		return playerHandler;
	}

	//private void UpdateBallColor(PlayerIndex ballIndex)
	//{
	//  var teamIndex = Team.GetTeam(ballIndex);
    //  var color = Color.white;

	//	if(ballIndex != PlayerIndex.One)
	//	{
	//		color = teamIndex == 1 ? Color.blue : Color.red;
	//	}

		//this.ballImage.color = color;
	//}
}
