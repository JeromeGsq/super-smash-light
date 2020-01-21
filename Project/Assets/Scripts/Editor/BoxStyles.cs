using UnityEngine;

/// <summary>
/// Style for box.
/// </summary>
public static class BoxStyles
{
    #region Attributes
    private static GUIStyle blueBoxStyle = null;
    private static GUIStyle cyanBoxStyle = null;
    private static GUIStyle greenBoxStyle = null;
    private static GUIStyle yellowBoxStyle = null;
    private static GUIStyle orangeBoxStyle = null;
    private static GUIStyle redBoxStyle = null;
    private static GUIStyle greyBoxStyle = null;
    private static GUIStyle darkGreyBoxStyle = null;
    private static GUIStyle lightGreyBoxStyle = null;
    #endregion

    /// <summary>
    /// Return a style for a box an assign it to the static attribute.
    /// </summary>
    /// <param name="guiBoxStyle">Reference to the static attribute to store the GUIStyle.</param>
    /// <param name="stringStyle">The string that represent the GUIStyle.</param>
    /// <returns></returns>
    private static GUIStyle GetBoxStyle(ref GUIStyle guiBoxStyle, string stringStyle)
    {
        if (guiBoxStyle == null)
        {
            guiBoxStyle = new GUIStyle((GUIStyle)stringStyle)
            {
                padding = new RectOffset(0, 0, 0, 0)
            };
        }
        return guiBoxStyle;
    }

    #region Properties
    public static GUIStyle BlueBoxStyle
    {
        get
        {
            return GetBoxStyle(ref blueBoxStyle, "flow node 1");
        }
    }


	public static GUIStyle CyanBoxStyle
	{
		get
		{
            return GetBoxStyle(ref cyanBoxStyle, "flow node 2");
        }
	}

    public static GUIStyle GreenBoxStyle
    {
        get
        {
            return GetBoxStyle(ref greenBoxStyle, "flow node 3");
        }
    }

    public static GUIStyle YellowBoxStyle
    {
        get
        {
            return GetBoxStyle(ref yellowBoxStyle, "flow node 4");
        }
    }

    public static GUIStyle OrangeBoxStyle
    {
        get
        {
            return GetBoxStyle(ref orangeBoxStyle, "flow node 5");
        }
    }

    public static GUIStyle RedBoxStyle
    {
        get
        {
            return GetBoxStyle(ref redBoxStyle, "flow node 6");
        }
    }

    public static GUIStyle GreyBoxStyle
    {
        get
        {
            return GetBoxStyle(ref greyBoxStyle, "flow node 0");
        }
    }

    public static GUIStyle LightGreyBoxStyle
    {
        get
        {
            return GetBoxStyle(ref lightGreyBoxStyle, "TE NodeBoxSelected");
        }
    }

    [System.Obsolete("Dark style is not supported. Use another style.")]
    public static GUIStyle DarkGreyBoxStyle
    {
        get
        {
            return GetBoxStyle(ref darkGreyBoxStyle, "ShurikenEffectBg");
        }
    }
    #endregion
}