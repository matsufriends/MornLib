using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornScene
{
    public static class MornSceneCore
    {
        private static readonly List<MornSceneMonoBase> _sceneUpdateList = new();
        private static readonly List<MornSceneMonoBase> _cachedUpdateList = new();

        internal static void Reset()
        {
            _sceneUpdateList.Clear();
            _cachedUpdateList.Clear();
        }

        internal static void ChangeScene<TEnum>(TEnum sceneType) where TEnum : Enum
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            foreach (var updateScene in _sceneUpdateList)
            {
                updateScene.OnExitScene(solver[updateScene]);
            }

            _sceneUpdateList.Clear();
            AddScene(sceneType);
        }

        internal static void AddScene<TEnum>(TEnum sceneType) where TEnum : Enum
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            var scene = solver[sceneType];
            scene.OnEnterScene(sceneType);
            _sceneUpdateList.Add(scene);
        }

        internal static void RemoveScene<TEnum>(TEnum sceneType) where TEnum : Enum
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            var scene = solver[sceneType];
            scene.OnExitScene(sceneType);
            if (_sceneUpdateList.Count > 0)
            {
                if (_sceneUpdateList[^1] != scene)
                {
                    Debug.LogError($"[RemoveScene({sceneType})]:TOPのシーン({_sceneUpdateList[^1]})からRemoveして下さい。");
                }
            }
            else
            {
                Debug.LogError($"[RemoveScene({sceneType})]:Sceneが追加されていません。");
            }
        }

        internal static void UpdateScene()
        {
            _cachedUpdateList.Clear();
            _cachedUpdateList.AddRange(_sceneUpdateList);
            for (var i = 0; i < _cachedUpdateList.Count; i++)
            {
                _cachedUpdateList[i].OnUpdateScene(i == 0);
            }
        }
    }
}
