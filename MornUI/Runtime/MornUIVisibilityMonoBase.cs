using UnityEngine;

namespace MornUI
{
    public abstract class MornUIVisibilityMonoBase : MonoBehaviour
    {
        public abstract void Show(bool immediate = false);
        public abstract void Hide(bool immediate = false);
    }
}