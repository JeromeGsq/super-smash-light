using UnityEditor;
using UnityEngine;

/// <summary>
/// The window for editing a sound
/// </summary>
public class EditSound : EditorWindow
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
        content = SoundManager.KeyList.JSONDictionary[key];

        EditSound window = CreateInstance<EditSound>();
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

        DisplayContent();

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
            if (SoundManager.KeyList.UpdateKey(key, newKey))
            {
                EditorUtility.DisplayDialog("Update", "You have change the key " + key + " by " + newKey, "Cancel");
                key = newKey;
                SoundManager.Instance.Save();
            }
            else
            {
                EditorUtility.DisplayDialog("Already exist!", "You have tried to had an existing key", "Cancel");
            }
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Display all content name and their field to update contents.
    /// </summary>
    private void DisplayContent()
    {
        // Header
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.Label("Content");

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);

        // We iterate on each languages associated to a key.
        scrollBar = GUILayout.BeginScrollView(scrollBar);

        // Sort Content
        SoundManager.Content.Sort();
        EditorGUIUtility.labelWidth = 200f;
        foreach (string c in SoundManager.Content)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(c, GUILayout.ExpandWidth(false));
            GUILayout.Space(10f);
            // Use element to pass the problem of enumerator.
            string element = content.JSONDictionary[c];
            element = EditorGUILayout.TextArea(element, GUILayout.Height(100f));
            if (!element.Equals(content.JSONDictionary[c]))
            {
                content.JSONDictionary[c] = element;
                SoundManager.Instance.Save();
            }
            GUILayout.EndHorizontal();
        }
        EditorGUIUtility.labelWidth = 150f;
        GUILayout.EndScrollView();
    }
}
