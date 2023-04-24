using MornSingleton;
using UnityEngine;

namespace MornLib.UIs
{
    [CreateAssetMenu(fileName = nameof(MornFlashSo), menuName = nameof(MornFlashSo))]
    public class MornFlashSo : MornSingletonSo<MornFlashSo>
    {
        [SerializeField] private float _interval;
        public float Interval => _interval;
    }
}
