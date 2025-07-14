using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public sealed class ImageEffectsAdapterPass : ScriptableRenderPass
    {
        readonly ImageEffectPass[] m_passes;

        readonly CommandBuffer m_profilingScopeBuffer;
        readonly ProfilingSampler m_profilingSampler;
        
        public ImageEffectsAdapterPass(ImageEffectsAdapter renderer)
        {
            string type_name = GetType().Name;
            
            renderPassEvent = renderer.renderPassEvent;
            m_passes = renderer.passes;
            m_profilingSampler = new ProfilingSampler(type_name);
            m_profilingScopeBuffer = new CommandBuffer() { name = type_name };
            
            ImageEffectPass.CreateCommandBuffer(type_name);
        }
    
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ref var camera_data = ref renderingData.cameraData;
        
            if (!CameraTool.IsRealCamera(camera_data.cameraType))
            {
                return;
            }
        
            ImageEffectPass.AssignVolumeStack();
            ImageEffectPass.SetupCameraBuffers(ref camera_data);

            var passes = m_passes;
            
            for (int i = 0, i_max = passes.Length; i < i_max; i++)
            {
                passes[i].Setup();
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            ref var camera_data = ref renderingData.cameraData;

            if (!CameraTool.IsRealCamera(camera_data.cameraType))
            {
                return;
            }
            
            ImageEffectPass.AssignContext(ref context);

            var passes = m_passes;
            
            using (new ProfilingScope(m_profilingScopeBuffer, m_profilingSampler))
            {
                for (int i = 0, i_max = passes.Length; i < i_max; i++)
                {
                    passes[i].Render();
                }
                
                ImageEffectPass.RenderFinalBlit();
            }
        }
    
        public override void OnCameraCleanup(CommandBuffer cmd) { }
    
        public void Release()
        {
            ImageEffectPass.ReleaseCameraBuffers();
            
            var passes = m_passes;
            
            for (int i = 0, i_max = passes.Length; i < i_max; i++)
            {
                passes[i].Release();
            }
        }
    };
}
