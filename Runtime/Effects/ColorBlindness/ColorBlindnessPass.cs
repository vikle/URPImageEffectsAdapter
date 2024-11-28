using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Color Blindness", fileName = "Color Blindness", order = 51)]
    public sealed class ColorBlindnessPass : ImageEffectPass<ColorBlindnessVolume>
    {
        static readonly int sr_severity = Shader.PropertyToID("_SeverityParams");
        static readonly string[] sr_blindTypesKeywords = new[] { "_PROTANOMALY", "_DEUTERANOMALY", "_TRITANOMALY" };
        const string k_DifferenceKeyword = "_DIFFERENCE";

        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/ColorBlindness");
        }
        
        protected override void OnPrepare(Material material, ColorBlindnessVolume volume, Queue<int> shaderPasses)
        {
            float severity_x10 = (volume.severity.value * 10f);
            int p1 = (int)Mathf.Min(10f, Mathf.Floor(severity_x10));
            int p2 = (int)Mathf.Min(10f, Mathf.Floor(severity_x10 + 1f));
            float weight = Fraction(severity_x10);
            
            material.SetVector(sr_severity, new Vector4(p1, p2, weight, 0f));

            CoreUtils.SetKeyword(material, k_DifferenceKeyword, volume.difference.value);

            int active_blind_type_index = (int)volume.blindType.value;
            string[] blind_keywords = sr_blindTypesKeywords;

            for (int i = 0, i_max = blind_keywords.Length; i < i_max; i++)
            {
                CoreUtils.SetKeyword(material, blind_keywords[i], active_blind_type_index == i);
            }
        }
        
        private static float Fraction(float value)
        {
            return (value - Mathf.Floor(value));
        }
    };
}
