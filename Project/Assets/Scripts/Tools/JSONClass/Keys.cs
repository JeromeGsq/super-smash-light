using System.Collections.Generic;

/// <summary>
/// This class is usefull for keep keys and associated content.
/// </summary>
public class Keys : IDictionary<Content>
{
    /// <summary>
    /// Tell if a value is associated for a specific key.
    /// </summary>
    /// <param name="key">The key to search the associated value.</param>
    /// <returns>
    ///     true if a value is set, false otherwise.
    /// </returns>
    public override bool HaveValue(string key)
    {
        return JSONDictionary[key] != default(Content);
    }

    /// <summary>
    /// Tell if all key contain languages filled
    /// </summary>
    /// <returns>
    ///     true if all value are set, false otherwise.
    /// </returns>
    public bool AllHaveValues(string key)
    {
        foreach (string language in JSONDictionary[key].Keys)
        {
            // Test for a specific key if a specific language is set or not
            if (JSONDictionary[key].JSONDictionary[language].Equals(""))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Add a key inside the dictionnary.
    /// </summary>
    /// <param name="key">The key to add.</param>
    /// <returns>true if the key is not present.</returns>
    public override bool AddKey(string key)
    {
        if (!JSONDictionary.ContainsKey(key))
        {
            // Add the key
            JSONDictionary.Add(key, new Content());
            return true;
        }
        return false;
    }

    /// <summary>
    /// Add a key inside the dictionnary.
    /// </summary>
    /// <param name="key">The key to add.</param>
    /// <param name="contents">The content to associate to a key.</param>
    /// <returns>true if the key is not present.</returns>
    public bool AddKey(string key, List<string> contents)
    {
        if (!JSONDictionary.ContainsKey(key))
        {
            // Add the key
            JSONDictionary.Add(key, new Content());

            // Add all associated content.
            foreach (string content in contents)
            {
                // Add the language to the key.
                JSONDictionary[key].AddKey(content);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Remove a key in all values.
    /// </summary>
    /// <param name="valueKey">The Key to remove in values.</param>
    public void RemoveValueKey(string valueKey)
    {
        foreach (string key in JSONDictionary.Keys)
        {
            JSONDictionary[key].JSONDictionary.Remove(valueKey);
        }
    }
}
