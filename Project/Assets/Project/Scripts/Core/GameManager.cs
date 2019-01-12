using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public float timer;
	public GameObject players;
	public GameObject spawns;
	public GameObject spawnBall;
	public GameObject ball;

	void Start()
	{
		this.StartGame();
	}

	void Update()
	{
		this.timer += Time.deltaTime;
	}

	public void StartGame()
	{
		Debug.Log("-- Game start");
		int i = 0;

		// Get an instance of your 4 players in an array or list ( => list is more flexible)
		foreach(Transform child in this.players.transform)
		{
			if(i < 2)
			{
				child.gameObject.GetComponent<Player>().SetTeam("Red");
			}
			else
			{
				child.gameObject.GetComponent<Player>().SetTeam("Blue");
			}

			child.gameObject.GetComponent<Player>().PlayerId = i;
            if (i == 0)
            {
                child.gameObject.GetComponent<Player>().ControllerIndex = ip_GamePad.Index.One;
            }
            else if (i == 1)
            {
                child.gameObject.GetComponent<Player>().ControllerIndex = ip_GamePad.Index.Two;
            }
            else if (i == 2)
            {
                child.gameObject.GetComponent<Player>().ControllerIndex = ip_GamePad.Index.Three;
            }
            else if (i == 3)
            {
                child.gameObject.GetComponent<Player>().ControllerIndex = ip_GamePad.Index.Four;
            }

            // Do it hardcoded for 4 players 
            int a;
			a = i % 2;

			if(a == 0)
			{
				a = 1 + i;
			}
			else
			{
				a = -1 + i;
			}

			// Never do GetChild() please
			child.gameObject.GetComponent<Player>().PlayerFriend = players.transform.GetChild(a).gameObject;

			i++;
		}

		this.ResetPosition();
	}

	public void ResetPosition()
	{
		Debug.Log("Reset players transform positions");

		int i = 0;
		foreach(Transform child in players.transform)
		{
			// Never do GetChild() please
			child.position = spawns.transform.GetChild(i).transform.position;
			i++;
		}

		this.ball.transform.position = this.spawnBall.transform.position;
	}
}
