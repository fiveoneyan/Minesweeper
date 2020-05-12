using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace komal.puremvc {
    [CustomEditor(typeof(ScrollRectEx), true)]
    public class ScrollRectExInspector : UnityEditor.Editor 
    {
        int index = 0;
        float time = 0.1f;
        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            ScrollRectEx scroll = (ScrollRectEx)target;
            GUI.enabled = Application.isPlaying;

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("FillCells"))
            {
                scroll.FillCells();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = 45;
            float w = (EditorGUIUtility.currentViewWidth - 100) / 2;
            EditorGUILayout.BeginHorizontal();
            index = EditorGUILayout.IntField("Index", index, GUILayout.Width(w));
            time = EditorGUILayout.FloatField("Time", time, GUILayout.Width(w));
            if(GUILayout.Button("Scroll", GUILayout.Width(45)))
            {
                scroll.ScrollToIndex(index, time);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
