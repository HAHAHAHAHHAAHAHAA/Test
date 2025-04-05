Shader "Custom/AnimatedWaterDistortion"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _MainTexColor ("Main Texture Color", Color) = (1, 1, 1, 1)
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.1
        _WaterColor ("Water Color", Color) = (0, 0, 1, 1)
        _DistortionSpeed ("Distortion Speed", Range(0, 10)) = 1.0
        _DistortionScale ("Distortion Scale", Range(0, 5)) = 1.0
        _DistortionDirection ("Distortion Direction", Vector) = (1, 0, 0, 0)
        _NoiseScale ("Noise Scale", Range(0.1, 10)) = 1.0
        _TimeScale ("Time Scale", Range(-5, 5)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _MainTexColor;
            float _DistortionStrength;
            fixed4 _WaterColor;
            float _DistortionSpeed;
            float _DistortionScale;
            float4 _DistortionDirection;
            float _NoiseScale;
            float _TimeScale;

            float PerlinNoise(float2 uv)
            {
                return tex2D(_MainTex, uv).r;
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv + _DistortionDirection.xy * _DistortionSpeed * _Time.y; 
                uv *= _DistortionScale;

                float noise = PerlinNoise(uv * _NoiseScale + _Time.y * _TimeScale);
                uv += noise * _DistortionStrength;

                fixed4 mainColor = tex2D(_MainTex, uv) * _MainTexColor;

                return mainColor * _WaterColor; 
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
