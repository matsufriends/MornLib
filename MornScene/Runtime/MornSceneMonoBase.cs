using System;
using UniRx;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornScene
{
    public abstract class MornSceneMonoBase : MonoBehaviour
    {
        [SerializeField] private MornSceneCanvasMono _sceneCanvas;
        [SerializeField] private GameObject _root;
        protected bool ActiveSelf { get; private set; }
        private readonly Subject<Unit> _onEnterSceneSubject = new();
        private readonly Subject<Unit> _onUpdateSceneSubject = new();
        private readonly Subject<Unit> _onExitSceneSubject = new();
        public IObservable<Unit> OnEnterSceneRx => _onEnterSceneSubject;
        public IObservable<Unit> OnUpdateSceneRx => _onUpdateSceneSubject;
        public IObservable<Unit> OnExitSceneRx => _onExitSceneSubject;

        protected void ChangeScene<TEnum>(TEnum sceneType) where TEnum : Enum
        {
            MornSceneCore<TEnum>.ChangeScene(sceneType);
        }

        protected void AddScene<TEnum>(TEnum sceneType) where TEnum : Enum
        {
            MornSceneCore<TEnum>.AddScene(sceneType);
        }

        protected void RemoveScene<TEnum>(TEnum sceneType) where TEnum : Enum
        {
            MornSceneCore<TEnum>.RemoveScene(sceneType);
        }

        internal void Initialize()
        {
            OnInitializeImpl();
        }

        internal void OnEnterScene<TEnum>(TEnum sceneType) where TEnum : Enum
        {
            ActiveSelf = true;
            SetSceneActive(sceneType, true);
            OnEnterSceneImpl();
            _onEnterSceneSubject.OnNext(Unit.Default);
        }

        internal void OnUpdateScene(bool isTop)
        {
            OnUpdateSceneImpl(isTop);
            _onUpdateSceneSubject.OnNext(Unit.Default);
        }

        internal void OnExitScene<TEnum>(TEnum sceneType) where TEnum : Enum
        {
            ActiveSelf = false;
            SetSceneActive(sceneType, false);
            OnExitSceneImpl();
            _onExitSceneSubject.OnNext(Unit.Default);
        }

        protected abstract void OnInitializeImpl();
        protected abstract void OnEnterSceneImpl();
        protected abstract void OnUpdateSceneImpl(bool isTop);
        protected abstract void OnExitSceneImpl();

        internal void ApplyCanvasScale(int width, int height)
        {
            if (_sceneCanvas != null)
            {
                _sceneCanvas.ApplyCanvasScale(width, height);
            }
        }

        internal void SetSceneActive<TEnum>(TEnum sceneType, bool isActive) where TEnum : Enum
        {
            if (_sceneCanvas != null)
            {
                _sceneCanvas.SetActiveImmediate(isActive);
                _sceneCanvas.name = $"[{sceneType}] Canvas";
            }

            if (_root != null)
            {
                _root.SetActive(isActive);
                _root.name = $"[{sceneType}] Root";
            }

            gameObject.name = $"[{sceneType}] {(isActive ? "Active" : "")}";
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }
    }
}
