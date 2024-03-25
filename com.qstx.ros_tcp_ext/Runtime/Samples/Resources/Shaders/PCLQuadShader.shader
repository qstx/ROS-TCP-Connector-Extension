Shader "Point Cloud/Quad"
{
    Properties
    {
        _PointSize("Point Size", Float) = 0.1
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color:COLOR0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2g
            {
                float4 vertex : POSITION;
                float4 color:COLOR0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            struct g2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR0;
            };

            float _PointSize;
            fixed4 _Color;

            v2g vert (appdata v)
            {
                v2g o;
                UNITY_SETUP_INSTANCE_ID(o);
                //UNITY_INITIALIZE_OUTPUT(FSInput, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = v.vertex;
                o.color=v.color;
                return o;
            }

            [maxvertexcount(6)]
            void geom(point v2g IN[1], inout TriangleStream<g2f> triStream)
            {
                g2f OUT;

                // 计算四边形的四个顶点
                float3 vertices[4] = {
                    float3(-0.5, -0.5, 0),
                    float3(0.5, -0.5, 0),
                    float3(0.5, 0.5, 0),
                    float3(-0.5, 0.5, 0)
                };
                
                // 定义两个三角形的顶点索引（以形成正方形）
                int indices[6] = {0, 1, 2, 0, 2, 3};

                // 生成两个三角形
                for (int i = 0; i < 6; i++)
                {
                    float3 camPos = UnityObjectToViewPos(IN[0].vertex) + float4(vertices[indices[i]] * _PointSize, 0);
                    OUT.vertex = UnityViewToClipPos(camPos);
                    OUT.color = IN[0].color;
                    triStream.Append(OUT);
                }
                // // 调整四边形的大小和位置
                // for (int i = 0; i < 4; i++)
                // {
                //     float3 camPos = UnityObjectToViewPos(IN[0].vertex) + float4(vertices[i] * _PointSize, 0);
                //     OUT.vertex = UnityViewToClipPos(camPos);
                //     OUT.color = IN[0].color;
                //     triStream.Append(OUT);
                // }

                triStream.RestartStrip();
            }

            fixed4 frag (g2f i) : SV_Target
            {
                return i.color*_Color/**float4(0.454,0.454,0.454,1)*/; // 使用设置的颜色
            }
            ENDCG
        }
    }
}
