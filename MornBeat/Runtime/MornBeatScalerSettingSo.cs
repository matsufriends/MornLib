using UnityEngine;

namespace MornBeat
{
    [CreateAssetMenu(fileName = nameof(MornBeatScalerSettingSo), menuName = "MornBeat/" + nameof(MornBeatScalerSettingSo))]
    public sealed class MornBeatScalerSettingSo : ScriptableObject
    {
        [SerializeField] private float _aimScale = 1.2f;
        [SerializeField] private float _scaleLerpT = 10;
        public float AimScale => _aimScale;
        public float ScaleLerpT => _scaleLerpT;
    }
}
