using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace URPImageEffectsAdapter
{
    public abstract class ImageEffectPass : ScriptableObject
    {
        public Shader shader;
        
        public abstract bool IsActive { get; }
        
        protected static ScriptableRenderContext s_context;
        protected static CommandBuffer sr_cmd;
        
        protected static RTHandle s_temporaryBuffer;
        protected static RTHandle s_cameraColorTarget;
        protected static VolumeStack s_volumeStack;

        static Material s_blitMaterial;
        
        ProfilingSampler m_profilingSampler;

        protected Material m_material;

        void OnEnable()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            if (shader == null)
            {
                shader = OnInitializeShader();
            }
            
            if (m_material == null)
            {
                m_material = CoreUtils.CreateEngineMaterial(shader);
            }
            
            m_profilingSampler ??= new ProfilingSampler(GetType().Name);
        }

        protected abstract Shader OnInitializeShader();
        
        public static void CreateBlitMaterialIfNeeded(Shader shader)
        {
            if (s_blitMaterial == null)
            {
                s_blitMaterial = CoreUtils.CreateEngineMaterial(shader);
            }
        }

        public static void InitCommandBuffer()
        {
            sr_cmd ??= new CommandBuffer();
        }
        
        public static void SetupVolumeStack()
        {
            s_volumeStack = VolumeManager.instance.stack;
        }

        public static void SetupCameraBuffers(ref CameraData cameraData)
        {
            var descriptor = cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            RenderingUtils.ReAllocateIfNeeded(ref s_temporaryBuffer, descriptor, FilterMode.Bilinear);
            
            s_cameraColorTarget = cameraData.renderer.cameraColorTargetHandle;
        }
        
        public static void SetupScriptableRenderContext(ref ScriptableRenderContext context)
        {
            s_context = context;
        }
        
        public abstract void OnSetup();

        public void Render()
        {
            if (IsActive)
            {
                using (new ProfilingScope(sr_cmd, m_profilingSampler))
                {
                    OnRender();
                }
            }
        }

        protected abstract void OnRender();
        
        protected static void ExecuteCommandBuffer()
        {
            s_context.ExecuteCommandBuffer(sr_cmd);
            sr_cmd.Clear();
        }

        protected static void BlitCameraTexture(RTHandle source, RTHandle destination)
        {
            Blitter.BlitCameraTexture(sr_cmd, source, destination, s_blitMaterial, 0);
        }
        
        public void Release()
        {
            CoreUtils.Destroy(m_material);
        }

        public static void ReleaseCameraBuffers()
        {
            s_temporaryBuffer?.Release();
            s_temporaryBuffer = null;
            s_cameraColorTarget = null;
            s_volumeStack = null;
        }
    };
    
    public abstract class ImageEffectPass<TVolume> : ImageEffectPass where TVolume : ImageEffectVolume
    {
        protected TVolume m_volume;
        
        bool m_isActive;
        public override bool IsActive => m_isActive;

        public override void OnSetup()
        {
            var volume = s_volumeStack.GetComponent<TVolume>();
            m_isActive = volume.IsActive();
            m_volume = volume;
        }
    };
}
