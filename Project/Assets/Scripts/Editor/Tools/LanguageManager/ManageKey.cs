using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The window for editing a key.
/// </summary>
public class ManageKey : EditorWindow
{
    // Attributes
    static private string key = "default";
    static private Content content;

    private string newKey = "";
    private Vector2 scrollBar;

    /// <summary>
    /// Initialize the window usefull to editing a key.
    /// </summary>
    public static void Init(string keyElement)
    {
        // Assign the key
        key = keyElement;
        content = LanguageManager.KeyList.JSONDictionary[key];
        ManageKey window = CreateInstance<ManageKey>();
        window.ShowUtility();
        const int w = 1000;
        const int h = 500;
        // Center the window.
        window.position = new Rect(Screen.currentResolution.width / 2f - w / 2,
                                   Screen.currentResolution.height / 2f - h / 2,
                                   w, h);
    }

    /// <summary>
    /// The window for editing a key
    /// </summary>
    private void OnGUI()
    {
        GUILayout.BeginVertical();

        DisplayKeyName();

        UpdateKeyName();

        DisplayLanguages();

        GUILayout.EndVertical();
    }

    /// <summary>
    /// Display the Key Name.
    /// </summary>
    private void DisplayKeyName()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.Label(key, (GUIStyle)"sv_label_2");

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Display a line to change a key name.
    /// </summary>
    private void UpdateKeyName()
    {
        GUILayout.BeginHorizontal();

        newKey = EditorGUILayout.TextField("New name for key : ", newKey);

        if (GUILayout.Button("Update Key"))
        {
            if (LanguageManager.KeyList.UpdateKey(key, newKey))
            {
                EditorUtility.DisplayDialog("Update", "You have change the key " + key + " by " + newKey, "Cancel");
                key = newKey;
                LanguageManager.Instance.Save();
            }
            else
            {
                EditorUtility.DisplayDialog("Already exist!", "You have tried to had an existing key", "Cancel");
            }
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Display all languages supported and their field to update contents.
    /// </summary>
    private void DisplayLanguages()
    {
        // Header
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.Label("Languages");

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);

        // We iterate on each languages associated to a key.
        scrollBar = GUILayout.BeginScrollView(scrollBar);

        // Sort languages
        LanguageManager.LanguageList.Sort();

        foreach (string language in LanguageManager.LanguageList)
        {
            GUILayout.BeginHorizontal();
            // Use element to pass the problem of enumerator.
            string element = content.JSONDictionary[language];
            element = EditorGUILayout.TextField(language, element);
            if (!element.Equals(content.JSONDictionary[language]))
            {
                content.JSONDictionary[language] = element;
                LanguageManager.Instance.Save();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }
}
