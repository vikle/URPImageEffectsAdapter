using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    public sealed class BlurPass : ImageEffectPass<BlurVolume>
    {
        public BlurPass(Shader shader) : base(shader) { }
        
        static readonly int sr_kernelSize = Shader.PropertyToID("_KernelSize");
        static readonly int sr_sigma = Shader.PropertyToID("_Sigma");

        protected override void OnRender()
        {
            var mat = m_material;
            var vol = m_volume;
            
            mat.SetInt(sr_kernelSize, vol.kernelSize.value);
            mat.SetFloat(sr_sigma, vol.sigma.value);
            
            int pass = ((int)vol.mode.value * 2);
        
            Blitter.BlitCameraTexture(sr_cmd, s_cameraColorTarget, s_temporaryBuffer, mat, pass);
            Blitter.BlitCameraTexture(sr_cmd, s_temporaryBuffer, s_cameraColorTarget, mat, ++pass);
            
            ExecuteCommandBuffer();
        }
    };
}
