using UnityEngine;

namespace MornLib.Hit2d
{
    public abstract class MornHit2dMono : MonoBehaviour
    {
        private bool _drawGizmoFlag;

        public int Overlap(Collider2D[] results, LayerMask layerMask)
        {
            if (MornHit2dSettingSo.Instance.DrawGizmos)
            {
                _drawGizmoFlag = true;
            }

            return OverlapImpl(results, layerMask);
        }

        protected abstract int OverlapImpl(Collider2D[] results, LayerMask layerMask);
        protected abstract void DrawGizmosImpl();
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (MornHit2dSettingSo.Instance.DrawGizmos == false)
            {
                return;
            }

            if (Application.isPlaying == false)
            {
                DrawGizmosImpl();
            }
            else if (_drawGizmoFlag)
            {
                DrawGizmosImpl();
                _drawGizmoFlag = false;
            }
        }
#endif
    }
}
