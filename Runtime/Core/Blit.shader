Shader "Hidden/ImageEffectsAdapter/Blit"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZTest Always ZWrite Off Cull Off
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FragBilinear
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"   
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            ENDHLSL
        }
    }
}