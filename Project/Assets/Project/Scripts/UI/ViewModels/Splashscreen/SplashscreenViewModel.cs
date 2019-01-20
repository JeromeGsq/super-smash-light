using UnityWeld.Binding;

[Binding]
public class SplashscreenViewModel : BaseViewModel
{
	public void Start()
	{
		StartCoroutine(CoroutineUtils.DelaySeconds(() =>
		{
			//NavigationService.Instance.ShowViewModel(typeof(MainTabbarViewModel));
		}, 1));
	}
}
