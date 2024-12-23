﻿using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Fog", typeof(UniversalRenderPipeline))]
    public sealed class FogVolume : ImageEffectVolume
    {
        public ClampedFloatParameter density = new ClampedFloatParameter(0f, 0f, 0.1f);
        public ClampedFloatParameter offset = new ClampedFloatParameter(7f, 0f, 100f);
        public ClampedFloatParameter fade = new ClampedFloatParameter(1f, 0f, 1f);
        public ColorParameter color = new ColorParameter(Color.gray, true, false, false);
        
        public override bool IsActive()
        {
            return (density.value > 0.00001f);
        }
    };
}
