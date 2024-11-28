using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Sharpness", fileName = "Sharpness", order = 51)]
    public sealed class SharpnessPass : ImageEffectPass<SharpnessVolume>
    {
        static readonly int sr_amount = Shader.PropertyToID("_Amount");

        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/Sharpness");
        }
        
        protected override void OnPrepare(Material material, SharpnessVolume volume, Queue<int> shaderPasses)
        {
            material.SetFloat(sr_amount, volume.amount.value);
        }
    };
}
