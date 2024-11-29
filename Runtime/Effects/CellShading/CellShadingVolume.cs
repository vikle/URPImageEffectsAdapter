using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Cell Shading", typeof(UniversalRenderPipeline))]
    public sealed class CellShadingVolume : ImageEffectVolume
    {
        public ClampedIntParameter shades = new ClampedIntParameter(0, 0, 32);
        public ClampedFloatParameter strength = new ClampedFloatParameter(0.7f, 0f, 1f);
        
        public override bool IsActive()
        {
            return (shades.value > 0);
        }
    };
}
