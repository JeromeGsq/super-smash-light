﻿using UnityWeld.Binding;

[Binding]
public class BaseViewModel : UnityViewModel
{
	[Binding]
	public void CloseViewModel()
	{
		NavigationService.Get.CloseViewModel(this);
	}
}