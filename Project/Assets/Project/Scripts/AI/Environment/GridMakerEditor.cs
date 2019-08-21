using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GridMaker))]
public class GridMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridMaker gm = (GridMaker)target;
        if (GUILayout.Button("Build JSP Data"))
        {
            gm.BuildJPSData();
        }
    }
}