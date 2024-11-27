using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter.Effects
{
    [CreateAssetMenu(menuName = "URP Image Effects Adapter/Kuwahara", fileName = "Kuwahara", order = 51)]
    public sealed class KuwaharaPass : ImageEffectPass<KuwaharaVolume>
    {
        static readonly int sr_kernelSize = Shader.PropertyToID("_KernelSize");

        protected override Shader OnInitializeShader()
        {
            return Shader.Find("Hidden/ImageEffectsAdapter/Effects/Kuwahara");
        }

        protected override void OnRender()
        {
            var mat = m_material;
            var vol = m_volume;
            
            mat.SetInt(sr_kernelSize, vol.kernelSize.value);
        
            Blitter.BlitCameraTexture(sr_cmd, s_cameraColorTarget, s_temporaryBuffer, mat, 0);
            BlitCameraTexture(s_temporaryBuffer, s_cameraColorTarget);
            
            ExecuteCommandBuffer();
        }
    };
}
