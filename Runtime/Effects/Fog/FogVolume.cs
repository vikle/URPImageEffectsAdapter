﻿using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Fog", typeof(UniversalRenderPipeline))]
    public sealed class FogVolume : ImageEffectVolume
    {
        public ClampedFloatParameter density = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter offset = new ClampedFloatParameter(0f, 0f, 100f);
        public ColorParameter color = new ColorParameter(Color.gray);
        
        public override bool IsActive()
        {
            return (MathF.Abs(density.value) > 0.01f);
        }
    };
}