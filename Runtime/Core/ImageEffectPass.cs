using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter
{
    public abstract class ImageEffectPass
    {
        public abstract bool IsActive { get; }
        
        protected static ScriptableRenderContext s_context;
        protected static readonly CommandBuffer sr_cmd;
        
        protected static RTHandle s_temporaryBuffer;
        protected static RTHandle s_cameraColorTarget;

        static Material s_blitMaterial;
        
        readonly ProfilingSampler m_profilingSampler;

        protected Material m_material;
        
        static ImageEffectPass()
        {
            sr_cmd = new CommandBuffer();
        }

        protected ImageEffectPass(Shader shader)
        {
            m_profilingSampler = new ProfilingSampler(GetType().Name);
            m_material = CoreUtils.CreateEngineMaterial(shader);
        }

        public static void CreateBlitMaterialIfNeeded(Shader shader)
        {
            if (s_blitMaterial == null)
            {
                s_blitMaterial = CoreUtils.CreateEngineMaterial(shader);
            }
        }
        
        public static void SetupStatic(ref ScriptableRenderContext context, 
                                       RTHandle temporaryBuffer,
                                       RTHandle cameraColorTarget)
        {
            s_context = context;
            s_temporaryBuffer = temporaryBuffer;
            s_cameraColorTarget = cameraColorTarget;
        }

        public abstract void Setup(VolumeStack stack);

        public void Render()
        {
            if (IsActive == false) return;
            
            using (new ProfilingScope(sr_cmd, m_profilingSampler))
            {
                OnRender();
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
    };
    
    public abstract class ImageEffectPass<TVolume> : ImageEffectPass where TVolume : ImageEffectVolume
    {
        protected ImageEffectPass(Shader shader) : base(shader) { }
        
        protected TVolume m_volume;
        
        public override bool IsActive => m_volume.IsActive();

        public override void Setup(VolumeStack stack)
        {
            m_volume = stack.GetComponent<TVolume>();
        }
    };
}
