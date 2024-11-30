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
            half4 _BlitTexture_ST;
            half4 _BlitTexture_TexelSize;

            half3 _Exposure;
            half3 _LMSBalance;
            half3 _Contrast;
            half3 _MidPoint;
            half3 _Brightness;
            half4 _ColorFilter;
            half3 _Saturation;
        CBUFFER_END

        static half3 WhiteBalance(half3 color, half3 balance)
        {
            const half3x3 LIN_2_LMS_MAT = {
                3.90405e-1, 5.49941e-1, 8.92632e-3,
                7.08416e-2, 9.63172e-1, 1.35775e-3,
                2.31082e-2, 1.28021e-1, 9.36245e-1
            };

            half3 lms = mul(LIN_2_LMS_MAT, color);

            const half3x3 LMS_2_LIN_MAT = {
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

            half4 frag(Varyings input) : SV_TARGET
            {
                half2 uv = input.texcoord;
                half3 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv).rgb;

                const half k_zero = half(0.0);

                color = max(k_zero, color * _Exposure);
                color = max(k_zero, WhiteBalance(color, _LMSBalance));
                color = max(k_zero, _Contrast * (color - _MidPoint) + _MidPoint + _Brightness);
                color = max(k_zero, color * _ColorFilter);
                color = max(k_zero, lerp(Luminance(color), color, _Saturation));

                return half4(color, 1.0);
            }
            ENDHLSL
        }
    }
}