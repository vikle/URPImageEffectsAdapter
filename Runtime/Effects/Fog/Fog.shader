Shader "Hidden/ImageEffectsAdapter/Effects/Fog"
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
            half4 _BlitTexture_ST;
            half4 _BlitTexture_TexelSize;
            half _Density;
            half _Offset;
            half4 _Color;
        CBUFFER_END

        TEXTURE2D_X(_CameraDepthTexture);  
        ENDHLSL

        Pass
        {
            HLSLPROGRAM 
            #pragma fragment frag

            half4 frag(Varyings input) : SV_TARGET
            {
                half2 uv = input.texcoord;

                half4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);
                
                half depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, uv);
                depth = Linear01Depth(depth, _ZBufferParams);

                half view_distance = (depth * _ProjectionParams.z);

                half factor = (_Density / sqrt(log(half(2.0)))) * max(half(0.0), view_distance - _Offset);
                factor = exp2(-factor * factor);

                color = lerp(_Color, color, saturate(factor));

                return color;                
            }

            ENDHLSL
        }
    }
}