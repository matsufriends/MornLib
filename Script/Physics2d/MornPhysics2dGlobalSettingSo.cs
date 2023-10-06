using MornSingleton;
using UnityEngine;

namespace MornLib.Physics2d
{
    [CreateAssetMenu(fileName = nameof(MornPhysics2dGlobalSettingSo), menuName = "MornLib/"+nameof(MornPhysics2dGlobalSettingSo))]
    public class MornPhysics2dGlobalSettingSo : MornSingletonSo<MornPhysics2dGlobalSettingSo>
    {
        [SerializeField] private float _groundY;
        public float GroundY => _groundY;
    }
}