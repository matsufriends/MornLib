using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornScene
{
    [RequireComponent(typeof(MornSceneMonoBase))]
    public abstract class MornSceneOnButtonPressedFocusMonoBase : MonoBehaviour
    {
        [SerializeField] private MornSceneMonoBase _scene;
        [SerializeField] private GameObject _focusObject;
        protected abstract bool OnBackButtonPressed { get; }

        private void Reset()
        {
            _scene = GetComponent<MornSceneMonoBase>();
        }

        private void Awake()
        {
            _scene.OnUpdateSceneRx.Subscribe(
                _ =>
                {
                    if (OnBackButtonPressed)
                    {
                        EventSystem.current.SetSelectedGameObject(_focusObject);
                    }
                }).AddTo(this);
        }
    }
}
