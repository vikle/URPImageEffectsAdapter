using UnityEngine;
using UnityEngine.Rendering;

namespace URPImageEffectsAdapter
{
    public static class MaterialExtensions
    {
        public static void SendVolumeParameter(this Material material, int nameId, IntParameter parameter)
        {
            material.SetInteger(nameId, parameter.value);
        }

        public static void SendVolumeParameter(this Material material, int nameId, FloatParameter parameter)
        {
            material.SetFloat(nameId, parameter.value);
        }

        public static void SendVolumeParameter(this Material material, int nameId, Vector3Parameter parameter)
        {
            material.SetVector(nameId, parameter.value);
        }

        public static void SendVolumeParameter(this Material material, int nameId, ColorParameter parameter)
        {
            material.SetColor(nameId, parameter.value);
        }
    };
}
