using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public abstract class ImageEffectPass : ScriptableObject
    {
        public Shader shader;

        protected abstract bool IsActive { get; }
        
        Material m_material;
        
        static ScriptableRenderContext s_context;
        static CommandBuffer s_cmd;
        
        static RTHandle s_tempColorTarget;
        static RTHandle s_cameraColorTarget;

        static RTHandle s_currentSource;
        static RTHandle s_currentDestination;
        
        protected static VolumeStack s_volumeStack;

        static readonly Queue<int> sr_shaderPasses = new Queue<int>();
        
        static Material s_blitMaterial;

        void OnValidate()
        {
            Initialize();
        }

        void OnEnable()
        {
            Initialize();
        }

        internal void Initialize()
        {
            if (shader == null)
            {
                shader = OnInitializeShader();
            }
            
            if (m_material == null)
            {
                m_material = CoreUtils.CreateEngineMaterial(shader);
            }
        }

        protected abstract Shader OnInitializeShader();
        
        internal static void InitBlitMaterial(Shader shader)
        {
            if (s_blitMaterial == null)
            {
                s_blitMaterial = CoreUtils.CreateEngineMaterial(shader);
            }
        }

        internal static void CreateCommandBuffer(string commandBufferName)
        {
            s_cmd?.Dispose();
            
            s_cmd = new CommandBuffer()
            {
                name = commandBufferName 
            };
        }
        
        internal static void AssignVolumeStack()
        {
            s_volumeStack = VolumeManager.instance.stack;
        }

        internal static void SetupCameraBuffers(ref CameraData cameraData)
        {
            var descriptor = cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            
            RenderingUtils.ReAllocateIfNeeded(ref s_tempColorTarget, descriptor, FilterMode.Bilinear);
            
            s_cameraColorTarget = cameraData.renderer.cameraColorTargetHandle;

            s_currentSource = null;
            s_currentDestination = null;
        }
        
        internal static void AssignContext(ref ScriptableRenderContext context)
        {
            s_context = context;
        }

        internal abstract void Setup();

        internal void Render()
        {
            if (IsActive == false) return;
            
            var shader_passes = sr_shaderPasses;
            shader_passes.Clear();

            OnPrepare(m_material, shader_passes);
            OnRender(shader_passes);
            ExecuteCommandBuffer();
        }

        protected abstract void OnPrepare(Material material, Queue<int> shaderPasses);

        private void OnRender(Queue<int> shaderPasses)
        {
            if (shaderPasses.Count == 0)
            {
                shaderPasses.Enqueue(0);
            }
            
            while (shaderPasses.Count > 0)
            {
                SwitchBuffers();
                int pass = shaderPasses.Dequeue();
                Blitter.BlitCameraTexture(s_cmd, s_currentSource, s_currentDestination, m_material, pass);
            }
        }
        
        private static void SwitchBuffers()
        {
            if (s_currentSource != s_cameraColorTarget)
            {
                s_currentSource = s_cameraColorTarget;
                s_currentDestination = s_tempColorTarget;
            }
            else
            {
                s_currentSource = s_tempColorTarget;
                s_currentDestination = s_cameraColorTarget;
            }
        }

        internal static void RenderFinalBlit()
        {
            var dest = s_currentDestination;
            if (dest == null) return;

            var cam = s_cameraColorTarget;
            if (dest == cam) return;

            Blitter.BlitCameraTexture(s_cmd, dest, cam, s_blitMaterial, 0);
            ExecuteCommandBuffer();
        }
        
        private static void ExecuteCommandBuffer()
        {
            s_context.ExecuteCommandBuffer(s_cmd);
            s_cmd.Clear();
        }
        
        internal void Release()
        {
            CoreUtils.Destroy(m_material);
        }

        internal static void ReleaseCameraBuffers()
        {
            s_tempColorTarget?.Release();
            s_tempColorTarget = null;
            s_cameraColorTarget = null;
            s_volumeStack = null;
        }
    };
    
    public abstract class ImageEffectPass<TVolume> : ImageEffectPass where TVolume : ImageEffectVolume
    {
        TVolume m_volume;
        bool m_isActive;
        
        protected sealed override bool IsActive => m_isActive;

        internal sealed override void Setup()
        {
            var volume = s_volumeStack.GetComponent<TVolume>();
            m_isActive = volume.IsActive();
            m_volume = volume;
        }

        protected sealed override void OnPrepare(Material material, Queue<int> shaderPasses)
        {
            OnPrepare(material, m_volume, shaderPasses);
        }

        protected abstract void OnPrepare(Material material, TVolume volume, Queue<int> shaderPasses);
    };
}
