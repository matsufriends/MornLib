using UnityEngine;

namespace MornUI
{
    [RequireComponent(typeof(MornUISelectableMono))]
    public abstract class MornUIBehaviourMonoBase : MonoBehaviour
    {
        public abstract void Selected();
        public abstract void OnSubmit(out bool canTransition);
        public abstract void OnMove(MornUIAxisDirType axis, out bool canTransition);
    }
}
