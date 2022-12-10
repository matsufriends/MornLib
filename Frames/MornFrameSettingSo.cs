using MornLib.Singletons;
using UnityEngine;

namespace MornLib.Frames
{
    [CreateAssetMenu(fileName = nameof(MornFrameSettingSo), menuName = nameof(MornFrameSettingSo))]
    public class MornFrameSettingSo : MornSingletonSo<MornFrameSettingSo>
    {
        [SerializeField] private int _aimFps = 60;
        public int AimFps => _aimFps;
    }
}
