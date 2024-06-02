using UnityEngine;

namespace MornCamera
{
    [CreateAssetMenu(fileName = nameof(MornCameraSettingSo), menuName = nameof(MornCameraSettingSo))]
    public sealed class MornCameraSettingSo : ScriptableObject
    {
        public Vector2 Resolution = new(1920, 1080);
    }
}