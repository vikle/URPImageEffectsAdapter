using System.Collections.Generic;
using UnityEngine;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Cell Shading", fileName = "Cell Shading", order = 51)]
    public sealed class CellShadingPass: ImageEffectPass<CellShadingVolume>
    {
        static readonly int sr_shades = Shader.PropertyToID("_Shades");
        
        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/CellShading");
        }
        
        protected override void OnPrepare(Material material, CellShadingVolume volume, Queue<int> shaderPasses)
        {
            material.SendVolumeParameter(sr_shades, volume.shades);
        }
    };
}
