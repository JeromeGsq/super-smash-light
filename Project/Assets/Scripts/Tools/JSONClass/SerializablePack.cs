using System;

/// <summary>
/// Class for pack an array a T object for Json Serialization.
/// </summary>
/// <typeparam name="T">The type of array of element.</typeparam>
[Serializable]
public class SerializablePack<T>
{
    public T[] pack;
}
