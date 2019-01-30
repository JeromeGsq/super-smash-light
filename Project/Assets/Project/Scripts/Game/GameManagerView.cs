using System;
using System.Collections.Generic;
using System.ComponentModel;
using GamepadInput;
using UnityEngine;
using UnityEngine.UI;
using static GamepadInput.ip_GamePad;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GameManager))]
public class GameManagerView : BaseView<GameManager>
{
	[Space(20)]

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

		this.InitTeam(this.ViewModel?.Team1, 1);
		this.InitTeam(this.ViewModel?.Team2, 2);
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
			this.UpdateBallColor(this.ViewModel.BallIndex);
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
			var player1 = InitPlayer(this.spanwnPlayer1Anchor, team.FirstPlayerIndex, Tags.Player1);
			var player3 = InitPlayer(this.spanwnPlayer3Anchor, team.SecondPlayerIndex, Tags.Player3);

			player1.FriendTransform = player3.gameObject.transform;
			player3.FriendTransform = player1.gameObject.transform;

			this.players.Add(player1.gameObject);
			this.players.Add(player3.gameObject);
		}

		if(index == 2)
		{
			var player2 = InitPlayer(this.spanwnPlayer2Anchor, team.FirstPlayerIndex, Tags.Player2);
			var player4 = InitPlayer(this.spanwnPlayer4Anchor, team.SecondPlayerIndex, Tags.Player4);

			player2.FriendTransform = player4.gameObject.transform;
			player4.FriendTransform = player2.gameObject.transform;

			this.players.Add(player2.gameObject);
			this.players.Add(player4.gameObject);
		}
	}

	private PlayerMovementHandler InitPlayer(Transform anchor, Index index, string tag)
	{
		GameObject player = Instantiate(this.playerPrefab, null);
		player.transform.position = anchor.position;

        player.name = $"Player - {index}";

		var playerHandler = player.GetComponent<PlayerMovementHandler>();
		playerHandler.Index = index;
		player.tag = tag;

		playerHandler.MainPosition = player.transform.position;

		SmoothFollow.Get.Targets.Add(player.transform);

		return playerHandler;
	}

	private void UpdateBallColor(Index ballIndex)
	{
		var teamIndex = Team.GetTeam(ballIndex);
		var color = Color.white;

		if(ballIndex != Index.Any)
		{
			color = teamIndex == 1 ? Color.blue : Color.red;
		}

		this.ballImage.color = color;
	}
}
