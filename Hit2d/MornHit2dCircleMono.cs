using UnityEngine;

namespace MornLib.Hit2d
{
    public sealed class MornHit2dCircleMono : MornHit2dMono
    {
        [SerializeField] private float _radius;

        public override int Overlap(Collider2D[] other, LayerMask layerMask)
        {
            return Physics2D.OverlapCircleNonAlloc(transform.position, _radius, other, layerMask);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (IsActive && MornHit2dSettingSo.Instance.DrawGizmos)
            {
                Gizmos.DrawWireSphere(transform.position, _radius);
            }
        }
#endif
    }
}
