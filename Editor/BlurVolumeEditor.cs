using UnityEditor;
using UnityEditor.Rendering;

namespace URPImageEffectsAdapterEditor.Effects
{
    using URPImageEffectsAdapterEditor;
    using URPImageEffectsAdapter.Effects;
    
    [CustomEditor(typeof(BlurVolume))]
    public sealed class BlurVolumeEditor : ImageEffectVolumeEditor<BlurVolume>
    {
        SerializedDataParameter m_mode;
        SerializedDataParameter m_kernelSize;
        SerializedDataParameter m_sigma;

        public override void OnEnable()
        {
            m_mode = UnpackParameter(x => x.mode);
            m_kernelSize = UnpackParameter(x => x.kernelSize);
            m_sigma = UnpackParameter(x => x.sigma);
        }

        public override void OnInspectorGUI()
        {
            PropertyField(m_mode);
            
            var mode = (EBlurMode)m_mode.value.intValue;
            
            PropertyField(m_kernelSize);
            
            if (mode == EBlurMode.Gaussian)
            {
                PropertyField(m_sigma);
            }
        }
    };
}
