using UnityEngine;
using UnityWeld.Binding;
using XInputDotNetPure;

public class Team
{
	public int Score {
		get; set;
	}

	public float BarLevel
	{
		get; set;
	}

	public PlayerIndex FirstPlayerIndex {
		get;
		set;
	}

	public PlayerIndex SecondPlayerIndex
	{
		get;
		set;
	}

	public static int GetTeam(PlayerIndex index)
	{
		var teamIndex = (int)index % 2;

		if(teamIndex == 1)
		{
			return 1;
		}
		else
		{
			return 2;
		}
	}
}