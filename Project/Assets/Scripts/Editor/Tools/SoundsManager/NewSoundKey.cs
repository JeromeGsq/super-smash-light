using UnityEditor;
using UnityEngine;

/// <summary>
/// Window for creating a new sound key.
/// </summary>
public class NewSoundKey : EditorWindow
{
    private string key = "default";

    /// <summary>
    /// Initialize the window usefull to create a new sound key.
    /// </summary>
    public static void Init()
    {
        NewSoundKey window = CreateInstance<NewSoundKey>();
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
            if (!SoundManager.KeyList.AddKey(key, SoundManager.Content))
            {
                EditorUtility.DisplayDialog("Already exist!", "You have tried to had an existing key", "Cancel");
            }
            else
            {
                SoundManager.Instance.Save();
            }
            this.Close();
        }
    }
}
