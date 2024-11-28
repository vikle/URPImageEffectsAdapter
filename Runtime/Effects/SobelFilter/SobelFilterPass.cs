using System.Collections.Generic;
using UnityEngine;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Sobel Filter", fileName = "Sobel Filter", order = 51)]
    public sealed class SobelFilterPass : ImageEffectPass<SobelFilterVolume>
    {
        static readonly int sr_delta = Shader.PropertyToID("_Delta");

        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/SobelFilter");
        }

        protected override void OnPrepare(Material material, SobelFilterVolume volume, Queue<int> shaderPasses)
        {
            material.SendVolumeParameter(sr_delta, volume.delta);
        }
    };
}
