using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Blur", typeof(UniversalRenderPipeline))]
    public sealed class BlurVolume : ImageEffectVolume
    {
        public VolumeParameter<EBlurMode> mode = new VolumeParameter<EBlurMode>()
        {
            value = EBlurMode.Gaussian,
            overrideState = true
        };
        
        public ClampedIntParameter kernelSize = new ClampedIntParameter(0, 0, 20);
        public ClampedFloatParameter sigma = new ClampedFloatParameter(5f, 0, 10f);

        public override bool IsActive()
        {
            bool is_active = (kernelSize.value > 0); 
            
            if (is_active && mode.value == EBlurMode.Gaussian)
            {
                is_active = (sigma.value > 0.01f);
            }
            
            return is_active;
        } 
    };
}
