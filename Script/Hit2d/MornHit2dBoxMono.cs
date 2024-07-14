using UnityEngine;

namespace MornLib.Hit2d
{
    public sealed class MornHit2dBoxMono : MornHit2dMono
    {
        [SerializeField] private Vector2 _size;

        protected override int OverlapImpl(Collider2D[] results, LayerMask layerMask)
        {
            return Physics2D.OverlapBoxNonAlloc(transform.position, _size, transform.eulerAngles.z, results, layerMask);
        }

        protected override void DrawGizmosImpl()
        {
            var cache = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(_size.x, _size.y, 1));
            Gizmos.matrix = cache;
        }
    }
}