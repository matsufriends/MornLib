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
        [SerializeField] private MornSceneDataSo _sceneData;
        [SerializeField] private MornSceneCanvasMono _sceneCanvas;
        [SerializeField] private GameObject _root;
        protected bool ActiveSelf { get; private set; }
        private readonly Subject<Unit> _onEnterSceneSubject = new();
        private readonly Subject<Unit> _onUpdateSceneSubject = new();
        private readonly Subject<Unit> _onExitSceneSubject = new();
        public IObservable<Unit> OnEnterSceneRx => _onEnterSceneSubject;
        public IObservable<Unit> OnUpdateSceneRx => _onUpdateSceneSubject;
        public IObservable<Unit> OnExitSceneRx => _onExitSceneSubject;

        protected void ChangeScene(MornSceneDataSo sceneData)
        {
            MornSceneCore.ChangeScene(sceneData);
        }

        protected void AddScene(MornSceneDataSo sceneData)
        {
            MornSceneCore.AddScene(sceneData);
        }

        protected void RemoveScene(MornSceneDataSo sceneData)
        {
            MornSceneCore.RemoveScene(sceneData);
        }

        internal void Initialize(MornSceneDataSo sceneData)
        {
            OnInitializeImpl(sceneData);
        }

        internal void OnEnterScene(MornSceneDataSo sceneData)
        {
            ActiveSelf = true;
            SetSceneActive(true);
            OnEnterSceneImpl(sceneData);
            _onEnterSceneSubject.OnNext(Unit.Default);
        }

        internal void OnUpdateScene(MornSceneDataSo sceneData, bool isTop)
        {
            OnUpdateSceneImpl(sceneData, isTop);
            _onUpdateSceneSubject.OnNext(Unit.Default);
        }

        internal void OnExitScene(MornSceneDataSo sceneData)
        {
            ActiveSelf = false;
            SetSceneActive(false);
            OnExitSceneImpl(sceneData);
            _onExitSceneSubject.OnNext(Unit.Default);
        }

        protected abstract void OnInitializeImpl(MornSceneDataSo sceneData);
        protected abstract void OnEnterSceneImpl(MornSceneDataSo sceneData);
        protected abstract void OnUpdateSceneImpl(MornSceneDataSo sceneData, bool isTop);
        protected abstract void OnExitSceneImpl(MornSceneDataSo sceneData);

        internal void ApplyCanvasScale(int width, int height)
        {
            if (_sceneCanvas != null)
            {
                _sceneCanvas.ApplyCanvasScale(width, height);
            }
        }

        internal void SetSceneActive(bool isActive)
        {
            var sceneName = _sceneData.SceneName;
            if (_sceneCanvas != null)
            {
                _sceneCanvas.SetActiveImmediate(isActive);
                _sceneCanvas.name = $"[{sceneName}] Canvas";
            }

            if (_root != null)
            {
                _root.SetActive(isActive);
                _root.name = $"[{sceneName}] Root";
            }

            gameObject.name = $"[{sceneName}] {(isActive ? "Active" : "")}";
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
#endif
        }
    }
}
