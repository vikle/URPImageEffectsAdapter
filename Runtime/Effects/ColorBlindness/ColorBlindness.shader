Shader "Hidden/ImageEffectsAdapter/Effects/ColorBlindness"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZTest Always ZWrite Off Cull Off
        
        HLSLINCLUDE
        #pragma vertex Vert

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"   
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BlitTexture_ST;
            float4 _BlitTexture_TexelSize;
            float4 _SeverityParams;
        CBUFFER_END

        static float LocalLuminance(float3 color)
        {
            return dot(color, float3(0.299, 0.587, 0.114));
        }
        ENDHLSL
        
        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag
            #pragma shader_feature_local _ _DIFFERENCE
            #pragma shader_feature_local _ _PROTANOMALY _DEUTERANOMALY _TRITANOMALY
            
            #ifdef _PROTANOMALY
            #include "Protanomaly.hlsl"
            #elif _DEUTERANOMALY
            #include "Deuteranomaly.hlsl"
            #elif _TRITANOMALY
            #include "Tritanomaly.hlsl"
            #endif                        
            
            float4 frag(Varyings input) : SV_TARGET
            {
                float2 uv = input.texcoord;
                float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                float4 severity = _SeverityParams;
                int p1 = (int)severity.x;
                int p2 = (int)severity.y;
                float weight = severity.z;

                float3x3 blindness;

                #ifdef _PROTANOMALY
                blindness = lerp(protanomalySeverities[p1], protanomalySeverities[p2], weight);
                #elif _DEUTERANOMALY
                blindness = lerp(deuteranomalySeverities[p1], deuteranomalySeverities[p2], weight);
                #elif _TRITANOMALY
                blindness = lerp(tritanomalySeverities[p1], tritanomalySeverities[p2], weight);
                #else
                blindness = 1.0;
                #endif            
                
                float3 cb = mul(blindness, color.rgb);               

                #ifdef _DIFFERENCE
                float3 difference = abs(color.rgb - cb);
                cb = lerp(LocalLuminance(color), float3(1.0, 0.0, 0.0), saturate(dot(difference, float(1.0))));
                #endif
                
                return float4(saturate(cb), 1.0);
            }

            ENDHLSL
        }
    }
}