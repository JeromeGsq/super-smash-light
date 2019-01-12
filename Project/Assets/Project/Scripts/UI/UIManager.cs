using GamepadInput;
using Root.Modules.UI;
using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
	protected ip_GamePad.Index IndexGamepad = ip_GamePad.Index.Any;
	protected GamepadState GamepadState;
	protected ui_SimpleMenu SimpleMenu;

	private void Awake()
	{
		this.GamepadState = new GamepadState();
		this.SimpleMenu = GetComponentInParent<ui_SimpleMenu>();
	}

	private void Update()
	{
		ip_GamePad.GetState(ref this.GamepadState, IndexGamepad);
	}
}
