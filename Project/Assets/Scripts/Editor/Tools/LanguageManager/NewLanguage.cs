using UnityEditor;
using UnityEngine;

/// <summary>
/// Window to update languages.
/// When a language is added, it is automaticaly saved in json file.
/// When it is removed, it is not save in the json file.
/// We need to click on save to do it.
/// </summary>
public class NewLanguage : EditorWindow
{
    // Attributes
    private Vector2 scrollBar;
    private string language = "";

    /// <summary>
    /// Initialize the window usefull to create a new key.
    /// </summary>
    public static void Init()
    {
        NewLanguage window = CreateInstance<NewLanguage>();
        window.ShowUtility();
        const int w = 512;
        const int h = 128;
        // Center the window.
        window.position = new Rect(Screen.currentResolution.width / 2f - w / 2,
                                   Screen.currentResolution.height / 2f - h / 2,
                                   w, h);
    }

    /// <summary>
    /// The window for creating a key
    /// </summary>
    private void OnGUI()
    {
        // The text field
        language = EditorGUILayout.TextField("New language : ", language);

        // Display the button only if the text field is not empty.
        if (language != null && language.Length > 0 && GUILayout.Button("Create"))
        {
            if (!AddLanguage(language))
            {
                EditorUtility.DisplayDialog("Already exist!", "You have tried to had an existing language", "Cancel");
            }
            else
            {
                LanguageManager.Instance.Save();
            }
        }

        DisplayLanguages();
    }

    /// <summary>
    /// Add a new language for all key in document.
    /// </summary>
    /// <param name="newLanguage">The new language to add.</param>
    /// <returns>true if the language is new and inserted, false otherwise.</returns>
    private bool AddLanguage(string newLanguage)
    {
        if (LanguageManager.LanguageList.Contains(newLanguage))
        {
            return false;
        }
        // Add Languages on all key.
        foreach (string key in LanguageManager.KeyList.Keys)
        {
            LanguageManager.KeyList.JSONDictionary[key].AddKey(newLanguage);
        }

        LanguageManager.LanguageList.Add(newLanguage);
        LanguageManager.LanguageList.Sort();
        return true;
    }

    /// <summary>
    /// Display all languages supported by the Language Manager
    /// </summary>
    private void DisplayLanguages()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Supported Languages :");
        scrollBar = GUILayout.BeginScrollView(scrollBar);

        // Sort the langage list.
        LanguageManager.LanguageList.Sort();

        // Display them
        foreach (string l in LanguageManager.LanguageList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(l);

            if (GUILayout.Button("Remove", GUILayout.Width(100f)) && EditorUtility.DisplayDialog("Are you sure?",
                                                l + " will be remove with its content and cannot be keep back after.",
                                                "Delete",
                                                "Cancel"))
            {
                RemoveLanguage(l);
                LanguageManager.DeleteKey = true;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Remove a language from Editor.
    /// </summary>
    /// <param name="language">The language to remove.</param>
    private void RemoveLanguage(string language)
    {
        LanguageManager.LanguageList.Remove(language);
        LanguageManager.KeyList.RemoveValueKey(language);
    }
}
