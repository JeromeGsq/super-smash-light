using GamepadInput;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ip_ControllerTestHandler : MonoBehaviour
{
	private static List<Color> ColorList = new List<Color> {
		// CLEAR
		new Color(0, 0, 0),
		// BLUE
		new Color(0.329f, 0.388f, 1f),
		// RED
		new Color(0.961f, 0.18f, 0.18f),
		// GREEN
		new Color(0.122f, 0.62f, 0.251f),
		// YELLOW
		new Color(1, 0.78f, 0.09f),
	};

	private static float StickMultiply = 20.0f;

	private Color mainColor;
	private ip_GamePad.Index controllerIndex = ip_GamePad.Index.Any;

	private GamepadState gamepadState;

	#region Inspector Values
	[Space(20)]

	[SerializeField]
	private Image ControllerPlaceholder;

	[Space(20)]

	[SerializeField]
	private TextMeshProUGUI UnderLabel;

	[Space(20)]

	[SerializeField]
	private Image AButton;
	[SerializeField]
	private Image BButton;
	[SerializeField]
	private Image XButton;
	[SerializeField]
	private Image YButton;
	[SerializeField]
	private Image RBButton;
	[SerializeField]
	private Image LBButton;
	[SerializeField]
	private Image RTrigger;
	[SerializeField]
	private Image LTrigger;
	[SerializeField]
	private Image StartButton;
	[SerializeField]
	private Image SelectButton;
	[SerializeField]
	private GameObject UpButton;
	[SerializeField]
	private GameObject RightButton;
	[SerializeField]
	private GameObject DownButton;
	[SerializeField]
	private GameObject LeftButton;

	[Space(20)]

	[SerializeField]
	private Transform LeftStick;
	private Vector3 leftStickBasePosition;
	[SerializeField]
	private Transform RightStick;
	private Vector3 rightStickBasePosition;

	#endregion

	public ip_GamePad.Index ControllerIndex
	{
		get
		{
			return this.controllerIndex;
		}
		set
		{
			this.controllerIndex = value;
			if(this.UnderLabel != null)
			{
				this.UnderLabel.text = "Controller : " + this.controllerIndex;
			}
			this.UpdateColor();
		}
	}

	private void Awake()
	{
		this.gamepadState = new GamepadState();
		this.rightStickBasePosition = this.RightStick.transform.localPosition;
		this.leftStickBasePosition = this.LeftStick.transform.localPosition;
	}

	private void UpdateColor()
	{
		switch(this.ControllerIndex)
		{
			case ip_GamePad.Index.Any:
				this.mainColor = ColorList[0];
				break;
			case ip_GamePad.Index.One:
				this.mainColor = ColorList[1];
				break;
			case ip_GamePad.Index.Two:
				this.mainColor = ColorList[2];
				break;
			case ip_GamePad.Index.Three:
				this.mainColor = ColorList[3];
				break;
			case ip_GamePad.Index.Four:
				this.mainColor = ColorList[4];
				break;
		}

		this.ControllerPlaceholder.color = this.mainColor;
	}

	private void Update()
	{
		this.UpdateInputs();
	}

	private void UpdateInputs()
	{
		ip_GamePad.GetState(ref this.gamepadState, ControllerIndex);

		this.AButton.color = this.gamepadState.A ? Color.gray : Color.white;
		this.BButton.color = this.gamepadState.B ? Color.gray : Color.white;
		this.XButton.color = this.gamepadState.X ? Color.gray : Color.white;
		this.YButton.color = this.gamepadState.Y ? Color.gray : Color.white;

		this.RBButton.color = this.gamepadState.RB ? Color.gray : Color.white;
		this.LBButton.color = this.gamepadState.LB ? Color.gray : Color.white;

		this.StartButton.color = this.gamepadState.Start ? Color.gray : Color.white;
		this.SelectButton.color = this.gamepadState.Back ? Color.gray : Color.white;

		this.RTrigger.color = this.gamepadState.RT == 1 ? Color.gray : Color.white;
		this.LTrigger.color = this.gamepadState.LT == 1 ? Color.gray : Color.white;

		this.UpButton.SetActive(this.gamepadState.UpPressed);
		this.RightButton.SetActive(this.gamepadState.RightPressed);
		this.DownButton.SetActive(this.gamepadState.DownPressed);
		this.LeftButton.SetActive(this.gamepadState.LeftPressed);

		this.LeftStick.transform.localPosition = new Vector2(this.leftStickBasePosition.x + this.gamepadState.LeftStickAxis.x * StickMultiply, this.leftStickBasePosition.y + this.gamepadState.LeftStickAxis.y * StickMultiply);
		this.RightStick.transform.localPosition = new Vector2(this.rightStickBasePosition.x + this.gamepadState.RightStickAxis.x * StickMultiply, this.rightStickBasePosition.y + this.gamepadState.RightStickAxis.y * StickMultiply);
	}
}
