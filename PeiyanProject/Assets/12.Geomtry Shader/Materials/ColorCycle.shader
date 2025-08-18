Shader "Custom/ColorCycle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 获取当前时间并乘以速度
                float time = _Time.y * _Speed;
                // 使用sin函数生成周期性变化的颜色值
                float3 color = float3(sin(time), sin(time + 2.1), sin(time + 4.2));
                // 将颜色值归一化到0-1范围
                color = (color + 1.0) * 0.5;
                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}