using System;
using UnityEditor;
using UnityEngine;

public class CaptureScreenTool
{


    public static void ScreenShot(int screenshotSize)
    {
        var formatedDate = DateTime.Now.ToString().Replace('/', '_').Replace(':', '_');
        ScreenCapture.CaptureScreenshot("Assets/Screenshots/Screenshot_" + formatedDate + ".png", screenshotSize);
    }

    [MenuItem("Tools/Game Window Screenshot/Resolution X1")]
    public static void CaptureAtResolutionX1()
    {
        ScreenShot(1);
    }


    [MenuItem("Tools/Game Window Screenshot/Resolution X2")]
    public static void CaptureAtResolutionX2()
    {
        ScreenShot(2);
    }


    [MenuItem("Tools/Game Window Screenshot/Resolution X3")]
    public static void CaptureAtResolutionX3()
    {
        ScreenShot(3);
    }


    [MenuItem("Tools/Game Window Screenshot/Resolution X4")]
    public static void CaptureAtResolutionX4()
    {
        ScreenShot(4);
    }
}
