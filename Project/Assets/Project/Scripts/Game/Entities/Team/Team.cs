using UnityWeld.Binding;
using static GamepadInput.ip_GamePad;

[Binding]
public class Team
{
	public int TeamIndex
	{
		get;
		set;
	}

	[Binding]
	public int Score {
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
}
