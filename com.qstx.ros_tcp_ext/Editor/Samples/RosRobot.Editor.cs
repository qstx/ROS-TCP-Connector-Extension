using UnityEngine;
using UnityEditor;

namespace QSTX.ROSTCPConnector.Extension.Editor
{
    [CustomEditor(typeof(RosRobot))]
    public class RosRobotEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            RosRobot t = target as RosRobot;
            Undo.RecordObject(t, "Record");

            base.OnInspectorGUI();
            if (!Application.isPlaying)
                return;
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Action"))
            {
                t.MoveToTarget(t.msg);
            }

            if (!t.IsSubscribed)
            {
                if(GUILayout.Button("Subscribe"))
                    t.Subscribe();
            }
            else
            {
                if(GUILayout.Button("Unsubscribe"))
                    t.UnSubscribe();
            }

            EditorGUILayout.EndVertical();
        }
    }
}
