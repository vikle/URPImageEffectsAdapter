using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public sealed class ImageEffectsAdapter : ScriptableRendererFeature
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Shader blitShader;
        public ImageEffectPass[] passes = Array.Empty<ImageEffectPass>();
        
        ImageEffectsAdapterPass m_mainPass;

        public override void Create()
        {
            for (int i = 0, i_max = passes.Length; i < i_max; i++)
            {
                if (passes[i] != null) continue;
                Debug.Log("NOTE: If any pass is null, ImageEffectsAdapter feature has been disabled.");
                return;
            }
            
            InitFinalBlitPass();
            InitPasses();
            
            m_mainPass = new ImageEffectsAdapterPass(this);
        }
        
        private void InitFinalBlitPass()
        {
            if (blitShader == null)
            {
                blitShader = Shader.Find("Hidden/ImageEffectsAdapter/Blit");
            }
            
            ImageEffectPass.InitBlitMaterial(blitShader);
        }

        private void InitPasses()
        {
            for (int i = 0, i_max = passes.Length; i < i_max; i++)
            {
                passes[i].Initialize();
            }
        }
        
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (m_mainPass != null)
            {
                renderer.EnqueuePass(m_mainPass);
            }
        }

        protected override void Dispose(bool disposing)
        {
            m_mainPass?.Release();
            m_mainPass = null;
        }
    };
}


