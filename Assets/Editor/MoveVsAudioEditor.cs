using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveVsAudio))]
public class MoveVsAudioEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("筛分男女语音"))
        {
            MoveVsAudio move = target as MoveVsAudio;
            move.DivideM_F();
        }
    }
}
