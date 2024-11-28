using System.Collections.Generic;
using UnityEngine;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Fog", fileName = "Fog", order = 51)]
    public sealed class FogPass : ImageEffectPass<FogVolume>
    {
        static readonly int sr_density = Shader.PropertyToID("_Density");
        static readonly int sr_offset = Shader.PropertyToID("_Offset");
        static readonly int sr_color = Shader.PropertyToID("_Color");
        
        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/Fog");
        }
        
        protected override void OnPrepare(Material material, FogVolume volume, Queue<int> shaderPasses)
        {
            material.SendVolumeParameter(sr_density, volume.density);
            material.SendVolumeParameter(sr_offset, volume.offset);
            material.SendVolumeParameter(sr_color, volume.color);
        }
    };
}
