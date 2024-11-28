using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Kuwahara", fileName = "Kuwahara", order = 51)]
    public sealed class KuwaharaPass : ImageEffectPass<KuwaharaVolume>
    {
        static readonly int sr_kernelSize = Shader.PropertyToID("_KernelSize");

        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/Kuwahara");
        }
        
        protected override void OnPrepare(Material material, KuwaharaVolume volume, Queue<int> shaderPasses)
        {
            material.SetFloat(sr_kernelSize, volume.kernelSize.value);
        }
    };
}
