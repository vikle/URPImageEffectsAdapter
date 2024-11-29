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
            half4 _BlitTexture_ST;
            half4 _BlitTexture_TexelSize;
            int _Shades;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            half4 frag(Varyings input) : SV_TARGET
            {
                half2 uv = input.texcoord;
                half4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);

                half shades = _Shades;
                half brightness = ((color.r + color.g + color.g) / half(3.0));
                half shade = floor(brightness * shades);
                half brighness_of_shade = (shade / shades);
                half factor = (brightness / brighness_of_shade);
                
                color.rgb /= factor;

                return color;
            }
            ENDHLSL
        }
    }
}