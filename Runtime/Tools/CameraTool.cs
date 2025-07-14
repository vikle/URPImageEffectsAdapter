using UnityEngine;

namespace URPImageEffectsAdapter
{
    internal static class CameraTool
    {
        internal static bool IsRealCamera(CameraType cameraType)
        {
            switch (cameraType)
            {
                case CameraType.SceneView: 
                case CameraType.Game:
                case CameraType.VR: 
                    return true;

                case CameraType.Reflection:
                case CameraType.Preview:
                default: return false;
            }
        }
    };
}
