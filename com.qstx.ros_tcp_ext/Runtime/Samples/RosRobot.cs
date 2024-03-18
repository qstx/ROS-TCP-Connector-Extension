using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QSTX.ROSTCPConnector.Extension;
using RosMessageTypes.Sensor;

namespace QSTX.ROSTCPConnector.Extension.Samples
{
    [AddComponentMenu(("ROS TCP Component/RosRobot"))]
    public class RosRobot : RosTopicSubscriber<JointStateMsg>
    {
        [SerializeField] private ArticulationBody[] joints;
        [SerializeField] private bool isDeg = true;
        public void MoveToTarget(JointStateMsg msg)
        {
            for (int i = 0; i < joints.Length; ++i)
            {
                var driver=joints[i].xDrive;
                driver.target = isDeg?(float)msg.position[i]:(float)msg.position[i]*Mathf.Rad2Deg;
                joints[i].xDrive = driver;
            }
        }

        protected override void Start()
        {
            Subscribe();
            MsgCallback += MoveToTarget;
        }
#if UNITY_EDITOR
        [Header("Only For 【RUNTIME】 Test")]
        public JointStateMsg msg;
#endif
    }
}
