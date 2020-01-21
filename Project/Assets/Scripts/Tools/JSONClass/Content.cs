/// <summary>
/// This class is usefull for keep contents. 
/// </summary>
public class Content : IDictionary<string>
{
    /// <summary>
    /// Add a key inside the dictionnary.
    /// </summary>
    /// <param name="key">The key to add.</param>
    /// <returns>true if the key is not present.</returns>
    public override bool AddKey(string key)
    {
        if (!JSONDictionary.ContainsKey(key))
        {
            JSONDictionary.Add(key, "");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tell if a value is associated for a specific key.
    /// </summary>
    /// <param name="key">The key to search the associated value.</param>
    /// <returns>
    ///     true if a value is set, false otherwise.
    /// </returns>
    public override bool HaveValue(string key)
    {
        return JSONDictionary[key] != "";
    }
}
