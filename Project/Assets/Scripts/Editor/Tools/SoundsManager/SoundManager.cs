using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Sound Manager is a tool usefull for Game Designer to easily integrate new sounds inside project in Unity.
/// </summary>
public class SoundManager : EditorWindow
{
    // Represent the instance of this class
    public static SoundManager Instance { get; private set; }

    static public Keys KeyList { get; private set; }
    static public List<string> Content { get; private set; } = new List<string>() { "AudioPath", "Comment", "WrittenAudio" };
    public bool DeleteKey { get; set; } = false;

    // Attributes
    private string searchKey = "";
    private Vector2 scrollBar;
    private string jsonPath = "Assets/Resources/sounds.json";


    [MenuItem("Tools/Sound Manager")]
    public static void Init()
    {
        // Create the instance
        Instance = EditorWindow.GetWindow(typeof(SoundManager)) as SoundManager;

        Instance.wantsMouseMove = true;
        // Show it
        Instance.Show();

        SoundManager.Instance.LoadJson();
    }

    /// <summary>
    /// Main Windows of Sound Manager 
    /// </summary>
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Key"))
        {
            NewSoundKey.Init();        
        }
        if (GUILayout.Button("Save"))
        {
            Save();
        }
        GUILayout.EndHorizontal();
        searchKey = EditorGUILayout.TextField(searchKey, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.Width(500f));
        DisplayKeys();
    }

    /// <summary>
    /// Pop a window if something is deleting and not saving.
    /// Two choice:
    ///     Do not save
    ///     Save
    /// </summary>
    private void OnDestroy()
    {
        if (DeleteKey)
        {
            if (!EditorUtility.DisplayDialog("Save before quit?",
                                        "Some keys or values have been destroyed but you don't save them. Would you like to save modifications",
                                        "Do not Save",
                                        "Save"))
            {
                Save();
            }
        }
        DeleteKey = false;
    }

    /// <summary>
    /// Load content of JSON file into memory of sound manager.
    /// </summary>
    private void LoadJson()
    {
        // Create json file if it does not exist
        if (!System.IO.Directory.Exists("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        if (!System.IO.File.Exists(jsonPath))
        {
            Utils.WriteJson(jsonPath, "{\"pack\":[]}");
        }

        // Store json content in memory
        string jsonContent = Utils.ReadJson(jsonPath);
        Assert.AreNotEqual("", jsonContent, "Invalid Json.");
        SerializablePack<SerializableKeys> pack = JsonUtility.FromJson<SerializablePack<SerializableKeys>>(jsonContent);
        Keys keys = Utils.SerializeKeyPackToDict(pack);
        KeyList = keys;
    }

    /// <summary>
    /// Display all keys present in the json file.
    /// </summary>
    private void DisplayKeys()
    {
        scrollBar = GUILayout.BeginScrollView(scrollBar);
        // Create a list to sort keys.
        List<string> keys = new List<string>();

        // Get keys from dictionary
        if (KeyList.Keys != null)
        {
            foreach (string key in KeyList.Keys)
            {
                keys.Add(key);
            }
        }

        // Sort them in alphabetical order
        keys.Sort();

        // Display them
        foreach (string key in keys)
        {
            // Filter to searched key.
            if (key.Contains(searchKey))
            {
                GUILayout.BeginHorizontal();
                // Green is good, orange is not
                GUIStyle color = KeyList.AllHaveValues(key) ? (GUIStyle)"sv_label_3" : (GUIStyle)"sv_label_5";
                GUILayout.Label(key, color);
                GUILayout.Space(10f);
                if (GUILayout.Button("Edit Content", GUILayout.Width(100f)))
                {
                    EditSound.Init(key);
                }
                if (GUILayout.Button("Remove", GUILayout.Width(100f)))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", key + " will be remove with its content and cannot be keep back after.", "Delete", "Cancel"))
                    {
                        KeyList.RemoveKey(key);
                        DeleteKey = true;
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// This function save the JSON of cache in a real file.
    /// </summary>
    public void Save()
    {
        // Convert Keys object into json string.
        SerializablePack<SerializableKeys> keys = Utils.DictToSerializeKeyPack(KeyList);
        string json = JsonUtility.ToJson(keys);

        // Write it into the json file
        Utils.WriteJson(jsonPath, json);

        // Reset the DeleteKey
        DeleteKey = false;
    }
}
