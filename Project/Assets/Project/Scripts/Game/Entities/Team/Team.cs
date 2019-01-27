using UnityEngine;
using UnityWeld.Binding;
using static GamepadInput.ip_GamePad;

public class Team
{
	public int Score {
		get; set;
	}

	public float BarLevel
	{
		get; set;
	}

	public Index FirstPlayerIndex {
		get;
		set;
	}

	public Index SecondPlayerIndex
	{
		get;
		set;
	}

	public static int GetTeam(Index index)
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