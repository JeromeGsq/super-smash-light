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
    public GameObject ProjectionReference1;
    public GameObject ProjectionReference2;
    public GameObject ProjectionReference3;
    public GameObject ProjectionReference4;

    [SerializeField]
    private int debugLevel = 1;

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

    [SerializeField]
    private GameObject playerPrefabTeam11;

    [SerializeField]
    private GameObject playerPrefabTeam12;

    [SerializeField]
    private GameObject playerPrefabTeam21;

    [SerializeField]
    private GameObject playerPrefabTeam22;

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
    
    [Space(10)]
    [SerializeField]
    private GameObject lv1;

    [Space(10)]
    [SerializeField]
    private GameObject lv2;

    [Space(10)]
    [SerializeField]
    private GameObject lv3;

    private List<GameObject> players;



    public override void Awake()
	{
		base.Awake();

		this.players = new List<GameObject>();
	}

	public override void Start()
	{
        
		base.Start();
        GameObject currentLevel = null;
        if (GameMenuManager3.selectedLevel == 0)
            GameMenuManager3.selectedLevel = debugLevel;
        if (GameMenuManager3.selectedLevel == 1)
        {
           currentLevel = Instantiate(lv1, null);
        }
        if (GameMenuManager3.selectedLevel == 2)
        {
            currentLevel = Instantiate(lv2, null);
        }
        if (GameMenuManager3.selectedLevel == 3)
        {
            currentLevel = Instantiate(lv3, null);
        }
        
        var playerSpawner = currentLevel.GetComponent<PlayerSpawner>();
        spanwnPlayer1Anchor = playerSpawner.spawnerPlayer1;
        spanwnPlayer2Anchor = playerSpawner.spawnerPlayer2;
        spanwnPlayer3Anchor = playerSpawner.spawnerPlayer3;
        spanwnPlayer4Anchor = playerSpawner.spawnerPlayer4;
        

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

        if (index == 1)
		{
            MovementHandler player1, player2;

            if (playerPrefabTeam11.GetComponent<PlayerMovementHandler>() != null)
                player1 = InitPlayer(playerPrefabTeam11, this.spanwnPlayer1Anchor, team.FirstPlayerIndex, Tags.Player1, 1);
            else
                player1 = InitAIPlayer(playerPrefabTeam11, this.spanwnPlayer1Anchor, team.FirstPlayerIndex, Tags.Player1, 1);

            if (playerPrefabTeam12.GetComponent<PlayerMovementHandler>() != null)
                player2 = InitPlayer(playerPrefabTeam12, this.spanwnPlayer2Anchor, team.SecondPlayerIndex, Tags.Player2, 1);
            else
                player2 = InitAIPlayer(playerPrefabTeam12, this.spanwnPlayer2Anchor, team.SecondPlayerIndex, Tags.Player2, 1);

			player1.FriendTransform = player2.gameObject.transform;
			player2.FriendTransform = player1.gameObject.transform;

			this.players.Add(player1.gameObject);
			this.players.Add(player2.gameObject);
            this.ProjectionReference1 = player1.gameObject;
            this.ProjectionReference2 = player2.gameObject;
        }

		if(index == 2)
		{
            MovementHandler player3, player4;

            if (playerPrefabTeam21.GetComponent<PlayerMovementHandler>() != null)
                player3 = InitPlayer(playerPrefabTeam21, this.spanwnPlayer3Anchor, team.FirstPlayerIndex, Tags.Player3, 2);
            else
                player3 = InitAIPlayer(playerPrefabTeam21, this.spanwnPlayer3Anchor, team.FirstPlayerIndex, Tags.Player3, 2);

            if (playerPrefabTeam21.GetComponent<PlayerMovementHandler>() != null)
                player4 = InitPlayer(playerPrefabTeam22, this.spanwnPlayer4Anchor, team.SecondPlayerIndex, Tags.Player4, 2);
            else
                player4 = InitAIPlayer(playerPrefabTeam22, this.spanwnPlayer4Anchor, team.SecondPlayerIndex, Tags.Player4, 2);

			player3.FriendTransform = player4.gameObject.transform;
			player4.FriendTransform = player3.gameObject.transform;

			this.players.Add(player3.gameObject);
			this.players.Add(player4.gameObject);
            this.ProjectionReference3 = player3.gameObject;
            this.ProjectionReference4 = player4.gameObject;
        }
        
	}

	private PlayerMovementHandler InitPlayer(GameObject prefab, Transform anchor, PlayerIndex index, string tag,int team)
	{
		GameObject player = Instantiate(prefab, null);
		player.transform.position = anchor.position;

		var playerHandler = player.GetComponent<PlayerMovementHandler>();

        if (playerHandler != null)
        {
            playerHandler.Index = index;
            playerHandler.myteam = team;
            player.tag = tag;
            playerHandler.MainPosition = player.transform.position;
        }
        else Debug.LogError("No Player Handler Found");

		SmoothFollow.Get.Targets.Add(player.transform);

		return playerHandler;
	}

    private AIMovementHandler InitAIPlayer(GameObject prefab, Transform anchor, PlayerIndex index, string tag, int team)
    {
        GameObject player = Instantiate(prefab, null);
        player.transform.position = anchor.position;

        var AIHandler = player.GetComponent<AIMovementHandler>();

        if (AIHandler != null)
        {
            AIHandler.Index = index;
            AIHandler.myteam = team;
            player.tag = tag;
            AIHandler.MainPosition = player.transform.position;
        }
        else Debug.LogError("No Player Handler Found");

        SmoothFollow.Get.Targets.Add(player.transform);

        return AIHandler;
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
