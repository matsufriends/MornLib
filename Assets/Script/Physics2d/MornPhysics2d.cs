using MornLib.Cores;
using UnityEngine;

namespace MornLib.Physics2d
{
    public sealed class MornPhysics2d : MonoBehaviour
    {
        [SerializeField] private MornPhysics2dLocalSettingSo _localSetting;
        private MornPhysics2dGlobalSettingSo _globalSetting;
        private Vector2 _vel;
        public Vector2 Vel => _vel;
        public Vector2 Pos => transform.position;
        public MornPhysics2dLocalSettingSo LocalSetting => _localSetting;

        private void Awake()
        {
            _globalSetting = MornPhysics2dGlobalSettingSo.Instance;
        }

        public void SetVelocity(Vector2 vel)
        {
            _vel = vel;
        }

        public void SetJumpVelocity()
        {
            _vel.y = 2 * _localSetting.JumpHeight / _localSetting.JumpUpDuration;
        }

        public void MoveX(float xMovement, float maxVelocityRatio)
        {
            _vel.x = Mathf.Lerp(_vel.x,
                MornMath.ClampMinus1Plus1(xMovement) * _localSetting.Movement * maxVelocityRatio,
                _localSetting.VelocityLerpT);
        }

        public bool IsOnGround()
        {
            var pos = transform.position;
            return MornMath.IsNearZero(pos.y - _globalSetting.GroundY) || pos.y <= _globalSetting.GroundY;
        }

        private float GetGravity()
        {
            var duration = _vel.y >= 0 ? _localSetting.JumpUpDuration : _localSetting.JumpDownDuration;
            return -2 * _localSetting.JumpHeight / (duration * duration);
        }

        public void MyFixedUpdate()
        {
            //v = vo + at
            //x = vo t + 1/2 a t^2

            //H:最高点 T:上昇まで/下降まで
            //H = vo * T/2
            //vo = 2 * H / T

            //0 = v0 + a * T
            //a = -(2 * H / T) / T
            _vel.y += GetGravity() * Time.fixedDeltaTime;
            var pos = transform.position;
            pos.x += _vel.x * Time.fixedDeltaTime;
            pos.y += _vel.y * Time.fixedDeltaTime;
            transform.position = pos;
            if (IsOnGround())
            {
                pos.y = _globalSetting.GroundY;
                _vel.y = 0;
                transform.position = pos;
            }
        }
    }
}
