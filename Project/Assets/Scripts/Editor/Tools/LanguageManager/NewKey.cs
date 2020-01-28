using UnityEditor;
using UnityEngine;

/// <summary>
/// Window to add a key.
/// When a key is added, it is automaticaly saved in json file.
/// </summary>
public class NewKey : EditorWindow
{
    private string key = "default";

    /// <summary>
    /// Initialize the window usefull to create a new key.
    /// </summary>
    public static void Init()
    {
        NewKey window = CreateInstance<NewKey>();
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
        key = EditorGUILayout.TextField("Key to add : ", key);

        // Display the button only if the text field is not empty.
        if (key != null && key.Length > 0 && GUILayout.Button("Add the key"))
        {
            if (!LanguageManager.KeyList.AddKey(key, LanguageManager.LanguageList))
            {
                EditorUtility.DisplayDialog("Already exist!", "You have tried to had an existing key", "Cancel");
            }
            else
            {
                LanguageManager.Instance.Save();
            }
            this.Close();
        }
    }
}
