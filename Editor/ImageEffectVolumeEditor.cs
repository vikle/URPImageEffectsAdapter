using System;
using System.Linq.Expressions;
using UnityEditor.Rendering;

namespace URPImageEffectsAdapterEditor
{
    using URPImageEffectsAdapter;

    public abstract class ImageEffectVolumeEditor<TVolume> : VolumeComponentEditor where TVolume : ImageEffectVolume
    {
        PropertyFetcher<TVolume> m_fetcher;

        protected SerializedDataParameter UnpackParameter<TValue>(Expression<Func<TVolume, TValue>> expr)
        {
            if (m_fetcher == null)
            {
                m_fetcher = new PropertyFetcher<TVolume>(serializedObject);
            }

            return Unpack(m_fetcher.Find(expr));
        }
    };
}
