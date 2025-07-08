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

        CBUFFER_START(UnityPerMaterial)
            float4 _BlitTexture_ST;
            float4 _BlitTexture_TexelSize;
            int _KernelSize;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            inline float4 SampleQuadrant(float2 uv, float2 res, int x1, int x2, int y1, int y2, float n)
            {
                float luminance_sum = 0.0;
                float luminance_sum2 = 0.0;
                float3 col_sum = 0.0;

                UNITY_LOOP for (int x = x1; x <= x2; x++)
                {
                    UNITY_LOOP for (int y = y1; y <= y2; y++)
                    {
                        float3 sample = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + float2(x, y) * res).rgb;
                        float lum = dot(sample, float3(0.299, 0.587, 0.114));
                        luminance_sum += lum;
                        luminance_sum2 += (lum * lum);
                        col_sum += saturate(sample);
                    }
                }

                float mean = (luminance_sum / n);
                float std = abs(luminance_sum2 / n - mean * mean);

                return float4(col_sum / n, std);
            }

            float4 frag(Varyings input) : SV_TARGET
            {
                float2 uv = input.texcoord;
                float2 texel_size = _BlitTexture_TexelSize.xy;
                int kernel_size = _KernelSize;
                float window_size = (kernel_size * 2.0 + 1.0);
                int quadrant_size = (int)ceil(window_size * 0.5);
                int num_samples = (quadrant_size * quadrant_size);                

                float4 q1 = SampleQuadrant(uv, texel_size, -kernel_size, 0, -kernel_size, 0, num_samples);
                float4 q2 = SampleQuadrant(uv, texel_size,0, kernel_size, -kernel_size, 0, num_samples);
                float4 q3 = SampleQuadrant(uv, texel_size,0, kernel_size, 0, kernel_size, num_samples);
                float4 q4 = SampleQuadrant(uv, texel_size,-kernel_size, 0, 0, kernel_size, num_samples);

                float min_std = min(q1.a, min(q2.a, min(q3.a, q4.a)));
                int4 q = (float4(q1.a, q2.a, q3.a, q4.a) == min_std);

                float4 result = (dot(q, 1.0) > 1.0)
                             ? saturate(float4((q1.rgb + q2.rgb + q3.rgb + q4.rgb) * 0.25, 1.0))                                                    
                             : saturate(float4(q1.rgb * q.x + q2.rgb * q.y + q3.rgb * q.z + q4.rgb * q.w, 1.0));

                return result;
            }
            ENDHLSL
        }
    }
}