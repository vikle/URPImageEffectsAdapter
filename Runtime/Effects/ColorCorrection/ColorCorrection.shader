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
            half _Temperature;
            half _Tint;
            half3 _Contrast;
            half3 _MidPoint;
            half3 _Brightness;
            half4 _ColorFilter;
            half3 _Saturation;

            static const half3x3 LIN_2_LMS_MAT = {
                3.90405e-1, 5.49941e-1, 8.92632e-3,
                7.08416e-2, 9.63172e-1, 1.35775e-3,
                2.31082e-2, 1.28021e-1, 9.36245e-1
            };

            static const half3x3 LMS_2_LIN_MAT = {
                +2.85847e+0, -1.62879e+0, -2.48910e-2,
                -2.10182e-1, +1.15820e+0, +3.24281e-4,
                -4.18120e-2, -1.18169e-1, +1.06867e+0
            };
        CBUFFER_END

        static half LocalLuminance(half3 color)
        {
            return dot(color, half3(0.299, 0.587, 0.114));
        }

        static half3 WhiteBalance(half3 color, half temp, half tint)
        {
            half t1 = (temp * half(10.0) / half(6.0));
            half t2 = (tint * half(10.0) / half(6.0));

            half x = (half(0.31271) - t1 * (t1 < half(0.0) ? half(0.1) : half(0.05)));
            half standard_illuminant_y = (half(2.87) * x - half(3.0) * x * x - half(0.27509507));
            half y = (standard_illuminant_y + t2 * half(0.05));

            half3 w1 = half3(0.949237, 1.03542, 1.08728);

            half Y = half(1.0);
            half X = (Y * x / y);
            half Z = (Y * (half(1.0) - x - y) / y);
            half L = (half(+0.7328) * X + half(0.4296) * Y - half(0.1624) * Z);
            half M = (half(-0.7036) * X + half(1.6975) * Y + half(0.0061) * Z);
            half S = (half(+0.0030) * X + half(0.0136) * Y + half(0.9834) * Z);
            half3 w2 = half3(L, M, S);

            half3 balance = half3(w1.x / w2.x, w1.y / w2.y, w1.z / w2.z);

            half3 lms = mul(LIN_2_LMS_MAT, color);
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
                color = max(k_zero, WhiteBalance(color, _Temperature, _Tint));
                color = max(k_zero, _Contrast * (color - _MidPoint) + _MidPoint + _Brightness);
                color = max(k_zero, color * _ColorFilter);
                color = max(k_zero, lerp(LocalLuminance(color), color, _Saturation));

                return half4(color, 1.0);
            }
            ENDHLSL
        }
    }
}