using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Blur", fileName = "Blur", order = 51)]
    public sealed class BlurPass : ImageEffectPass<BlurVolume>
    {
        static readonly int sr_kernelSize = Shader.PropertyToID("_KernelSize");
        static readonly int sr_sigma = Shader.PropertyToID("_Sigma");

        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/Blur");
        }
        
        protected override void OnPrepare(Material material, BlurVolume volume, Queue<int> shaderPasses)
        {
            material.SetInt(sr_kernelSize, volume.kernelSize.value);
            material.SetFloat(sr_sigma, volume.sigma.value);
            
            int pass = ((int)volume.mode.value * 2);
        
            shaderPasses.Enqueue(pass);
            shaderPasses.Enqueue(pass + 1);
        }
    };
}
