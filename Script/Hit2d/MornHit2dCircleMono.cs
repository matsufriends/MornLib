using UnityEngine;

namespace MornLib.Hit2d
{
    public sealed class MornHit2dCircleMono : MornHit2dMono
    {
        [SerializeField] private float _radius;

        protected override int OverlapImpl(Collider2D[] results, LayerMask layerMask)
        {
            return Physics2D.OverlapCircleNonAlloc(transform.position, _radius, results, layerMask);
        }

        protected override void DrawGizmosImpl()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}