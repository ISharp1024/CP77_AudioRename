using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JsonReader))]
public class JsonReaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("创建TXT文件"))
        {
            JsonReader jr = target as JsonReader;
            jr.CreateTXT();
        }

        if (GUILayout.Button("转换JSON文件"))
        {
            JsonReader jr = target as JsonReader;
            jr.ConvertJson();
        }

        if (GUILayout.Button("清空TXT文件"))
        {
            JsonReader jr = target as JsonReader;
            jr.ClearTXTFolder();
        }

        if (GUILayout.Button("整合TXT文件"))
        {
            JsonReader jr = target as JsonReader;
            jr.AssembleJson();
        }
    }
}
