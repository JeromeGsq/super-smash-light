using Root.DesignPatterns;
using DG.Tweening;

public class CameraHandler : SceneSingleton<CameraHandler>
{
	public void Rumble(float amount = 1)
	{
		this.transform.DOShakePosition(0.1f, strength: amount);
	}
}
