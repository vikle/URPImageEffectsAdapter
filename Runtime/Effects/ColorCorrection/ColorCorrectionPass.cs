using System.Collections.Generic;
using UnityEngine;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Color Correction", fileName = "Color Correction", order = 51)]
    public sealed class ColorCorrectionPass : ImageEffectPass<ColorCorrectionVolume>
    {
        static readonly int sr_exposure = Shader.PropertyToID("_Exposure");
        static readonly int sr_brightness = Shader.PropertyToID("_Brightness");
        static readonly int sr_linearMidPoint = Shader.PropertyToID("_MidPoint");
        static readonly int sr_contrast = Shader.PropertyToID("_Contrast");
        static readonly int sr_saturation = Shader.PropertyToID("_Saturation");
        static readonly int sr_lmsBalance = Shader.PropertyToID("_LMSBalance");
        static readonly int sr_colorFilter = Shader.PropertyToID("_ColorFilter");
        static readonly int sr_dark = Shader.PropertyToID("_Dark");

        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/ColorCorrection");
        }
        
        protected override void OnPrepare(Material material, ColorCorrectionVolume volume, Queue<int> shaderPasses)
        {
            float t1 = (volume.temperature.value / 65f);
            float t2 = (volume.tint.value / 65f);

            float x = (0.31271f - t1 * (t1 < 0f ? 0.1f : 0.05f));
            float y = ((2.87f * x - 3f * x * x - 0.27509507f) + t2 * 0.05f);
            
            float X = (x / y);
            float Z = (1f - x - y) / y;
            float L = (+0.7328f * X + 0.4296f - 0.1624f * Z);
            float M = (-0.7036f * X + 1.6975f + 0.0061f * Z);
            float S = (+0.0030f * X + 0.0136f + 0.9834f * Z);
            
            material.SetVector(sr_lmsBalance, new Vector4(0.949237f / L, 1.03542f / M, 1.08728f / S, 1f));
            
            material.SendVolumeParameter(sr_exposure, volume.exposure);
            material.SendVolumeParameter(sr_brightness, volume.brightness);
            material.SendVolumeParameter(sr_linearMidPoint, volume.linearMidPoint);
            material.SendVolumeParameter(sr_contrast, volume.contrast);
            material.SendVolumeParameter(sr_saturation, volume.saturation);
            material.SendVolumeParameter(sr_colorFilter, volume.colorFilter);
            material.SendVolumeParameter(sr_dark, volume.dark);
        }
    };
}
