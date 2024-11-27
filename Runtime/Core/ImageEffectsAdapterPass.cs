using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public sealed class ImageEffectsAdapterPass : ScriptableRenderPass
    {
        readonly ImageEffectPass[] m_passes;

        public ImageEffectsAdapterPass(ImageEffectsAdapter renderer)
        {
            renderPassEvent = renderer.renderPassEvent;
            m_passes = renderer.passes.ToArray();
        }
    
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ref var camera_data = ref renderingData.cameraData;
        
            if (!IsRealCamera(camera_data.cameraType))
            {
                return;
            }
        
            ImageEffectPass.SetupVolumeStack();
            ImageEffectPass.SetupCameraBuffers(ref camera_data);
        
            for (int i = 0, i_max = m_passes.Length; i < i_max; i++)
            {
                m_passes[i].OnSetup();
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            ref var camera_data = ref renderingData.cameraData;

            if (!IsRealCamera(camera_data.cameraType))
            {
                return;
            }
            
            ImageEffectPass.SetupScriptableRenderContext(ref context);

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
            ImageEffectPass.ReleaseCameraBuffers();
            
            for (int i = 0, i_max = m_passes.Length; i < i_max; i++)
            {
                m_passes[i].Release();
            }
        }
    };
}
