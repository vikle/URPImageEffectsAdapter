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
            half4 _BlitTexture_ST;
            half4 _BlitTexture_TexelSize;
            int _KernelSize;
            half _Sigma;
        CBUFFER_END

        half gaussian(half pos)
        {
            half s = _Sigma;
            return (half(1.0) / sqrt(half(2.0) * half(PI) * s * s)) * exp(-(pos * pos) / (half(2.0) * s * s));
        }
        ENDHLSL

        // Box Blur Horizontal [0]
        Pass
        {
            HLSLPROGRAM
            #pragma fragment frag

            half4 frag(Varyings input) : SV_TARGET
            {
                half2 uv = input.texcoord;
                half sx = _BlitTexture_TexelSize.x;              
                half4 output = 0.0;
                half samples = 0.0;

                UNITY_LOOP for (half x = -_KernelSize; x <= _KernelSize; x++)
                {
                    output += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + half2(x * sx, 0.0));
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

            half4 frag(Varyings input) : SV_TARGET
            {                
                half2 uv = input.texcoord;
                half sy = _BlitTexture_TexelSize.y;                
                half4 output = 0.0;
                half samples = 0.0;

                UNITY_LOOP for (half y = -_KernelSize; y <= _KernelSize; y++)
                {
                    output += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + half2(0.0, y * sy));
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

            half4 frag(Varyings input) : SV_Target
            {
                half2 uv = input.texcoord;
                half sx = _BlitTexture_TexelSize.x;              
                half4 output = 0.0;
                half samples = 0.0;

                UNITY_LOOP for (half x = -_KernelSize; x <= _KernelSize; x++)
                {
                    half gauss = gaussian(x);
                    output += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + half2(x * sx, 0.0));
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

            half4 frag(Varyings input) : SV_Target
            {
                half2 uv = input.texcoord;
                half sy = _BlitTexture_TexelSize.y;                
                half4 output = 0.0;
                half samples = 0.0;

                UNITY_LOOP for (half y = -_KernelSize; y <= _KernelSize; y++)
                {
                    half gauss = gaussian(y);
                    output += gauss * SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv + half2(0.0, y * sy));
                    samples += gauss;
                }

                return output / samples;
            }
            ENDHLSL
        }
    }
}