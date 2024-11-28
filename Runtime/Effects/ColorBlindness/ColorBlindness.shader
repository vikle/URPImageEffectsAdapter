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
            half4 _BlitTexture_ST;
            half4 _BlitTexture_TexelSize;
            half4 _SeverityParams;
        CBUFFER_END

        static half LocalLuminance(half3 color)
        {
            return dot(color, half3(0.299, 0.587, 0.114));
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
            
            half4 frag(Varyings input) : SV_TARGET
            {
                half2 uv = input.texcoord;
                half4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                half4 severity = _SeverityParams;
                int p1 = (int)severity.x;
                int p2 = (int)severity.y;
                half weight = severity.z;

                half3x3 blindness;

                #ifdef _PROTANOMALY
                blindness = lerp(protanomalySeverities[p1], protanomalySeverities[p2], weight);
                #elif _DEUTERANOMALY
                blindness = lerp(deuteranomalySeverities[p1], deuteranomalySeverities[p2], weight);
                #elif _TRITANOMALY
                blindness = lerp(tritanomalySeverities[p1], tritanomalySeverities[p2], weight);
                #else
                blindness = 1.0;
                #endif            
                
                half3 cb = mul(blindness, color.rgb);               

                #ifdef _DIFFERENCE
                half3 difference = abs(color.rgb - cb);
                cb = lerp(LocalLuminance(color), half3(1.0, 0.0, 0.0), saturate(dot(difference, half(1.0))));
                #endif
                
                return half4(saturate(cb), 1.0);
            }

            ENDHLSL
        }
    }
}