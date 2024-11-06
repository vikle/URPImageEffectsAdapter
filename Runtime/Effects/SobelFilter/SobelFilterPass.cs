using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    public sealed class SobelFilterPass : ImageEffectPass<SobelFilterVolume>
    {
        public SobelFilterPass(Shader shader) : base(shader) { }
        
        static readonly int sr_delta = Shader.PropertyToID("_Delta");

        protected override void OnRender()
        {
            var mat = m_material;
            var vol = m_volume;

            mat.SetFloat(sr_delta, vol.delta.value);

            Blitter.BlitCameraTexture(sr_cmd, s_cameraColorTarget, s_temporaryBuffer, mat, 0);
            BlitCameraTexture(s_temporaryBuffer, s_cameraColorTarget);

            ExecuteCommandBuffer();
        }
    };
}
