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

        SAMPLER(sampler_BlitTexture);

        CBUFFER_START(UnityPerMaterial)
            half4 _BlitTexture_ST;
            half4 _BlitTexture_TexelSize;
            half _Delta;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag
            
            half sobel(half2 uv)
            {
                half d = _Delta;

                half4 x = 0.0;
                half4 y = 0.0;

                half2 uv_add_d = (uv + (half2)d);
                half2 uv_sub_d = (uv - (half2)d);
                half2 uv_d_to_neg = (uv + half2(d, -d));
                half2 uv_neg_to_d = (uv + half2(-d, d));

                x += SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv_sub_d);
                x -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv_d_to_neg);
                x += SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, half2(uv_sub_d.x, uv.y)) * 2.0;
                x -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, half2(uv_add_d.x, uv.y)) * 2.0;
                x += SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv_neg_to_d);
                x -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv_add_d);

                y += SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv_sub_d);
                y += SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, half2(uv.x, uv_sub_d.y)) * 2.0;
                y += SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv_d_to_neg);
                y -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv_neg_to_d);
                y -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, half2(uv.x, uv_add_d.y)) * 2.0;
                y -= SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv_add_d);

                return sqrt(x * x + y * y);
            }

            half4 frag(Varyings input) : SV_Target
            {
                half2 uv = input.texcoord;
                half3 s = (1.0 - sobel(uv));
                return SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv) * half4(s, 1.0);
            }
            ENDHLSL
        }
    }
}