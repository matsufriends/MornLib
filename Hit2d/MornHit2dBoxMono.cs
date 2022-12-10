using UnityEngine;

namespace MornLib.Hit2d
{
    public sealed class MornHit2dBoxMono : MornHit2dMono
    {
        [SerializeField] private Vector2 _size;

        public override int Overlap(Collider2D[] other, LayerMask layerMask)
        {
            return Physics2D.OverlapBoxNonAlloc(transform.position, _size, transform.eulerAngles.z, other, layerMask);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (MornHit2dSettingSo.Instance.DrawGizmos == false)
            {
                return;
            }

            if (Application.isPlaying == false || IsActive)
            {
                var cache = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawWireCube(Vector3.zero, _size);
                Gizmos.matrix = cache;
            }
        }
#endif
    }
}
