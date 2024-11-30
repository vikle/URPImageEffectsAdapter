using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Color Correction", typeof(UniversalRenderPipeline))]
    public sealed class ColorCorrectionVolume : ImageEffectVolume
    {
        public BoolParameter enabled = new BoolParameter(false);
        
        public Vector3Parameter exposure = new Vector3Parameter(Vector3.one);
        public Vector3Parameter brightness = new Vector3Parameter(Vector3.zero);
        public Vector3Parameter linearMidPoint = new Vector3Parameter(Vector3.one * 0.5f);
        public Vector3Parameter contrast = new Vector3Parameter(Vector3.one);
        public Vector3Parameter saturation = new Vector3Parameter(Vector3.one);
        public ClampedFloatParameter temperature = new ClampedFloatParameter(0f, -1f, 1f);
        public ClampedFloatParameter tint = new ClampedFloatParameter(0f, -1f, 1f);
        public ColorParameter colorFilter = new ColorParameter(Color.white, true, false, false);

        public override bool IsActive()
        {
            return enabled.value;
        }
    };
}
