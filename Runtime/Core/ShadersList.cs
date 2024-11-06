using System;
using UnityEngine;

namespace URPImageEffectsAdapter
{
    [Serializable]
    public struct ShadersList
    {
        public Shader kuwahara;
        public Shader sobelFilter;
        public Shader sharpness;
        public Shader blur;
    };
}
