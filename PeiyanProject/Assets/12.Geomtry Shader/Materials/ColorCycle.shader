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
                // ��ȡ��ǰʱ�䲢�����ٶ�
                float time = _Time.y * _Speed;
                // ʹ��sin�������������Ա仯����ɫֵ
                float3 color = float3(sin(time), sin(time + 2.1), sin(time + 4.2));
                // ����ɫֵ��һ����0-1��Χ
                color = (color + 1.0) * 0.5;
                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}