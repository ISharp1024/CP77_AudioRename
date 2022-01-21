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

        if (GUILayout.Button("����TXT�ļ�"))
        {
            JsonReader jr = target as JsonReader;
            jr.CreateTXT();
        }

        if (GUILayout.Button("ת��JSON�ļ�"))
        {
            JsonReader jr = target as JsonReader;
            jr.ConvertJson();
        }

        if (GUILayout.Button("���TXT�ļ�"))
        {
            JsonReader jr = target as JsonReader;
            jr.ClearTXTFolder();
        }

        if (GUILayout.Button("����TXT�ļ�"))
        {
            JsonReader jr = target as JsonReader;
            jr.AssembleJson();
        }
    }
}
