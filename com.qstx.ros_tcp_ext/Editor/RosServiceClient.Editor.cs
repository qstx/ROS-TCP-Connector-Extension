using UnityEditor;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using QSTX.ROSTCPConnector.Extension;

namespace QSTX.ROSTCPConnector.Extension.Editor
{
    public class RosServiceClientEditor<TRequest, TResponse> : UnityEditor.Editor
        where TRequest : Message where TResponse : Message, new()
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}