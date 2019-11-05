using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(GridMaker))]
public class GridMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridMaker gm = (GridMaker)target;
        if (GUILayout.Button("Build JPS Data"))
        {
            gm.BuildJPSData();
        }
        if (GUILayout.Button("JPS Path Test"))
        {
            gm.JSPTest();
        }
    }
}
#endif
