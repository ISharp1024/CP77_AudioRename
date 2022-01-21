using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Convert))]
public class ConvertEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("修改Json"))
        {
            Convert convert = target as Convert;
            convert.ModifyJson();
        }

        if (GUILayout.Button("修改字幕Json"))
        {
            Convert convert = target as Convert;
            convert.ModifySubtileJsonBatch();
        }
    }

}
