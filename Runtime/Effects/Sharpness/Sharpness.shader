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

        CBUFFER_START(UnityPerMaterial)
            float4 _BlitTexture_ST;
            float4 _BlitTexture_TexelSize;
            float _Amount;
        CBUFFER_END
        ENDHLSL
        
        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            float4 frag(Varyings input) : SV_TARGET
            {
                float2 uv = input.texcoord;
                float sx = _BlitTexture_TexelSize.x;
                float sy = _BlitTexture_TexelSize.y;
                
                float4 n = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + float2(0.0, sy));
                float4 e = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + float2(sx, 0.0));
                float4 s = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv - float2(0.0, sy));
                float4 w = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv - float2(sx, 0.0));

                float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                float amount = _Amount;
                float neighbor = -amount;
                float center = (amount * 4.0 + 1.0);

                color = saturate(n * neighbor + e * neighbor + color * center + s * neighbor + w * neighbor);
                
                return color;
            }

            ENDHLSL
        }
    }
}