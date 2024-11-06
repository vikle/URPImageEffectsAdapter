using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    using Effects;
    
    public sealed class ImageEffectsAdapterPass : ScriptableRenderPass
    {
        readonly ImageEffectPass[] m_passes;
        
        RTHandle m_temporaryBuffer;

        public ImageEffectsAdapterPass(ImageEffectsAdapter renderer)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

            ImageEffectPass.CreateBlitMaterialIfNeeded(renderer.blitShader);
            
            ref readonly var shaders = ref renderer.shaders;
            
            m_passes = new ImageEffectPass[]
            {
                new KuwaharaPass(shaders.kuwahara),
                new SobelFilterPass(shaders.sobelFilter),
                new SharpnessPass(shaders.sharpness),
                new BlurPass(shaders.blur)
            };
        }
    
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var camera_data = renderingData.cameraData;
        
            if (!IsRealCamera(camera_data.cameraType))
            {
                return;
            }
        
            var descriptor = camera_data.cameraTargetDescriptor;
        
            descriptor.depthBufferBits = 0;
        
            RenderingUtils.ReAllocateIfNeeded(ref m_temporaryBuffer, descriptor, FilterMode.Bilinear);
        
            var stack = VolumeManager.instance.stack;
            
            for (int i = 0, i_max = m_passes.Length; i < i_max; i++)
            {
                m_passes[i].Setup(stack);
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var camera_data = renderingData.cameraData;

            if (!IsRealCamera(camera_data.cameraType))
            {
                return;
            }

            var cam_target = camera_data.renderer.cameraColorTargetHandle;
            
            ImageEffectPass.SetupStatic(ref context, m_temporaryBuffer, cam_target);

            for (int i = 0, i_max = m_passes.Length; i < i_max; i++)
            {
                m_passes[i].Render();
            }
        }

        private static bool IsRealCamera(CameraType cameraType)
        {
            switch (cameraType)
            {
                case CameraType.SceneView: 
                case CameraType.Game:
                case CameraType.VR: 
                    return true;

                case CameraType.Reflection:
                case CameraType.Preview:
                default: return false;
            }
        }
    
        public override void OnCameraCleanup(CommandBuffer cmd) { }
    
        public void Release()
        {
            m_temporaryBuffer?.Release();
            
            for (int i = 0, i_max = m_passes.Length; i < i_max; i++)
            {
                m_passes[i].Release();
            }
        }
    };
}
