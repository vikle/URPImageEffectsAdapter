using System.Collections.Generic;
using UnityEngine;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Color Correction", fileName = "Color Correction", order = 51)]
    public sealed class ColorCorrectionPass : ImageEffectPass<ColorCorrectionVolume>
    {
        static readonly int sr_exposure = Shader.PropertyToID("_Exposure");
        static readonly int sr_temperature = Shader.PropertyToID("_Temperature");
        static readonly int sr_tint = Shader.PropertyToID("_Tint");
        static readonly int sr_contrast = Shader.PropertyToID("_Contrast");
        static readonly int sr_linearMidPoint = Shader.PropertyToID("_MidPoint");
        static readonly int sr_brightness = Shader.PropertyToID("_Brightness");
        static readonly int sr_colorFilter = Shader.PropertyToID("_ColorFilter");
        static readonly int sr_saturation = Shader.PropertyToID("_Saturation");
        
        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/ColorCorrection");
        }
        
        protected override void OnPrepare(Material material, ColorCorrectionVolume volume, Queue<int> shaderPasses)
        {
            material.SendVolumeParameter(sr_exposure, volume.exposure);
            material.SendVolumeParameter(sr_temperature, volume.temperature);
            material.SendVolumeParameter(sr_tint, volume.tint);
            material.SendVolumeParameter(sr_contrast, volume.contrast);
            material.SendVolumeParameter(sr_linearMidPoint, volume.linearMidPoint);
            material.SendVolumeParameter(sr_brightness, volume.brightness);
            material.SendVolumeParameter(sr_colorFilter, volume.colorFilter);
            material.SendVolumeParameter(sr_saturation, volume.saturation);
        }
    };
}
