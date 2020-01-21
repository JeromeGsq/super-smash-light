using UnityEditor;
using UnityEngine;

/// <summary>
/// Window to remove a key.
/// When a key is removed, it is not save in the json file.
/// We need to click on save to do it.
/// </summary>
public class RemoveKey : EditorWindow
{
    private string key = "";

    /// <summary>
    /// Initialize the window usefull to create a new key.
    /// </summary>
    public static void Init()
    {
        RemoveKey window = CreateInstance<RemoveKey>();
        window.ShowUtility();
        const int w = 512;
        const int h = 128;
        // Center the window.
        window.position = new Rect(Screen.currentResolution.width / 2f - w / 2,
                                   Screen.currentResolution.height / 2f - h / 2,
                                   w, h);
    }

    /// <summary>
    /// The window content for creating a key
    /// </summary>
    private void OnGUI()
    {
        // The text field
        key = EditorGUILayout.TextField("Key to remove : ", key);

        // Display the button only if the text field is not empty.
        if (key != null && key.Length > 0 && GUILayout.Button("Remove the key"))
        {
            if (!EditorUtility.DisplayDialog("Are you sure?", key + " will be remove with its content and cannot be keep back after.", "Cancel", "Delete"))
            {
                string message;
                string button;
                if (LanguageManager.KeyList.RemoveKey(key))
                {
                    message = key + " has been removed correctly.";
                    button = "Ok";
                }
                else
                {
                    message = "An error occured while deleting " + key + ", maybe this key does not exist.";
                    button = "Cancel";
                }
                EditorUtility.DisplayDialog("Confirmation", message, button);
                this.Close();
            }
        }
    }
}
