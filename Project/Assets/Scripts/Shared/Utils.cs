using System;
using System.IO;
using UnityEngine;

public  static class Utils
{
    /// <summary>
    /// Convert a distance in float to its distance in meters and kilometers.
    /// </summary>
    /// <param name="distance">The distance in float to convert.</param>
    /// <returns>The distance in string converted.</returns>
    public static string DistanceToString(float distance)
    {
        string formatted;
        // Take care negative distances.
        if (Mathf.Abs(distance) >= 1000f)
        {
            formatted = String.Format("{0:0.0} ", (distance / 1000f)) + "km";
        }
        else
        {
            formatted = String.Format("{0:0} ", distance) + "m";
        }
        return formatted;
    }

    #region JSONConverter

    /// <summary>
    /// Convert a SerializablePack<SerializableKeys> to a Keys object.
    /// </summary>
    /// <param name="pack">The pack to convert.</param>
    /// <returns>
    ///     Return the Keys object to store elements in memory.
    /// </returns>
    public static Keys SerializeKeyPackToDict(SerializablePack<SerializableKeys> pack)
    {
        // Extract Serializable keys from wrapper.
        SerializableKeys[] keys = pack.pack;

        // Create the dict.
        Keys keysDict = new Keys();
        for (int i = 0; i < keys.Length; ++i)
        {
            // Add the key into dict
            keysDict.AddKey(keys[i].key);
            // Add values associated with the key.
            for (int j = 0; j < keys[i].elts.Length; ++j)
            {
                keysDict.JSONDictionary[keys[i].key].AddKey(keys[i].elts[j].elt);
                keysDict.JSONDictionary[keys[i].key].UpdateContent(keys[i].elts[j].elt, keys[i].elts[j].content);
            }
        }

        return keysDict;
    }

    /// <summary>
    /// Convert a Keys object to a SerializablePack<SerializableKeys> object.
    /// </summary>
    /// <param name="keys">The Keys object to convert.</param>
    /// <returns>
    ///     Return the SerializablePack<SerializableKeys> to store it in a JSON.
    /// </returns>
    public static SerializablePack<SerializableKeys> DictToSerializeKeyPack(Keys keys)
    {
        // Initialize SerializableKeys array to return into a pack.
        SerializableKeys[] serializableKeys = new SerializableKeys[keys.JSONDictionary.Keys.Count];
        int keyCpt = 0;

        foreach (var valuePair in keys.JSONDictionary)
        {
            SerializableKeys elt = new SerializableKeys
            {
                key = valuePair.Key
            };

            SerializableElements[] serializableElements = new SerializableElements[valuePair.Value.JSONDictionary.Keys.Count];
            int eltsCpt = 0;

            foreach (var valPair in valuePair.Value.JSONDictionary)
            {
                // Assign a new SerializableElements and increment the counter after the assignment
                serializableElements[eltsCpt++] = new SerializableElements
                {
                    elt = valPair.Key,
                    content = valPair.Value
                };
            }

            elt.elts = serializableElements;

            serializableKeys[keyCpt++] = elt;
        }

        // Pack the Serializable Keys and return them.
        return new SerializablePack<SerializableKeys>
        {
            pack = serializableKeys
        };
    }

    /// <summary>
    /// Write the jsonContent in the jsonPath.
    /// </summary>
    /// <param name="jsonPath">Path of the file where the content have to be written.</param>
    /// <param name="jsonContent">The content to write.</param>
    public static void WriteJson(string jsonPath, string jsonContent)
    {
        using (StreamWriter w = new StreamWriter(jsonPath))
        {
            w.WriteLine(jsonContent);
        }
    }

    /// <summary>
    /// Read the content of the jsonPath and return it in a string.
    /// </summary>
    /// <param name="jsonPath">The path of the file that have to be red.</param>
    /// <returns>The content red in the jsonPath.</returns>
    public static string ReadJson(string jsonPath)
    {
        string result;
        using (StreamReader r = new StreamReader(jsonPath))
        {
            result = r.ReadToEnd();
        }
        return result;
    }
    #endregion
}
