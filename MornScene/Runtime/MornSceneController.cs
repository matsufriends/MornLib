using System.Collections.Generic;
using UnityEngine;

namespace MornScene
{
    public sealed class MornSceneController
    {
        private readonly List<MornSceneDataSo> _cachedUpdateList = new();
        private readonly MornSceneSolver _sceneSolver;
        private readonly List<MornSceneDataSo> _sceneUpdateList = new();

        public MornSceneController(MornSceneSolver sceneSolver)
        {
            _sceneSolver = sceneSolver;
        }

        internal void Reset()
        {
            _sceneUpdateList.Clear();
            _cachedUpdateList.Clear();
        }

        internal void ChangeScene(MornSceneDataSo sceneDataSo)
        {
            foreach (var updateScene in _sceneUpdateList) _sceneSolver[updateScene].OnExitScene(updateScene);

            _sceneUpdateList.Clear();
            AddScene(sceneDataSo);
        }

        internal void AddScene(MornSceneDataSo sceneData)
        {
            _sceneSolver[sceneData].OnEnterScene(sceneData);
            _sceneUpdateList.Add(sceneData);
        }

        internal void RemoveScene(MornSceneDataSo sceneData)
        {
            _sceneSolver[sceneData].OnExitScene(sceneData);
            var sceneName = sceneData.SceneName;
            if (_sceneUpdateList.Count > 0)
            {
                if (_sceneUpdateList[^1] != sceneData)
                    Debug.LogError($"[RemoveScene({sceneName})]:TOPのシーン({_sceneUpdateList[^1]})からRemoveして下さい。");
                else
                    _sceneUpdateList.RemoveAt(_sceneUpdateList.Count - 1);
            }
            else
            {
                Debug.LogError($"[RemoveScene({sceneName})]:Sceneが追加されていません。");
            }
        }

        internal void UpdateScene()
        {
            _cachedUpdateList.Clear();
            _cachedUpdateList.AddRange(_sceneUpdateList);
            for (var i = 0; i < _cachedUpdateList.Count; i++)
            {
                var sceneData = _cachedUpdateList[i];
                _sceneSolver[sceneData].OnUpdateScene(sceneData, i == _cachedUpdateList.Count - 1);
            }
        }
    }
}