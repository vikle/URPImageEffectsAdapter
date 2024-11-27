using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Color Blindness", typeof(UniversalRenderPipeline))]
    public sealed class ColorBlindnessVolume : ImageEffectVolume
    {
        public enum EBlindType : byte
        {
            Protanomaly,
            Deuteranomaly,
            Tritanomaly
        };

        public VolumeParameter<EBlindType> blindType = new VolumeParameter<EBlindType>();
        public ClampedFloatParameter severity = new ClampedFloatParameter(0f, 0f, 1f);
        public BoolParameter difference = new BoolParameter(false);
        
        public override bool IsActive()
        {
            return (severity.value > 0.01f);
        }
    };
}
