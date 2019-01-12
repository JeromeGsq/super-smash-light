using GamepadInput;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ui_Button : Button
{
	private GamepadState gamepadState;

	public ip_GamePad.Index IndexGamepad = ip_GamePad.Index.One;

	[Space(20)]

	public bool isFocused;
	public bool IsFocused
	{
		get
		{
			return isFocused;
		}
		set
		{
			this.isFocused = value;
			this.UpdateFocusStatus();
		}
	}

	[Space(20)]

	public ui_Button NextUpButton;
	public ui_Button NextRightButton;
	public ui_Button NextDownButton;
	public ui_Button NextLeftButton;

	[Space(20)]

	public UnityEvent AButton;
	public UnityEvent BButton;
	public UnityEvent XButton;
	public UnityEvent YButton;

	[Space(20)]

	public UnityEvent Up;
	public UnityEvent Right;
	public UnityEvent Down;
	public UnityEvent Left;

	[Space(20)]

	public UnityEvent RBButton;
	public UnityEvent LBButton;

	[Space(20)]

	public UnityEvent StartButton;
	public UnityEvent SelectButton;

	protected override void Awake()
	{
		base.Awake();
		this.gamepadState = new GamepadState();
	}

	protected override void Start()
	{
		base.Start();
		this.UpdateFocusStatus();
	}

	private void Update()
	{
		ip_GamePad.GetState(ref this.gamepadState, IndexGamepad);

		if(this.IsFocused == false)
		{
			return;
		}

		if(this.gamepadState == null)
		{
			return;
		}

		if(this.gamepadState.A)
		{
			if(this.AButton != null)
			{
				this.AButton.Invoke();
			}
		}
		else if(this.gamepadState.B)
		{
			if(this.BButton != null)
			{
				this.BButton.Invoke();
			}
		}
		else if(this.gamepadState.X)
		{
			if(this.XButton != null)
			{
				this.XButton.Invoke();
			}
		}
		else if(this.gamepadState.Y)
		{
			if(this.YButton != null)
			{
				this.YButton.Invoke();
			}
		}
		else if(this.gamepadState.Start)
		{
			if(this.StartButton != null)
			{
				this.StartButton.Invoke();
			}
		}
		else if(this.gamepadState.Back)
		{
			if(this.SelectButton != null)
			{
				this.SelectButton.Invoke();
			}
		}

		if(this.gamepadState.UpPressed || this.gamepadState.LStickUpPressed)
		{
			if(this.Up != null)
			{
				this.Up.Invoke();
				this.SetNextButton(this.NextUpButton, this.gamepadState);
			return;
			}
			return;
		}
		else if(this.gamepadState.DownPressed || this.gamepadState.LStickDownPressed)
		{
			if(this.Down != null)
			{
				this.Down.Invoke();
				this.SetNextButton(this.NextDownButton, this.gamepadState);
			return;
			}
			return;
		}
		else if(this.gamepadState.LeftPressed || this.gamepadState.LStickLeftPressed)
		{
			if(this.Left != null)
			{
				this.Left.Invoke();
				this.SetNextButton(this.NextLeftButton, this.gamepadState);
				return;
			}
			return;
		}
		else if(this.gamepadState.RightPressed || this.gamepadState.LStickRightPressed)
		{
			if(this.Right != null)
			{
				this.Right.Invoke();
				this.SetNextButton(this.NextRightButton, this.gamepadState);
				return;
			}
			return;
		}
	}

	private void SetNextButton(ui_Button button, GamepadState gamepadState)
	{
		if(button != null)
		{
			button.gamepadState = gamepadState;
			button.IsFocused = true;
			this.IsFocused = false;
		}
	}

	private void UpdateFocusStatus()
	{
		if(this.IsFocused)
		{
			this.transform.localScale = Vector3.one * 0.9f;
		}
		else
		{
			this.transform.localScale = Vector3.one * 1f;
		}
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(ui_Button), true)]
[CanEditMultipleObjects]
public class ui_ButtonEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if(GUILayout.Button("Refresh"))
		{
			ui_Button menu = ((ui_Button)target);
		}

		base.OnInspectorGUI();
	}
}
#endif