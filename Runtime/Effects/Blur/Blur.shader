Shader "Hidden/ImageEffectsAdapter/Effects/Blur"
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
            int _KernelSize;
            float _Sigma;
        CBUFFER_END

        float gaussian(float pos)
        {
            float s = _Sigma;
            return (1.0 / sqrt(2.0 * PI * s * s)) * exp(-(pos * pos) / (2.0 * s * s));
        }
        ENDHLSL

        // Box Blur Horizontal [0]
        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            float4 frag(Varyings input) : SV_TARGET
            {
                float2 uv = input.texcoord;
                float sx = _BlitTexture_TexelSize.x;              
                float4 output = 0.0;
                float samples = 0.0;

                UNITY_LOOP for (float x = -_KernelSize; x <= _KernelSize; x++)
                {
                    output += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + float2(x * sx, 0.0));
                    samples++;
                }

                return output / samples;
            }
            ENDHLSL
        }

        // Box Blur Vertical [1]
        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            float4 frag(Varyings input) : SV_TARGET
            {                
                float2 uv = input.texcoord;
                float sy = _BlitTexture_TexelSize.y;                
                float4 output = 0.0;
                float samples = 0.0;

                UNITY_LOOP for (float y = -_KernelSize; y <= _KernelSize; y++)
                {
                    output += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + float2(0.0, y * sy));
                    samples++;
                }

                return output / samples;
            }
            ENDHLSL
        }

        // Gaussian Blur Horizontal [2]
        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            float4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                float sx = _BlitTexture_TexelSize.x;              
                float4 output = 0.0;
                float samples = 0.0;

                UNITY_LOOP for (float x = -_KernelSize; x <= _KernelSize; x++)
                {
                    float gauss = gaussian(x);
                    output += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + float2(x * sx, 0.0));
                    samples += gauss;
                }

                return output / samples;
            }
            ENDHLSL
        }

        // Gaussian Blur Vertical [3]
        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            float4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                float sy = _BlitTexture_TexelSize.y;                
                float4 output = 0.0;
                float samples = 0.0;

                UNITY_LOOP for (float y = -_KernelSize; y <= _KernelSize; y++)
                {
                    float gauss = gaussian(y);
                    output += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + float2(0.0, y * sy));
                    samples += gauss;
                }

                return output / samples;
            }
            ENDHLSL
        }
    }
}