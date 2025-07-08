Shader "Hidden/ImageEffectsAdapter/Effects/CellShading"
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
            int _Shades;
            float _Strength;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            float4 frag(Varyings input) : SV_TARGET
            {
                float2 uv = input.texcoord;
                float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                float shades = _Shades;
                float brightness = ((color.r + color.g + color.g) / 3.0);
                float shade = floor(brightness * shades);
                float brighness_of_shade = (shade / shades);
                float factor = (brightness / brighness_of_shade);
                
                color.rgb = max(color.rgb * _Strength, color.rgb / factor);
                
                return color;
            }
            ENDHLSL
        }
    }
}