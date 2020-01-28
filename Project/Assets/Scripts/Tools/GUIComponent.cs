using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GUIComponent
{
    /// <summary>
    /// Little minus button.
    /// </summary>
    /// <returns>true if the button is clicked, false otherwise.</returns>
    public static bool MinusButton()
    {
        return GUILayout.Button("", (GUIStyle)"OL Minus", GUILayout.ExpandWidth(false));
    }

    /// <summary>
    /// Little plus button.
    /// </summary>
    /// <returns>true if the button is clicked, false otherwise.</returns>
    public static bool PlusButton()
    {
        return GUILayout.Button("", (GUIStyle)"OL Plus", GUILayout.ExpandWidth(false));
    }
}
