using System;
using UnityEngine;
using UnityEditor;

namespace QSTX.ROSTCPConnector.Extension.Editor
{
    [CustomEditor(typeof(RosRobot))]
    public class RosRobotEditor : UnityEditor.Editor
    {
        private SerializedProperty degOrradProperty;
        private SerializedProperty msgEventProperty;
        private SerializedProperty topicNameProperty;
        private SerializedProperty jointsProperty;
        private SerializedProperty testMsgProperty;

        private void OnEnable()
        {
            degOrradProperty = serializedObject.FindProperty("isDeg");
            msgEventProperty = serializedObject.FindProperty("msgEvent");
            topicNameProperty = serializedObject.FindProperty("topicName");
            jointsProperty = serializedObject.FindProperty("joints");
            testMsgProperty = serializedObject.FindProperty("msg");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            RosRobot t = target as RosRobot;
            
            if (degOrradProperty.boolValue && GUILayout.Button("Use Degrees"))
                degOrradProperty.boolValue = false;
            else if(!degOrradProperty.boolValue && GUILayout.Button("Use Radians"))
                degOrradProperty.boolValue = true;
            EditorGUILayout.PropertyField(msgEventProperty);
            EditorGUILayout.PropertyField(jointsProperty);
            EditorGUILayout.PropertyField(topicNameProperty);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(testMsgProperty);
            serializedObject.ApplyModifiedProperties();
            
            if (!Application.isPlaying)
                return;
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Action"))
                t.MoveToTarget(t.msg);

            if (!t.IsSubscribed)
            {
                if(GUILayout.Button("Subscribe"))
                    t.Subscribe();
            }
            else if(GUILayout.Button("Unsubscribe"))
                    t.UnSubscribe();

            EditorGUILayout.EndVertical();
        }
    }
}
