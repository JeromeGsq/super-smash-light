using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine.UI;
using Root.Modules.UI;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

[InitializeOnLoad]
public static class Opti
{
	// Chargement de la liste des Gameobjects présents dans la hiérarchie
	static List<int> markedObjects;

	static Texture2D cubeGizmo;

	// Constructeur
	static Opti()
	{
		cubeGizmo = AssetDatabase.LoadAssetAtPath("Assets/Project/Scripts/Editor/Gizmo/cube.png", typeof(Texture2D)) as Texture2D;
		// EditorApplication.hierarchyWindowItemOnGUI += DrawLine;
		// EditorApplication.hierarchyWindowItemOnGUI += DestroyHelpers;

		EditorApplication.hierarchyWindowItemOnGUI += ShowColliderLogo;
		EditorApplication.hierarchyWindowItemOnGUI += ShowMeshLogo;
		EditorApplication.hierarchyWindowItemOnGUI += ShowAnimatorLogo;
		EditorApplication.hierarchyWindowItemOnGUI += ShowLightLogo;

		EditorApplication.hierarchyWindowItemOnGUI += CleanButtonChildText;

		Selection.selectionChanged += () =>
		{
			MenuAutoShow();
		};
		//EditorApplication.hierarchyWindowItemOnGUI += CleanTransforms;
	}

	#region Collider logo
	static void ShowColliderLogo(int instanceID, Rect selectionRect)
	{
		Rect logoRect = new Rect(selectionRect);
		logoRect.width = 8;
		logoRect.x -= 20;
		logoRect.y += 4;

		Rect textRect = new Rect(selectionRect);
		textRect.y += 1;

		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if(go != null && go.GetComponent<Collider>())
		{

			GUI.color = new Color(0, 1, 0);
			GUI.Label(logoRect, cubeGizmo);
			GUI.color = new Color(1, 1, 1);
		}
	}
	#endregion

	#region Mesh logo
	static void ShowMeshLogo(int instanceID, Rect selectionRect)
	{
		Rect logoRect = new Rect(selectionRect);
		logoRect.width = 8;
		logoRect.x -= 16;
		logoRect.y += 4;

		Rect textRect = new Rect(selectionRect);
		textRect.y += 1;

		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if(go != null && go.GetComponent<MeshRenderer>())
		{

			GUI.color = new Color(1, 1, 1);
			GUI.Label(logoRect, cubeGizmo);
			GUI.color = new Color(1, 1, 1);
		}
	}
	#endregion

	#region Animator logo
	static void ShowAnimatorLogo(int instanceID, Rect selectionRect)
	{
		Rect logoRect = new Rect(selectionRect);
		logoRect.width = 8;
		logoRect.x -= 16;
		logoRect.y += 8;

		Rect textRect = new Rect(selectionRect);
		textRect.y += 1;

		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if(go != null && (go.GetComponent<Animator>() || go.GetComponent<Animation>()))
		{

			GUI.color = new Color(1, 0, 0);
			GUI.Label(logoRect, cubeGizmo);
			GUI.color = new Color(1, 1, 1);
		}
	}
	#endregion

	#region Light logo
	static void ShowLightLogo(int instanceID, Rect selectionRect)
	{
		Rect logoRect = new Rect(selectionRect);
		logoRect.width = 8;
		logoRect.x -= 20;
		logoRect.y += 8;

		Rect textRect = new Rect(selectionRect);
		textRect.y += 1;

		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if(go != null && go.GetComponent<Light>())
		{

			GUI.color = new Color(1, 1, 0);
			GUI.Label(logoRect, cubeGizmo);
			GUI.color = new Color(1, 1, 1);
		}
	}
	#endregion

	#region Group Into Father
	[MenuItem("GameObject/Opti/Group Into Father #%g")]
	static void AddAsChild()
	{
		Transform[] tTab = Selection.transforms;
		List<Transform> tList = new List<Transform>();

		foreach(Transform t in tTab)
		{
			tList.Add(t);
		}

		tList.Sort((IComparer<Transform>)new SortingByName());

		if(tList.Count > 0)
		{
			GameObject go = new GameObject("__Parent__");
			go.transform.SetParent(tList[0].parent);
			go.transform.SetAsFirstSibling();
			for(int i = 0; i < tList.Count; i++)
			{
				tList[i].SetParent(go.transform);
			}
		}
		Selection.activeGameObject = null;

		Debug.Log("Group Into Father : Ok");
	}
	#endregion

	#region Separation Line
	//Créé une ligne de séparation dans la hiérachie
	[MenuItem("GameObject/Opti/Separation Line", false, 0)]
	static void Init()
	{
		GameObject go = new GameObject("___");
		if(Selection.activeTransform != null)
		{
			go.transform.parent = Selection.activeTransform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
		}
	}

	static void DrawLine(int instanceID, Rect selectionRect)
	{
		Rect r = new Rect(selectionRect);
		r.x = r.x - 30;
		r.y = r.y + 1;

		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if(go != null && go.name.Contains("___") && EditorGUIUtility.currentViewWidth > 0)
		{

			if(go.name.Contains("("))
			{
				go.name = "___";
			}
			GUI.color = new Color(1, 1, 1);
			string size = "";
			for(int i = 0; i < EditorGUIUtility.currentViewWidth; i++)
			{
				if(i % 6 == 0)
				{
					size += "_";
				}
			}
			GUI.Label(r, size);
		}
	}
	#endregion

	#region Separation Empty
	//Créé une ligne de séparation dans la hiérachie
	[MenuItem("GameObject/Opti/Separation Empty", false, 0)]
	static void InitEmpty()
	{
		GameObject go = new GameObject("");
		if(Selection.activeTransform != null)
		{
			go.transform.parent = Selection.activeTransform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
		}
	}
	#endregion

	#region Destroy Helpers
	static void DestroyHelpers(int instanceID, Rect selectionRect)
	{
		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if(go != null && (go.name.Contains("___") || go.name.Equals("")))
		{
			Object.DestroyImmediate(go);
		}
	}
	#endregion

	#region Clean Button
	static void MenuAutoShow()
	{
		if(EditorApplication.isPlaying)
		{
			return;
		}

		GameObject go = Selection.activeGameObject;

		if(go != null && go.name.Equals("UI_Main"))
		{
			return;
		}

		if(go != null && go.GetComponent<ui_SimpleWindow>())
		{
			var goMenus = GameObject.FindObjectsOfType<ui_SimpleWindow>();

			for(int i = 0; i < goMenus.Length; i++)
			{
				if(goMenus[i].name.Equals("UI_Main"))
				{
					continue;
				}

				goMenus[i].gameObject.SetActive(false);
			}

			go.gameObject.SetActive(true);
		}
		else
		{
			Transform goParent = null;
			if(go != null && go.transform != null && go.transform.parent != null)
			{
				goParent = go.transform.parent;

				while(goParent != null && goParent.GetComponentInParent<ui_SimpleWindow>() == null)
				{
					if(goParent.name.Equals("UI_Main"))
					{
						goParent = goParent.parent;
					}
					else if(goParent.GetComponent<ui_SimpleWindow>())
					{
						break;
					}
					else
					{
						goParent = goParent.parent;
					}
				}

				var goMenus = GameObject.FindObjectsOfType<ui_SimpleWindow>();

				for(int i = 0; i < goMenus.Length; i++)
				{
					if(goMenus[i].name.Equals("UI_Main"))
					{
						continue;
					}

					goMenus[i].gameObject.SetActive(false);
				}

				if(goParent != null)
				{
					goParent.gameObject.SetActive(true);
				}
			}
		}
	}
	#endregion

	#region Clean Button
	static void CleanButtonChildText(int instanceID, Rect selectionRect)
	{
		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if(go != null && go.GetComponent<Button>())
		{
			var childText = go.GetComponentInChildren<Text>();
			if(childText)
			{
				var child = childText.gameObject;
				string value = childText.text;

				Object.DestroyImmediate(child.GetComponent<Text>());

				var TextMeshProUGUI = child.AddComponent<TextMeshProUGUI>();
				TextMeshProUGUI.text = value;
				TextMeshProUGUI.rectTransform.anchorMin = new Vector2(0, 0);
				TextMeshProUGUI.rectTransform.anchorMax = new Vector2(1, 1);
				TextMeshProUGUI.rectTransform.pivot = new Vector2(0.5f, 0.5f);
				TextMeshProUGUI.rectTransform.sizeDelta = new Vector2(0, 0);
				TextMeshProUGUI.alignment = TextAlignmentOptions.Center;
				TextMeshProUGUI.enableAutoSizing = true;
			}

			if(go != null && go.GetComponentInChildren<TextMeshProUGUI>() && go.GetComponent<Button>())
			{
				var childTextMeshProUGUI = go.GetComponentInChildren<TextMeshProUGUI>();
				if(!childTextMeshProUGUI.name.Equals(go.name.Replace("Button", "Text")))
				{
					childTextMeshProUGUI.name = go.name.Replace("Button", "Text");
				}
			}
		}
	}
	#endregion

	#region Clean Transforms
	// Clean les transforms des gameobjects de tri
	static void CleanTransforms(int instanceID, Rect selectionRect)
	{
		GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
		if(go != null && (go.name.Equals("") || go.name.Equals("___")))
		{
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
		}
	}
	#endregion

	#region JumpTo
	[MenuItem("Opti/Jump to _work %w")]
	static void JumpToWork()
	{
		if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
		{
			Debug.Log("Warning, the previous scene has been saved");
		}
		else
		{
			Debug.Log("Warning, the previous scene hasn't been saved");
		}
		EditorSceneManager.OpenScene("./Assets/Project/Scenes/_work.unity");
		Debug.Log("Load _work");
	}

	[MenuItem("Opti/Jump to Main %h")]
	static void JumpToHome()
	{
		if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
		{
			Debug.Log("Warning, the previous scene has been saved");
		}
		else
		{
			Debug.Log("Warning, the previous scene hasn't been saved");
		}
		EditorSceneManager.OpenScene("./Assets/Project/Scenes/Main.unity");
		Debug.Log("Load Main");
	}
	#endregion

	#region Open Explorer 
	[MenuItem("GameObject/Opti/Show In Explorer %e")]
	static void OpenFolderExplorer()
	{
		EditorUtility.RevealInFinder(UnityUtil.GetSelectedPathOrFallback());
	}
	#endregion

	#region Compile and Play
	[MenuItem("Opti/Compile and Play %P")]
	static void Play()
	{
		AssetDatabase.Refresh();
		EditorApplication.isPlaying = true;
	}
	#endregion
}

class SortingByName : IComparer<Transform>
{
	int IComparer<Transform>.Compare(Transform _objA, Transform _objB)
	{
		string a = _objA.name;
		string b = _objB.name;
		return a.CompareTo(b);
	}
}

public static class UnityUtil
{
	public static string GetSelectedPathOrFallback()
	{
		string path = "Assets";

		foreach(UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
		{
			path = AssetDatabase.GetAssetPath(obj);
			if(!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				path = Path.GetDirectoryName(path);
				break;
			}
		}
		return path;
	}
}