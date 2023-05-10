using MornAttribute;
using UnityEngine;

namespace MornUI
{
    public sealed class MornUISolverMono : MonoBehaviour
    {
        [SerializeField] [ReadOnly] internal MornUISelectableMono _current;
        [SerializeField] internal float _freezeSeconds = 0.2f;
        [SerializeField] internal float _fastIntervalSeconds = 0.08f;
    }
}
