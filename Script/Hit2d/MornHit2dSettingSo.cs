using MornSingleton;
using UnityEngine;

namespace MornLib.Hit2d
{
    [CreateAssetMenu(fileName = nameof(MornHit2dSettingSo), menuName = "MornLib/" + nameof(MornHit2dSettingSo))]
    public sealed class MornHit2dSettingSo : MornSingletonSo<MornHit2dSettingSo>
    {
        [SerializeField] private bool _drawGizmos;
        public bool DrawGizmos => _drawGizmos;
    }
}