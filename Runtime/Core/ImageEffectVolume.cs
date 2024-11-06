using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public abstract class ImageEffectVolume : VolumeComponent, IPostProcessComponent
    {
        protected ImageEffectVolume()
        {
            displayName = GetType().Name.Replace("Volume", string.Empty);
        }
        
        public abstract bool IsActive();
        
        public bool IsTileCompatible()
        {
            return false;
        }
    };
}
