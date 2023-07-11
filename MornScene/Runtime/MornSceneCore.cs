using System.Collections.Generic;
using UnityEngine;

namespace MornScene
{
    public static class MornSceneCore
    {
        private static readonly List<MornSceneDataSo> _sceneUpdateList = new();
        private static readonly List<MornSceneDataSo> _cachedUpdateList = new();

        internal static void Reset()
        {
            _sceneUpdateList.Clear();
            _cachedUpdateList.Clear();
        }

        internal static void ChangeScene(MornSceneDataSo sceneDataSo)
        {
            var solver = MornSceneSolver.Instance;
            foreach (var updateScene in _sceneUpdateList)
            {
                solver[updateScene].OnExitScene(updateScene);
            }

            _sceneUpdateList.Clear();
            AddScene(sceneDataSo);
        }

        internal static void AddScene(MornSceneDataSo sceneData)
        {
            var solver = MornSceneSolver.Instance;
            solver[sceneData].OnEnterScene(sceneData);
            _sceneUpdateList.Add(sceneData);
        }

        internal static void RemoveScene(MornSceneDataSo sceneData)
        {
            var solver = MornSceneSolver.Instance;
            var scene = solver[sceneData];
            scene.OnExitScene(sceneData);
            var sceneName = sceneData.SceneName;
            if (_sceneUpdateList.Count > 0)
            {
                if (_sceneUpdateList[^1] != sceneData)
                {
                    Debug.LogError($"[RemoveScene({sceneName})]:TOPのシーン({_sceneUpdateList[^1]})からRemoveして下さい。");
                }
            }
            else
            {
                Debug.LogError($"[RemoveScene({sceneName})]:Sceneが追加されていません。");
            }
        }

        internal static void UpdateScene()
        {
            var solver = MornSceneSolver.Instance;
            _cachedUpdateList.Clear();
            _cachedUpdateList.AddRange(_sceneUpdateList);
            for (var i = 0; i < _cachedUpdateList.Count; i++)
            {
                var sceneData = _cachedUpdateList[i];
                solver[sceneData].OnUpdateScene(sceneData, i == 0);
            }
        }
    }
}
