using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornScene
{
    [RequireComponent(typeof(MornSceneMonoBase))]
    public sealed class MornSceneOnEnterFocusMono : MonoBehaviour
    {
        [SerializeField] private MornSceneMonoBase _scene;
        [SerializeField] private GameObject _focusObject;

        private void Awake()
        {
            _scene.OnEnterSceneRx.Subscribe(_ => EventSystem.current.SetSelectedGameObject(_focusObject)).AddTo(this);
        }

        private void Reset()
        {
            _scene = GetComponent<MornSceneMonoBase>();
        }
    }
}