using System;

/// <summary>
/// Class for Serialization into JSON
/// </summary>
[Serializable]
public struct SerializableKeys
{
    public string key;
    public SerializableElements[] elts;
}
