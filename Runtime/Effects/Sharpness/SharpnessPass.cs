using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    public sealed class SharpnessPass : ImageEffectPass<SharpnessVolume>
    {
        static readonly int sr_amount = Shader.PropertyToID("_Amount");
        
        public SharpnessPass(Shader shader) : base(shader) { }
        
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
