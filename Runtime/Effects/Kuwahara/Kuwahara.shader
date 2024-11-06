Shader "Hidden/ImageEffectsAdapter/Effects/Kuwahara"
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

        SAMPLER(sampler_BlitTexture);

        CBUFFER_START(UnityPerMaterial)
            half4 _BlitTexture_ST;
            half4 _BlitTexture_TexelSize;
            int _KernelSize;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag
            
            static half luminance(half3 color)
            {
                return dot(color, half3(0.299f, 0.587f, 0.114f));
            }

            inline half4 SampleQuadrant(half2 uv, half2 res, int x1, int x2, int y1, int y2, half n)
            {
                half luminance_sum = 0.0;
                half luminance_sum2 = 0.0;
                half3 col_sum = 0.0;

                [loop]
                for (int x = x1; x <= x2; x++)
                {
                    [loop]
                    for (int y = y1; y <= y2; y++)
                    {
                        half3 sample = SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv + half2(x, y) * res).rgb;
                        half l = luminance(sample);
                        luminance_sum += l;
                        luminance_sum2 += l * l;
                        col_sum += saturate(sample);
                    }
                }

                half mean = luminance_sum / n;
                half std = abs(luminance_sum2 / n - mean * mean);

                return half4(col_sum / n, std);
            }

            half4 frag(Varyings input) : SV_TARGET
            {
                half2 uv = input.texcoord;
                half2 texel_size = _BlitTexture_TexelSize.xy;
                int kernel_size = _KernelSize;
                half window_size = 2.0 * kernel_size + 1.0;
                int quadrant_size = int(ceil(window_size / 2.0));
                int num_samples = quadrant_size * quadrant_size;                

                half4 q1 = SampleQuadrant(uv, texel_size, -kernel_size, 0, -kernel_size, 0, num_samples);
                half4 q2 = SampleQuadrant(uv, texel_size,0, kernel_size, -kernel_size, 0, num_samples);
                half4 q3 = SampleQuadrant(uv, texel_size,0, kernel_size, 0, kernel_size, num_samples);
                half4 q4 = SampleQuadrant(uv, texel_size,-kernel_size, 0, 0, kernel_size, num_samples);

                half min_std = min(q1.a, min(q2.a, min(q3.a, q4.a)));
                int4 q = (half4(q1.a, q2.a, q3.a, q4.a) == min_std);

                half4 result = (dot(q, 1.0) > 1.0)
                             ? saturate(half4((q1.rgb + q2.rgb + q3.rgb + q4.rgb) / 4.0, 1.0))                                                    
                             : saturate(half4(q1.rgb * q.x + q2.rgb * q.y + q3.rgb * q.z + q4.rgb * q.w, 1.0));

                return result;
            }
            ENDHLSL
        }
    }
}