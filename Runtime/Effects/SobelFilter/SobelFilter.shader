Shader "Hidden/ImageEffectsAdapter/Effects/SobelFilter"
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
            float _Delta;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag
            
            float sobel(float2 uv)
            {
                float d = _Delta;
                float two_val = 2.0;

                float4 x = 0.0;
                float4 y = 0.0;

                float2 uv_add_d = (uv + float2(d, d));
                float2 uv_sub_d = (uv - float2(d, d));
                float2 uv_d_to_neg = (uv + float2(d, -d));
                float2 uv_neg_to_d = (uv + float2(-d, d));

                x += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv_sub_d);
                x -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv_d_to_neg);
                x += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, float2(uv_sub_d.x, uv.y)) * two_val;
                x -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, float2(uv_add_d.x, uv.y)) * two_val;
                x += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv_neg_to_d);
                x -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv_add_d);

                y += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv_sub_d);
                y += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, float2(uv.x, uv_sub_d.y)) * two_val;
                y += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv_d_to_neg);
                y -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv_neg_to_d);
                y -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, float2(uv.x, uv_add_d.y)) * two_val;
                y -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv_add_d);

                return sqrt(x * x + y * y);
            }

            float4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                float3 s = (1.0 - sobel(uv));
                return SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv) * float4(s, 1.0);
            }
            ENDHLSL
        }
    }
}