using UnityEngine;

namespace MornLib.Physics2d
{
    [CreateAssetMenu(fileName = nameof(MornPhysics2dLocalSettingSo), menuName = nameof(MornPhysics2dLocalSettingSo))]
    public class MornPhysics2dLocalSettingSo : ScriptableObject
    {
        [SerializeField] private float _jumpUpDuration;
        [SerializeField] private float _jumpDownDuration;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _movement;
        [SerializeField] [Range(0, 1f)] private float _velocityLerpT;
        public float JumpUpDuration => _jumpUpDuration;
        public float JumpDownDuration => _jumpDownDuration;
        public float JumpHeight => _jumpHeight;
        public float Movement => _movement;
        public float VelocityLerpT => _velocityLerpT;
    }
}
