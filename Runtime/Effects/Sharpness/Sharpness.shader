Shader "Hidden/ImageEffectsAdapter/Effects/Sharpness"
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

        SAMPLER(sampler_BlitTexture);

        CBUFFER_START(UnityPerMaterial)
            half4 _BlitTexture_ST;
            half4 _BlitTexture_TexelSize;
            half _Amount;
        CBUFFER_END
        ENDHLSL
        
        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            half4 frag(Varyings input) : SV_TARGET
            {
                half2 uv = input.texcoord;
                half sx = _BlitTexture_TexelSize.x;
                half sy = _BlitTexture_TexelSize.y;
                
                half4 n = SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv + half2(0.0, sy));
                half4 e = SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv + half2(sx, 0.0));
                half4 s = SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv - half2(0.0, sy));
                half4 w = SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv - half2(sx, 0.0));

                half4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_BlitTexture, uv);

                half amount = _Amount;
                half neighbor = -amount;
                half center = (amount * half(4.0) + half(1.0));

                color = saturate(n * neighbor + e * neighbor + color * center + s * neighbor + w * neighbor);
                
                return color;
            }

            ENDHLSL
        }
    }
}