using System.Collections.Generic;

/// <summary>
/// Generic class to contains dictionary of elements.
/// </summary>
/// <typeparam name="T">The type of elements.</typeparam>
abstract public class IDictionary<T>
{
    public Dictionary<string, T> JSONDictionary { get; protected set; } = new Dictionary<string, T>();

    public Dictionary<string, T>.KeyCollection Keys => JSONDictionary.Keys;

    /// <summary>
    /// Add or update content associated to a key in the dictionnary.
    /// </summary>
    /// <param name="key">The key on which we put the content.</param>
    /// <param name="newContent">The content to associate to the key.</param>
    public void UpdateContent(string key, T newContent)
    {
        if (JSONDictionary.ContainsKey(key))
        {
            JSONDictionary[key] = newContent;
        }
        else
        {
            JSONDictionary.Add(key, newContent);
        }
    }

    /// <summary>
    /// Change an existing key to another. 
    /// </summary>
    /// <param name="oldKey">The previous key name.</param>
    /// <param name="newKey">The wanted key name.</param>
    /// <returns>
    ///     true if the newKey is correctly update,
    ///     false if the newKey is already in dictionnary.
    /// </returns>
    public bool UpdateKey(string oldKey, string newKey)
    {
        if (JSONDictionary.ContainsKey(newKey))
        {
            return false;
        }
        T content = JSONDictionary[oldKey];
        JSONDictionary.Remove(oldKey);
        JSONDictionary.Add(newKey, content);
        return true;
    }

    /// <summary>
    /// Add a key inside the dictionnary.
    /// </summary>
    /// <param name="key">The key to add.</param>
    /// <returns>true if the key is not present, false otherwise.</returns>
    abstract public bool AddKey(string key);

    /// <summary>
    /// Delete from the dictionary the specified key value.
    /// </summary>
    /// <param name="key">Key of the element to delete.</param>
    /// <returns>
    ///     true if the search and the deleting of the element is successful ; false otherwise. 
    ///     This method return false if the key is not found in the dictionary
    /// </returns>
    public bool RemoveKey(string key)
    {
        return JSONDictionary.Remove(key);
    }

    /// <summary>
    /// Tell if a value is associated for a specific key.
    /// </summary>
    /// <param name="key">The key to search the associated value.</param>
    /// <returns>
    ///     true if a value is set, false otherwise.
    /// </returns>
    abstract public bool HaveValue(string key);
}
