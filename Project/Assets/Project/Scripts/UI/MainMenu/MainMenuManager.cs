
public class MainMenuManager : UIManager
{
	public ui_Button SelectedButton;

	private void OnEnable()
	{
	//	SelectedButton.IsFocused = true;
	}

	private void LateUpdate()
	{
		if(this.GamepadState.Start)
		{
			SimpleMenu.Next();
		}
	}
}