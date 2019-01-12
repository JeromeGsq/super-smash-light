
public class SplashscreenManager : UIManager
{
	private void LateUpdate()
	{
		if(this.GamepadState.Start)
		{
			SimpleMenu.Next();
		}
	}
}
