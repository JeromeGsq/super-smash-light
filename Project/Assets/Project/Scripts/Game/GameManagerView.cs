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
    public GameObject BallProjectionReference;

    [SerializeField]
    private int debugLevel = 1;

    [Space(20)]
    [SerializeField]
    public GameManager gamemanager;

	[Space(20)]
    [SerializeField]
    private GameObject countdownPrefab;


    [Space(10)]
    [SerializeField]
	private GameObject ballPrefab;

	[Space(10)]

	[SerializeField]
	private GameObject playerPrefab;

    /*
    [SerializeField]
    private GameObject playerPrefabTeam11;

    [SerializeField]
    private GameObject playerPrefabTeam12;

    [SerializeField]
    private GameObject playerPrefabTeam21;

    [SerializeField]
    private GameObject playerPrefabTeam22;
    */

    [SerializeField]
    private GameObject prefabModelT1;

    [SerializeField]
    private GameObject prefabModelT2;

    [SerializeField]
    private GameObject prefabModelIAT1;

    [SerializeField]
    private GameObject prefabModelIAT2;
    
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

    private MovementHandler player1, player2, player3, player4;



    public override void Awake()
	{
		base.Awake();
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

            InitTeam();

            Destroy(countdown);

            FindObjectOfType<GridMaker>().BuildJPSData();
        }, 4));
	}
    /*
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
    */
	
	private void InitBall()
	{
		var ball = Instantiate(this.ballPrefab);
		ball.transform.position = this.spawnBallAnchor.position;
        BallProjectionReference = ball;

		SmoothFollow.Get.Targets.Add(ball.transform);
	}

    private void InitTeam()
    {
        TData savedData = FindObjectOfType<TData>();
        if (savedData != null)
        {
            switch (savedData.p1)
            {
                case 0:
                    Debug.Log("Clean Up saved team 0");
                    break;
                case 1:
                    player1 = InitPlayer(prefabModelT1, this.spanwnPlayer1Anchor, ViewModel.team1.NextIndex(), Tags.Player1, ViewModel.team1);
                    ViewModel.team1.AddPlayer(player1);
                    break;
                case 2:
                    player1 = InitPlayer(prefabModelT2, this.spanwnPlayer1Anchor, ViewModel.team2.NextIndex(), Tags.Player1, ViewModel.team2);
                    ViewModel.team2.AddPlayer(player1);
                    break;
                default:
                    Debug.LogError("Bad Data");
                    break;
            }

            switch (savedData.p2)
            {
                case 0:
                    Debug.Log("Clean Up saved team 0");
                    break;
                case 1:
                    player2 = InitPlayer(prefabModelT1, this.spanwnPlayer2Anchor, ViewModel.team1.NextIndex(), Tags.Player2, ViewModel.team1);
                    ViewModel.team1.AddPlayer(player2);
                    break;
                case 2:
                    player2 = InitPlayer(prefabModelT2, this.spanwnPlayer2Anchor, ViewModel.team2.NextIndex(), Tags.Player2, ViewModel.team2);
                    ViewModel.team2.AddPlayer(player2);
                    break;
                case 3:
                    player2 = InitPlayer(prefabModelIAT1, this.spanwnPlayer2Anchor, ViewModel.team1.NextIndex(), Tags.Player2, ViewModel.team1);
                    ViewModel.team1.AddPlayer(player2);
                    break;
                case 4:
                    player2 = InitPlayer(prefabModelIAT2, this.spanwnPlayer2Anchor, ViewModel.team2.NextIndex(), Tags.Player2, ViewModel.team2);
                    ViewModel.team2.AddPlayer(player2);
                    break;
                default:
                    Debug.LogError("Bad Data");
                    break;
            }

            switch (savedData.p3)
            {
                case 0:
                    Debug.Log("Clean Up saved team 0");
                    break;
                case 1:
                    player3 = InitPlayer(prefabModelT1, this.spanwnPlayer3Anchor, ViewModel.team1.NextIndex(), Tags.Player3, ViewModel.team1);
                    ViewModel.team1.AddPlayer(player3);
                    break;
                case 2:
                    player3 = InitPlayer(prefabModelT2, this.spanwnPlayer3Anchor, ViewModel.team2.NextIndex(), Tags.Player3, ViewModel.team2);
                    ViewModel.team2.AddPlayer(player3);
                    break;
                case 3:
                    player3 = InitPlayer(prefabModelIAT1, this.spanwnPlayer3Anchor, ViewModel.team1.NextIndex(), Tags.Player3, ViewModel.team1);
                    ViewModel.team1.AddPlayer(player3);
                    break;
                case 4:
                    player3 = InitPlayer(prefabModelIAT2, this.spanwnPlayer3Anchor, ViewModel.team2.NextIndex(), Tags.Player3, ViewModel.team2);
                    ViewModel.team2.AddPlayer(player3);
                    break;
                default:
                    Debug.LogError("Bad Data");
                    break;
            }

            switch (savedData.p4)
            {
                case 0:
                    Debug.Log("Clean Up saved team 0");
                    break;
                case 1:
                    player4 = InitPlayer(prefabModelT1, this.spanwnPlayer4Anchor, ViewModel.team1.NextIndex(), Tags.Player4, ViewModel.team1);
                    ViewModel.team1.AddPlayer(player4);
                    break;
                case 2:
                    player4 = InitPlayer(prefabModelT2, this.spanwnPlayer4Anchor, ViewModel.team2.NextIndex(), Tags.Player4, ViewModel.team2);
                    ViewModel.team2.AddPlayer(player4);
                    break;
                case 3:
                    player4 = InitPlayer(prefabModelIAT1, this.spanwnPlayer4Anchor, ViewModel.team1.NextIndex(), Tags.Player4, ViewModel.team1);
                    ViewModel.team1.AddPlayer(player4);
                    break;
                case 4:
                    player4 = InitPlayer(prefabModelIAT2, this.spanwnPlayer4Anchor, ViewModel.team2.NextIndex(), Tags.Player4, ViewModel.team2);
                    ViewModel.team2.AddPlayer(player4);
                    break;
                default:
                    Debug.LogError("Bad Data");
                    break;
            }
        }

        player1.FriendTransform = player1.myteam.GetOther(player1).transform;
        player2.FriendTransform = player2.myteam.GetOther(player2).transform;
        player3.FriendTransform = player3.myteam.GetOther(player3).transform;
        player4.FriendTransform = player4.myteam.GetOther(player4).transform;

        this.ProjectionReference1 = player1.gameObject;
        this.ProjectionReference2 = player2.gameObject;
        this.ProjectionReference3 = player3.gameObject;
        this.ProjectionReference4 = player4.gameObject;
    }

	private MovementHandler InitPlayer(GameObject prefab, Transform anchor, PlayerIndex index, string tag,Team team)
	{
		GameObject player = Instantiate(prefab, null);
		player.transform.position = anchor.position;

		var playerHandler = player.GetComponent<MovementHandler>();

        if (playerHandler != null)
        {
            //Debug.LogWarning("player " + index);
            playerHandler.Index = index;
            playerHandler.myteam = team;
            player.tag = tag;
            playerHandler.MainPosition = player.transform.position;
        }
        else Debug.LogError("No Player Handler Found");

		SmoothFollow.Get.Targets.Add(player.transform);

		return playerHandler;
	}

    /*
    private AIMovementHandler InitAIPlayer(GameObject prefab, Transform anchor, PlayerIndex index, string tag, Team team)
    {
        GameObject player = Instantiate(prefab, null);
        player.transform.position = anchor.position;

        var AIHandler = player.GetComponent<AIMovementHandler>();

        if (AIHandler != null)
        {
            //Debug.LogWarning("ai " + index);
            AIHandler.Index = index;
            AIHandler.myteam = team;
            player.tag = tag;
            AIHandler.MainPosition = player.transform.position;
        }
        else Debug.LogError("No Player Handler Found");

        SmoothFollow.Get.Targets.Add(player.transform);

        return AIHandler;
    }
    */

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
