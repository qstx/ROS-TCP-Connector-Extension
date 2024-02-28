using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QSTX.ROSTCPConnector.Extension;
using RosMessageTypes.Sensor;

namespace QSTX.ROSTCPConnector.Extension
{
    [AddComponentMenu(("ROS TCP Component/RosRobot"))]
    public class RosRobot : RosTopicSubscriber<JointStateMsg>
    {
        [SerializeField] private ArticulationBody[] joints;
        public void MoveToTarget(JointStateMsg msg)
        {
            for (int i = 0; i < joints.Length; ++i)
            {
                var driver=joints[i].xDrive;
                driver.target = (float)msg.position[i];
                joints[i].xDrive = driver;
            }
        }
#if UNITY_EDITOR
        [Header("Only For 【RUNTIME】 Test")]
        public JointStateMsg msg;
#endif
    }
}
