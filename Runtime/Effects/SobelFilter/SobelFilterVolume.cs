using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Sobel Filter", typeof(UniversalRenderPipeline))]
    public sealed class SobelFilterVolume : ImageEffectVolume
    {
        public ClampedFloatParameter delta = new ClampedFloatParameter(0f, 0f, 0.001f);
        
        public override bool IsActive()
        {
            return (delta.value > 0.0001f);
        }
    };
}
