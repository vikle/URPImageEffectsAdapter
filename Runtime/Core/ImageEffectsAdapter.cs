using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public sealed class ImageEffectsAdapter : ScriptableRendererFeature
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Shader blitShader;

        public List<ImageEffectPass> passes = new List<ImageEffectPass>(8);
        
        ImageEffectsAdapterPass m_pass;

        public override void Create()
        {
            passes.RemoveAll(pass => pass == null);
            
            InitShaders();
            InitPasses();
            
            m_pass = new ImageEffectsAdapterPass(this);
        }
        
        private void InitShaders()
        {
            if (blitShader == null)
            {
                blitShader = Shader.Find("Hidden/ImageEffectsAdapter/Blit");
            }
        }

        private void InitPasses()
        {
            ImageEffectPass.InitCommandBuffer();
            ImageEffectPass.CreateBlitMaterialIfNeeded(blitShader);

            for (int i = 0, i_max = passes.Count; i < i_max; i++)
            {
                passes[i].Initialize();
            }
        }
        
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_pass);
        }

        protected override void Dispose(bool disposing)
        {
            m_pass?.Release();
            m_pass = null;
        }
    };
}


