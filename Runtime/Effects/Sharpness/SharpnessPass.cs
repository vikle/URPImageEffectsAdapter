using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Sharpness", fileName = "Sharpness", order = 51)]
    public sealed class SharpnessPass : ImageEffectPass<SharpnessVolume>
    {
        static readonly int sr_amount = Shader.PropertyToID("_Amount");

        protected override void OnInitializeShader()
        {
            shader = Shader.Find("Hidden/ImageEffectsAdapter/Effects/Sharpness");
        }

        protected override void OnRender()
        {
            var mat = m_material;
            var vol = m_volume;
            
            mat.SetFloat(sr_amount, vol.amount.value);
            
            Blitter.BlitCameraTexture(sr_cmd, s_cameraColorTarget, s_temporaryBuffer, mat, 0);
            BlitCameraTexture(s_temporaryBuffer, s_cameraColorTarget);
            
            ExecuteCommandBuffer();
        }
    };
}
