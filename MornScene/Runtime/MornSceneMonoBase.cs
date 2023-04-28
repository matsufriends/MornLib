using System;
using UniRx;
using UnityEngine;

namespace MornScene
{
    public abstract class MornSceneMonoBase<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private MornSceneCanvasMono _sceneCanvas;
        [SerializeField] private GameObject _root;
        [SerializeField] private TEnum _sceneType;
        private readonly Subject<TEnum> _changeSceneSubject = new();
        private readonly Subject<TEnum> _addSceneSubject = new();
        private readonly Subject<TEnum> _removeSceneSubject = new();
        public TEnum SceneType => _sceneType;
        internal IObservable<TEnum> OnChangeScene => _changeSceneSubject;
        internal IObservable<TEnum> OnAddScene => _addSceneSubject;
        internal IObservable<TEnum> OnRemoveScene => _removeSceneSubject;
        protected bool ActiveSelf { get; private set; }

        protected void ChangeScene(TEnum sceneType)
        {
            _changeSceneSubject.OnNext(sceneType);
        }

        protected void AddScene(TEnum sceneType)
        {
            _addSceneSubject.OnNext(sceneType);
        }

        protected void RemoveScene(TEnum sceneType)
        {
            _removeSceneSubject.OnNext(sceneType);
        }

        internal void Initialize()
        {
            OnInitializeImpl();
        }

        internal void OnEnterScene()
        {
            ActiveSelf = true;
            SetSceneActive(true);
            OnEnterSceneImpl();
        }

        internal void OnUpdateScene(bool isTopMost)
        {
            OnUpdateSceneImpl(isTopMost);
        }

        internal void OnExitScene()
        {
            ActiveSelf = false;
            SetSceneActive(false);
            OnExitSceneImpl();
        }

        protected abstract void OnInitializeImpl();
        protected abstract void OnEnterSceneImpl();
        protected abstract void OnUpdateSceneImpl(bool isTopMost);
        protected abstract void OnExitSceneImpl();

        internal void ApplyCanvasScale(int width, int height)
        {
            if (_sceneCanvas != null)
            {
                _sceneCanvas.ApplyCanvasScale(width, height);
            }
        }

        internal void SetSceneActive(bool isActive)
        {
            if (_sceneCanvas != null)
            {
                _sceneCanvas.SetActiveImmediate(isActive);
            }

            if (_root != null)
            {
                _root.SetActive(isActive);
            }

            gameObject.name = $"[{_sceneType.ToString()}] {(isActive ? "Active" : "Inactive")}";
        }
    }
}
