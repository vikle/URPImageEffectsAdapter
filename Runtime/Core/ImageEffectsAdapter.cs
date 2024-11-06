using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public sealed class ImageEffectsAdapter : ScriptableRendererFeature
    {
        public Shader blitShader;
        public ShadersList shaders;
        
        ImageEffectsAdapterPass m_pass;

        public override void Create()
        {
            InitShaders();
            m_pass = new ImageEffectsAdapterPass(this);
        }

        private void InitShaders()
        {
            if (blitShader == null)
            {
                blitShader = Shader.Find("Hidden/ImageEffectsAdapter/Blit");
            }
            
            if (shaders.kuwahara == null)
            {
                shaders.kuwahara = Shader.Find("Hidden/ImageEffectsAdapter/Effects/Kuwahara");
            }

            if (shaders.sobelFilter == null)
            {
                shaders.sobelFilter = Shader.Find("Hidden/ImageEffectsAdapter/Effects/SobelFilter");
            }
            
            if (shaders.sharpness == null)
            {
                shaders.sharpness = Shader.Find("Hidden/ImageEffectsAdapter/Effects/Sharpness");
            }

            if (shaders.blur == null)
            {
                shaders.blur = Shader.Find("Hidden/ImageEffectsAdapter/Effects/Blur");
            }
        }
        
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(m_pass);
        }

        protected override void Dispose(bool disposing)
        {
            m_pass.Release();
        }
    };
}


