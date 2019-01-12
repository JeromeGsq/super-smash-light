using GamepadInput;
using Root.DesignPatterns;
using UnityEngine;

public class ip_Debugger : GlobalSingleton<ip_Debugger>
{

	private GamepadState gamepadState;
	private bool isDown;

	[SerializeField]
	private GameObject panel;

	[Space(20)]

	[SerializeField]
	private ip_ControllerTestHandler controllerTestHandlerOne;
	[SerializeField]
	private ip_ControllerTestHandler controllerTestHandlerTwo;
	[SerializeField]
	private ip_ControllerTestHandler controllerTestHandlerThree;
	[SerializeField]
	private ip_ControllerTestHandler controllerTestHandlerFour;

	private void Awake() 
	{
		this.gamepadState = new GamepadState();

		if(this.controllerTestHandlerOne != null)
		{
			this.controllerTestHandlerOne.ControllerIndex = GamepadInput.ip_GamePad.Index.One;
		}

		if(this.controllerTestHandlerTwo != null) 
		{
			this.controllerTestHandlerTwo.ControllerIndex = GamepadInput.ip_GamePad.Index.Two;
		}

		if(this.controllerTestHandlerThree != null)
		{
			this.controllerTestHandlerThree.ControllerIndex = GamepadInput.ip_GamePad.Index.Three;
		}

		if(this.controllerTestHandlerFour != null)
		{
			this.controllerTestHandlerFour.ControllerIndex = GamepadInput.ip_GamePad.Index.Four;
		}
	}

	private void Update()
	{
		ip_GamePad.GetState(ref this.gamepadState, ip_GamePad.Index.Any);

		// Display popup
		if(this.gamepadState.Start == true && this.gamepadState.Back == true && this.isDown == false)
		{
			this.isDown = true;
			this.panel.SetActive(!this.panel.activeSelf);
		}
		else if(this.gamepadState.Start == false && this.gamepadState.Back == false && this.isDown == true)
		{
			this.isDown = false;
		}

		if(Input.GetKeyDown(KeyCode.Tab))
		{
			this.panel.SetActive(!this.panel.activeSelf);
		}

		// Switch player keyboard

		if(Input.GetKey(KeyCode.F1))
		{
			ip_GamePad.KeyboardIndex = ip_GamePad.Index.One;
		}
		else if(Input.GetKey(KeyCode.F2))
		{
			ip_GamePad.KeyboardIndex = ip_GamePad.Index.Two;
		}
		else if(Input.GetKey(KeyCode.F3))
		{
			ip_GamePad.KeyboardIndex = ip_GamePad.Index.Three;
		}
		else if(Input.GetKey(KeyCode.F4))
		{
			ip_GamePad.KeyboardIndex = ip_GamePad.Index.Four;
		}
		else if(Input.GetKey(KeyCode.F5))
		{
			ip_GamePad.KeyboardIndex = ip_GamePad.Index.Any;
		}
	}
}
