using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TransformRecorder
{
    [CustomEditor(typeof(TransformRecorder))]
    public class TransformRecorderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TransformRecorder transformRecorder = target as TransformRecorder;

            if(EditorApplication.isPlaying)
            {
                if(transformRecorder.mode == TransformRecorder.Mode.Record)
                {
                    DrawRecordModeUI(transformRecorder);
                }
                else
                {
                    DrawPlaybackUI(transformRecorder);
                }
            }
        }

        void DrawRecordModeUI(TransformRecorder transformRecorder)
        {
            if(transformRecorder.IsRecording())
            {
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Stop Recording"))
                {
                    transformRecorder.StopRecording();
                }
            }
            else
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Start Recording"))
                {
                    transformRecorder.StartRecording();
                }
            }
        }

        void DrawPlaybackUI(TransformRecorder transformRecorder)
        {
            if (GUILayout.Button("Play"))
            {
                transformRecorder.StartPlayback();
            }
        }
    }
}