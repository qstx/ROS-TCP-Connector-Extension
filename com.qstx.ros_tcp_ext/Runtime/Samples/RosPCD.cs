using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RosMessageTypes.Sensor;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

namespace QSTX.ROSTCPConnector.Extension.Samples
{
    [AddComponentMenu(("ROS TCP Component/RosPCD"))]
    [RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
    public class RosPCD : RosTopicSubscriber<PointCloud2Msg>
    {
        private void ShowPCD(PointCloud2Msg msg)
        {
            uint cnt = (uint)msg.data.Length / msg.point_step;//msg.height * msg.width;
            vertices = new Vector3[cnt];
            colors = new Color[cnt];
            
            string[] channels = msg.fields.Select(field => field.name).ToArray();

            Dictionary<string, int> channelToIdx = new Dictionary<string, int>();
            for (int i = 0; i < msg.fields.Length; i++)
            {
                channelToIdx.Add(msg.fields[i].name, i);
            }
            
            Func<int, Color> colorGenerator = (int iPointStep) => Color.white;
            
            if (true/*m_UseRgbChannel*/)
            {
                // switch (ColorModeSetting)
                // {
                //     case ColorMode.HSV:
                //         if (m_HueChannel.Length > 0)
                //         {
                //             int hueChannelOffset = (int)message.fields[channelToIdx[m_HueChannel]].offset;
                //             colorGenerator = (int iPointStep) =>
                //             {
                //                 int colC = BitConverter.ToInt16(message.data, (iPointStep + hueChannelOffset));
                //                 return Color.HSVToRGB(Mathf.InverseLerp(m_HueRange[0], m_HueRange[1], colC), 1, 1);
                //             };
                //         }
                //         break;
                //     case ColorMode.SeparateRGB:
                        if (m_RChannel.Length > 0 && m_GChannel.Length > 0 && m_BChannel.Length > 0)
                        {
                            int rChannelOffset = (int)msg.fields[channelToIdx[m_RChannel]].offset;
                            int gChannelOffset = (int)msg.fields[channelToIdx[m_GChannel]].offset;
                            int bChannelOffset = (int)msg.fields[channelToIdx[m_BChannel]].offset;
                            colorGenerator = (int iPointStep) =>
                            {
                                var colR = BitConverter.ToSingle(msg.data, iPointStep + rChannelOffset);//Mathf.InverseLerp(m_RRange[0], m_RRange[1], BitConverter.ToSingle(message.data, iPointStep + rChannelOffset));
                                var colG = BitConverter.ToSingle(msg.data, iPointStep + gChannelOffset);//Mathf.InverseLerp(m_GRange[0], m_GRange[1], BitConverter.ToSingle(message.data, iPointStep + gChannelOffset));
                                var colB = BitConverter.ToSingle(msg.data, iPointStep + bChannelOffset);//Mathf.InverseLerp(m_BRange[0], m_BRange[1], BitConverter.ToSingle(message.data, iPointStep + bChannelOffset));
                                return new Color(colR, colG, colB, 1);
                            };
                        }
                //         break;
                //     case ColorMode.CombinedRGB:
                //         if (m_RgbChannel.Length > 0)
                //         {
                //             int rgbChannelOffset = (int)message.fields[channelToIdx[m_RgbChannel]].offset;
                //             colorGenerator = (int iPointStep) => new Color32
                //             (
                //                 message.data[iPointStep + rgbChannelOffset + 2],
                //                 message.data[iPointStep + rgbChannelOffset + 1],
                //                 message.data[iPointStep + rgbChannelOffset],
                //                 255
                //             );
                //         }
                //         break;
                // }
            }
            int xChannelOffset = (int)msg.fields[channelToIdx[m_XChannel]].offset;
            int yChannelOffset = (int)msg.fields[channelToIdx[m_YChannel]].offset;
            int zChannelOffset = (int)msg.fields[channelToIdx[m_ZChannel]].offset;
            
            for (int i = 0; i < cnt; i++)
            {
                int iPointStep = i * (int)msg.point_step;
                var x = BitConverter.ToSingle(msg.data, iPointStep + xChannelOffset);
                var y = BitConverter.ToSingle(msg.data, iPointStep + yChannelOffset);
                var z = BitConverter.ToSingle(msg.data, iPointStep + zChannelOffset);
                Vector3<FLU> rosPoint = new Vector3<FLU>(x, y, z);
                vertices[i] = rosPoint.toUnity;
                colors[i] = colorGenerator(iPointStep);
            }
            
            mesh.Clear();
            mesh.indexFormat = cnt < 65536 ? UnityEngine.Rendering.IndexFormat.UInt16 : UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.vertices = vertices;
            mesh.colors = colors;
        }

        protected override void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh=mesh;
            
            Subscribe();
            MsgCallback += ShowPCD;
        }
        [SerializeField]
        private string m_RChannel = "r", m_GChannel = "g", m_BChannel = "b";
        [SerializeField]
        private string m_XChannel = "x", m_YChannel = "y", m_ZChannel = "z";

        private Mesh mesh;
        private Vector3[] vertices;
        private Color[] colors;

#if UNITY_EDITOR
        [SerializeField]
        private PointCloud2Msg msg;
#endif
    }
}
