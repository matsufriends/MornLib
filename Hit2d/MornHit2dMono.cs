using UnityEngine;

namespace MornLib.Hit2d
{
    public abstract class MornHit2dMono : MonoBehaviour
    {
        protected bool IsActive { get; private set; }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }

        public abstract int Overlap(Collider2D[] other, LayerMask layerMask);
    }
}
