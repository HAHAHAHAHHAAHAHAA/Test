Shader "Lpk/LightModel/RoundToon"
{
    Properties
    {
        _BaseMap            ("Texture", 2D)                       = "white" {}
        _BaseColor          ("Color", Color)                      = (0.5,0.5,0.5,1)
        
        [Space]
        _ShadowStep         ("ShadowStep", Range(0, 1))           = 0.5
        _ShadowStrength     ("Shadow Strength", Range(0, 1))      = 0.5
        _LightDirection     ("Light Direction", Vector)           = (0.5, 0.5, 0.5, 0)
        
        [Space]   
        _OutlineWidth       ("OutlineWidth", Range(0.0, 10.0))    = 0.15
        _OutlineColor       ("OutlineColor", Color)               = (0.0, 0.0, 0.0, 1)
    }
    SubShader
    {
        Tags { 
            "RenderType" = "Opaque"
            "Queue" = "Geometry+10" 
        }
        
        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half _ShadowStep;
                half _ShadowStrength;
                half3 _LightDirection;
            CBUFFER_END

            struct Attributes
            {     
                float4 positionOS   : POSITION;
                half3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
            }; 

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                half3 normalWS      : TEXCOORD1;
                float4 positionCS   : SV_POSITION;
                float fogCoord      : TEXCOORD2;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);

                output.positionCS = vertexInput.positionCS;
                output.uv = input.uv;
                output.normalWS = normalInput.normalWS;
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // Текстура и нормали
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                half3 N = normalize(input.normalWS);
                half3 L = normalize(_LightDirection);
                
                // Имитация тени
                half NL = dot(N, L) * 0.5 + 0.5;
                half shadow = step(_ShadowStep, NL);
                shadow = lerp(1.0, shadow, _ShadowStrength);
                
                // Итоговый цвет
                half3 color = _BaseColor.rgb * baseMap.rgb * shadow;
                color = MixFog(color, input.fogCoord);
                
                return half4(color, 1.0);
            }
            ENDHLSL
        }

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "UniversalForward" }
            Cull Front
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                float _OutlineWidth;
                float4 _OutlineColor;
            CBUFFER_END

            struct Attributes
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
            };
            
            Varyings vert(Attributes v)
            {
                Varyings o;
                
                float3 posWS = TransformObjectToWorld(v.vertex.xyz);
                float3 normalWS = TransformObjectToWorldNormal(v.normal);
                
                posWS += normalWS * _OutlineWidth * 0.1; 
                
                o.pos = TransformWorldToHClip(posWS);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return _OutlineColor; 
            }
            ENDHLSL
        }
    }
}