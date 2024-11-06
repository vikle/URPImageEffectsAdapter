using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Sharpness", typeof(UniversalRenderPipeline))]
    public sealed class SharpnessVolume : ImageEffectVolume
    {
        public ClampedFloatParameter amount = new ClampedFloatParameter(0f, -10f, 10f);
        
        public override bool IsActive()
        {
            return (MathF.Abs(amount.value) > 0.01f);
        }
    };
}
