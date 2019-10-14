using UnityEngine;
using UnityWeld.Binding;
using XInputDotNetPure;

public class Team
{
    public int number;
    private int indexCallCount = 0;
    public MovementHandler[] players = new MovementHandler[2];

    public Team(int n)
    {
        number = n;
    }

    public int Score {
		get; set;
	}

	public float BarLevel
	{
		get; set;
	}

	public PlayerIndex FirstPlayerIndex {
		private get;
		set;
	}

	public PlayerIndex SecondPlayerIndex
	{
		private get;
		set;
	}

    public PlayerIndex NextIndex()
    {
        switch(indexCallCount)
        {
            case 0:
                indexCallCount++;
                return FirstPlayerIndex;
            case 1:
                indexCallCount++;
                return SecondPlayerIndex;
            default:
                Debug.LogError("Too many calls for team indexes");
                return PlayerIndex.One;
        }
    }

    public void AddPlayer(MovementHandler m)
    {
        if(players[0] == null)
        {
            players[0] = m;
        }
        else
        {
            players[1] = m;
        }
    }

    public MovementHandler GetOther(MovementHandler self)
    {
        if (players[0] = self) return players[1];
        else if (players[1] = self) return players[0];
        else
        {
            Debug.LogError("Teammate call to wrong team");
            return null;
        }
    }

    /*
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
    */
}