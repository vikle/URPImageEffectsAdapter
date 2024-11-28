using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
            material.SetFloat(sr_density, volume.density.value);
            material.SetFloat(sr_offset, volume.offset.value);
            material.SetColor(sr_color, volume.color.value);
        }
    };
}
