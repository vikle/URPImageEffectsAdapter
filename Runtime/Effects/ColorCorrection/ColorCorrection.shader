Shader "Hidden/ImageEffectsAdapter/Effects/ColorCorrection"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100
        ZTest Always ZWrite Off Cull Off

        HLSLINCLUDE
        #pragma vertex Vert

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BlitTexture_ST;
            float4 _BlitTexture_TexelSize;

            float3 _Exposure;
            float3 _LMSBalance;
            float3 _Contrast;
            float3 _MidPoint;
            float3 _Brightness;
            float4 _ColorFilter;
            float3 _Saturation;
            float _Dark;
        CBUFFER_END

        static float3 WhiteBalance(float3 color, float3 balance)
        {
            const float3x3 LIN_2_LMS_MAT = {
                3.90405e-1, 5.49941e-1, 8.92632e-3,
                7.08416e-2, 9.63172e-1, 1.35775e-3,
                2.31082e-2, 1.28021e-1, 9.36245e-1
            };

            float3 lms = mul(LIN_2_LMS_MAT, color);

            const float3x3 LMS_2_LIN_MAT = {
                +2.85847e+0, -1.62879e+0, -2.48910e-2,
                -2.10182e-1, +1.15820e+0, +3.24281e-4,
                -4.18120e-2, -1.18169e-1, +1.06867e+0
            };            

            return mul(LMS_2_LIN_MAT, lms * balance);
        }
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            float4 frag(Varyings input) : SV_TARGET
            {
                float2 uv = input.texcoord;
                float3 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).rgb;

                const float k_zero = 0.0;

                color = max(k_zero, color * _Exposure);
                color = max(k_zero, WhiteBalance(color, _LMSBalance));
                color = max(k_zero, _Contrast * (color - _MidPoint) + _MidPoint + _Brightness);
                color = max(k_zero, color * _ColorFilter);
                color = max(k_zero, lerp(Luminance(color), color, _Saturation));
                color = lerp(color, color * (color.r + color.g + color.b), _Dark);
                
                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
}