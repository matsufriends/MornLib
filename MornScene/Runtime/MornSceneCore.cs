using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornScene
{
    public static class MornSceneCore<TEnum> where TEnum : Enum
    {
        private static readonly List<MornSceneMonoBase> s_sceneUpdateList = new();
        private static readonly List<MornSceneMonoBase> s_cachedUpdateList = new();

        internal static void Reset()
        {
            s_sceneUpdateList.Clear();
            s_cachedUpdateList.Clear();
        }

        internal static void ChangeScene(TEnum sceneType)
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            foreach (var updateScene in s_sceneUpdateList)
            {
                updateScene.OnExitScene(solver[updateScene]);
            }

            s_sceneUpdateList.Clear();
            AddScene(sceneType);
        }

        internal static void AddScene(TEnum sceneType)
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            var scene = solver[sceneType];
            scene.OnEnterScene(sceneType);
            s_sceneUpdateList.Add(scene);
        }

        internal static void RemoveScene(TEnum sceneType)
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            var scene = solver[sceneType];
            scene.OnExitScene(sceneType);
            if (s_sceneUpdateList.Count > 0)
            {
                if (s_sceneUpdateList[^1] != scene)
                {
                    Debug.LogError($"[RemoveScene({sceneType})]:TOPのシーン({s_sceneUpdateList[^1]})からRemoveして下さい。");
                }
            }
            else
            {
                Debug.LogError($"[RemoveScene({sceneType})]:Sceneが追加されていません。");
            }
        }

        internal static void UpdateScene()
        {
            s_cachedUpdateList.Clear();
            s_cachedUpdateList.AddRange(s_sceneUpdateList);
            for (var i = 0; i < s_cachedUpdateList.Count; i++)
            {
                s_cachedUpdateList[i].OnUpdateScene(i == 0);
            }
        }
    }
}
