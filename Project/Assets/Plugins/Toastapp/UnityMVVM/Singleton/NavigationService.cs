using Root.DesignPatterns;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NavigationService : SceneSingleton<NavigationService>
{
	private const string ViewPrefabsPath = "Prefabs/UI/Views";

	[SerializeField]
	private Transform RootView;

	[SerializeField]
	private List<GameObject> viewPrefabs;

	private List<Twin<GameObject, Type>> viewAndViewModelTypePrefabs;
	
	private List<Twin<GameObject, Type>> navigationStack = new List<Twin<GameObject, Type>>();

	protected void Awake()
	{
		this.CleanRootView();
		this.LoadPrefabsFromResources();
		this.ExtractViewModels();
	}

	private void CleanRootView()
	{
		foreach(Transform child in this.RootView.transform)
		{
			Destroy(child.gameObject);
		}
	}

	private void LoadPrefabsFromResources()
	{
		this.viewPrefabs = new List<GameObject>( Resources.LoadAll<GameObject>(ViewPrefabsPath));
	}

	private void ExtractViewModels()
	{
		this.viewAndViewModelTypePrefabs = new List<Twin<GameObject, Type>>();

		foreach(var viewGameObject in viewPrefabs)
		{
			var components = viewGameObject.GetComponents(typeof(Component));

			foreach(var component in components)
			{
				var interfaces = component.GetType().GetInterfaces();
				foreach(var inter in interfaces)
				{
					// If this gameobject contains a component of type IViewModel...
					if(inter.Equals(typeof(IViewModel)))
					{
						// ... add it to available Views
						var viewModelComponent = component as IViewModel;
						this.viewAndViewModelTypePrefabs.Add(Twin.New(viewGameObject, viewModelComponent.GetType()));
						break;
					}
				}
			}
		}
	}

	public void ShowViewModel(Type destinationViewModelType)
	{
		this.ShowViewModel<object>(destinationViewModelType, null);
	}

	public void ShowViewModel(Type destinationViewModelType, Transform root)
	{
		this.ShowViewModel<object>(destinationViewModelType, null, root);
	}

	public GameObject ShowViewModel<T>(Type destinationViewModelType, T parameters, Transform rootView = null)
	{
		// Use default root
		if(rootView == null)
		{
			rootView = this.RootView;
		}

		foreach(var viewAndViewModelTypeGameObject in this.viewAndViewModelTypePrefabs)
		{
			// If this is the correct destinationViewModelType...
			if(viewAndViewModelTypeGameObject.Type.Equals(destinationViewModelType))
			{
				// ... create View ...
				GameObject instantiatedView = Instantiate(viewAndViewModelTypeGameObject.GameObject, rootView);
				var components = instantiatedView.GetComponents(typeof(Component));

				// ... and initialize its ViewModel
				foreach(var component in components)
				{
					var interfaces = component.GetType().GetInterfaces();
					foreach(var inter in interfaces)
					{
						if(inter.Equals(typeof(IViewModel)))
						{
							var viewModel = component as IViewModel;
							viewModel.SetParameters(parameters);
							break;
						}
					}
				}

				this.navigationStack.Add(Twin.New(instantiatedView, viewAndViewModelTypeGameObject.Type));
				return instantiatedView;
			}
		}

		return null;
	}

	public void CloseViewModel(IViewModel viewModelInstance)
	{
		Twin<GameObject, Type> selectedInstance = null;

		foreach(var item in this.navigationStack)
		{
			if(viewModelInstance.GetType().Equals(item.Type))
			{
				selectedInstance = item;
			}
		}

		Destroy(selectedInstance.GameObject);
		this.navigationStack.Remove(selectedInstance);
	}
}