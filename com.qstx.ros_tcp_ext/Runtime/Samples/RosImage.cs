using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.Sensor;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using UnityEditor;
using UnityEngine.UI;

namespace QSTX.ROSTCPConnector.Extension.Samples
{
    [AddComponentMenu(("ROS TCP Component/RosImage"))]
    public class RosImage : RosTopicSubscriber<CompressedImageMsg>
    {
        [SerializeField] private RawImage image;
        private void ShowImage(CompressedImageMsg msg)
        {
            DestroyImmediate(image.texture);
            image.texture = msg.ToTexture2D();
        }
        protected override void Start()
        {
            Subscribe();
            MsgCallback += ShowImage;
        }
    }
}
