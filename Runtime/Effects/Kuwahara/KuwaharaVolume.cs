using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter.Effects
{
    [VolumeComponentMenuForRenderPipeline("Image-effects/Kuwahara", typeof(UniversalRenderPipeline))]
    public sealed class KuwaharaVolume : ImageEffectVolume
    {
        public ClampedIntParameter kernelSize = new ClampedIntParameter(0, 0, 10);
        
        public override bool IsActive()
        {
            return (kernelSize.value > 0);
        }
    };
}
