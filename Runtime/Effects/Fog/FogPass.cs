using System.Collections.Generic;
using UnityEngine;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Fog", fileName = "Fog", order = 51)]
    public sealed class FogPass : ImageEffectPass<FogVolume>
    {
        static readonly int sr_params = Shader.PropertyToID("_FogParams");
        static readonly int sr_color = Shader.PropertyToID("_FogColor");

        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/Fog");
        }
        
        protected override void OnPrepare(Material material, FogVolume volume, Queue<int> shaderPasses)
        {
            float density = (volume.density.value / 0.8325546f);
            
            material.SetVector(sr_params, new Vector4(density, volume.offset.value, volume.fade.value, 0f));
            material.SendVolumeParameter(sr_color, volume.color);
        }
    };
}
