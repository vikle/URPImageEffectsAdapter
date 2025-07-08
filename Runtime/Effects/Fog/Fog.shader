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
            float4 _BlitTexture_ST;
            float4 _BlitTexture_TexelSize;
            float4 _FogParams;
            float4 _FogColor;
        CBUFFER_END

        TEXTURE2D_X(_CameraDepthTexture);  
        ENDHLSL

        Pass
        {
            HLSLPROGRAM 
            #pragma fragment frag

            float4 frag(Varyings input) : SV_TARGET
            {
                float4 params = _FogParams;
                
                float2 uv = input.texcoord;
                float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);
                
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_LinearClamp, uv);
                depth = Linear01Depth(depth, _ZBufferParams);

                float view_distance = (depth * _ProjectionParams.z);
                float factor = (params.x * max(0.0, view_distance - params.y));
                factor = exp2(-factor * factor);
                factor = saturate(factor);
                
                color = lerp(lerp(color, _FogColor, params.z), color, factor);

                return color;                
            }

            ENDHLSL
        }
    }
}