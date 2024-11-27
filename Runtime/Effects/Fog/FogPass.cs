using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Fog", fileName = "Fog", order = 51)]
    public sealed class FogPass : ImageEffectPass<FogVolume>
    {
        static readonly int sr_density = Shader.PropertyToID("_Density");
        static readonly int sr_offset = Shader.PropertyToID("_Offset");
        static readonly int sr_color = Shader.PropertyToID("_Color");
        
        protected override void OnInitializeShader()
        {
            shader = Shader.Find("Hidden/ImageEffectsAdapter/Effects/Fog");
        }
        
        protected override void OnRender()
        {
            var mat = m_material;
            var vol = m_volume;
            
            mat.SetFloat(sr_density, vol.density.value);
            mat.SetFloat(sr_offset, vol.offset.value);
            mat.SetColor(sr_color, vol.color.value);
        
            Blitter.BlitCameraTexture(sr_cmd, s_cameraColorTarget, s_temporaryBuffer, mat, 0);
            BlitCameraTexture(s_temporaryBuffer, s_cameraColorTarget);
            
            ExecuteCommandBuffer();
        }
    };
}
