using System.Collections.Generic;
using UnityEngine;

public class JSONAccessAPI
{
    private enum JSONType { Languages, Sounds }

    private static string languages = "Assets/Resources/languages.json";
    private static string sounds = "Assets/Resources/sounds.json";

    /// <summary>
    /// Get the JSONContent of the good json.
    /// </summary>
    /// <param name="type">Type of JSON to parse.</param>
    /// <returns>Return a Keys object associated to a JSON type.</returns>
    private static Keys GetJSONContent(JSONType type)
    {
        string jsonContent = "";

        switch (type)
        {
            case JSONType.Languages:
                jsonContent = Utils.ReadJson(languages);
                break;

            case JSONType.Sounds:
                jsonContent = Utils.ReadJson(sounds);
                break;
        }
        SerializablePack<SerializableKeys> pack = JsonUtility.FromJson<SerializablePack<SerializableKeys>>(jsonContent);

        return Utils.SerializeKeyPackToDict(pack);
    }

    /// <summary>
    /// Get the content associated to a key and its given contentKey.
    /// </summary>
    /// <param name="type">Type of JSON to parse.</param>
    /// <param name="key">Key associated to the content.</param>
    /// <param name="contentKey">Content Key associated to the content.</param>
    /// <returns>Return the content associated to a </returns>
    private static string GetContent(JSONType type, string key, string contentKey)
    {
        Keys keys = GetJSONContent(type);
        return keys.JSONDictionary[key].JSONDictionary[contentKey];
    }

    /// <summary>
    /// Get all keys in the JSON.
    /// </summary>
    /// <param name="type">Type of JSON to parse.</param>
    /// <returns>All Keys associated to the given type.</returns>
    private static List<string> GetKeys(JSONType type)
    {
        Keys keys = GetJSONContent(type);
        List<string> listKeys = new List<string>();
        if (keys != null)
        {
            foreach (string key in keys.Keys)
            {
                listKeys.Add(key);
            }
        }
        listKeys.Sort();
        return listKeys;
    }

    /// <summary>
    ///  Get all keys in the JSON associated to sounds.
    /// </summary>
    /// <returns>All keys associated to audio.</returns>
    public static List<string> GetAudioKeys()
    {
        return GetKeys(JSONType.Sounds);
    }

    /// <summary>
    ///  Get all keys in the JSON associated to languages.
    /// </summary>
    /// <returns>All keys associated to languages.</returns>
    public static List<string> GetTextKeys()
    {
        return GetKeys(JSONType.Languages);
    }

    /// <summary>
    /// Return the path associated to a specific key.
    /// </summary>
    /// <param name="key">The key associated to the Sound.</param>
    /// <returns>Return the path associated to the key.</returns>
    public static string GetAudioPath(string key)
    {
        return GetContent(JSONType.Sounds, key, "AudioPath");
    }

    /// <summary>
    /// Return the text associated to a specific key.
    /// </summary>
    /// <param name="key">The key associated to the Sound.</param>
    /// <returns>Return the text associated to the key.</returns>
    public static string GetAudioText(string key)
    {
        return GetContent(JSONType.Sounds, key, "WrittenAudio");
    }

    /// <summary>
    /// Return the content associated associated to a specific key.
    /// </summary>
    /// <param name="key">The key associated to the text.</param>
    /// <returns>Return the content associated to the key.</returns>
    public static string GetContentText(string key, string language)
    {
        return GetContent(JSONType.Languages, key, language);
    }
}
