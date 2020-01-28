/// <summary>
/// Class for add methods to string class.
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Auxiliary function
    /// </summary>
    /// <param name="str">String that have to be cut.</param>
    /// <param name="last">Indicator of the last occurence of string last.</param>
    /// <param name="before">Determine if it is before or after last function.</param>
    /// <param name="caseSensitive">Indicate if the last string is caseSensitive or not. By default it is.</param>
    /// <returns>The wanted string.</returns>
    private static string AuxiliaryLast(string str, string last, bool before, bool caseSensitive=true)
    {
        if (str != null && last != null)
        {
            if (last == "")
            {
                return before ? str : "";
            }

            string strTmp = str;

            if (!caseSensitive)
            {
                strTmp = strTmp.ToLower();
                last = last.ToLower();
            }

            int index = strTmp.LastIndexOf(last);

            if (index != -1)
            {
                int startIndex = before ? 0 : index + last.Length;
                int length = before ? index : str.Length - (startIndex);
                return str.Substring(startIndex, length);
            }
        }
        return str;
    }

    /// <summary>
    /// Return the string before the last string specified.
    /// </summary>
    /// <param name="str">String that have to be cut.</param>
    /// <param name="last">Indicator of the last occurence of string last.</param>
    /// <param name="caseSensitive">Indicate if the last string is caseSensitive or not. By default it is.</param>
    /// <returns>Substring that does not contain the last string.</returns>
    public static string BeforeLast(this string str, string last, bool caseSensitive=true)
    {
        return AuxiliaryLast(str, last, true, caseSensitive);
    }

    /// <summary>
    /// Return the string after the last string specified.
    /// </summary>
    /// <param name="str">String that have to be cut.</param>
    /// <param name="last">Indicator of the last occurence of string last.</param>
    /// <param name="caseSensitive">Indicate if the last string is caseSensitive or not. By default it is.</param>
    /// <returns>Substring that does not contain the last string and all elements before.</returns>
    public static string AfterLast(this string str, string last, bool caseSensitive=true)
    {
        return AuxiliaryLast(str, last, false, caseSensitive);
    }

    /// <summary>
    /// Return a boolean that indicate if the string is empty or not.
    /// Null and "" is considered like empty.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>true if the string is empty, false otherwise.</returns>
    public static bool Empty(this string str)
    {
        return str == null || str.Equals("");
    }
}
