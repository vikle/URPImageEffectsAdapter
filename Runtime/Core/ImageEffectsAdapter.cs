using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public sealed class ImageEffectsAdapter : ScriptableRendererFeature
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Shader blitShader;

        public ImageEffectPass[] passes = Array.Empty<ImageEffectPass>();
        
        ImageEffectsAdapterPass m_pass;

        public override void Create()
        {
            if (passes.Any(p => p == null))
            {
                Debug.LogError("Any image effect pass is null");
                Dispose(true);
                return;
            }
            
            InitShaders();
            m_pass = new ImageEffectsAdapterPass(this);
        }
        
        private void InitShaders()
        {
            if (blitShader == null)
            {
                blitShader = Shader.Find("Hidden/ImageEffectsAdapter/Blit");
            }
            
            ImageEffectPass.InitCommandBuffer();
            ImageEffectPass.CreateBlitMaterialIfNeeded(blitShader);

            for (int i = 0, i_max = passes.Length; i < i_max; i++)
            {
                passes[i].Initialize();
            }
        }
        
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (m_pass != null)
            {
                renderer.EnqueuePass(m_pass);
            }
        }

        protected override void Dispose(bool disposing)
        {
            m_pass?.Release();
            m_pass = null;
        }
    };
}


