using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornScene
{
    public static class MornSceneCore<TEnum> where TEnum : Enum
    {
        private static readonly Stack<MornSceneMonoBase> s_sceneUpdateStack = new();
        private static readonly List<MornSceneMonoBase> s_cachedUpdateList = new();

        static MornSceneCore()
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            solver.Initialize();
        }

        internal static void ChangeScene(TEnum sceneType)
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            while (s_sceneUpdateStack.TryPop(out var updateScene))
            {
                updateScene.OnExitScene(solver[updateScene]);
            }

            AddScene(sceneType);
        }

        internal static void AddScene(TEnum sceneType)
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            var scene = solver[sceneType];
            scene.OnEnterScene(sceneType);
            s_sceneUpdateStack.Push(scene);
        }

        internal static void RemoveScene(TEnum sceneType)
        {
            var solver = MornSceneSolverBase<TEnum>.Instance;
            var scene = solver[sceneType];
            scene.OnExitScene(sceneType);
            if (s_sceneUpdateStack.TryPop(out var topScene))
            {
                if (topScene != scene)
                {
                    Debug.LogError($"[RemoveScene({sceneType})]:TOPのシーン({solver[topScene]})からRemoveして下さい。");
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
            s_cachedUpdateList.AddRange(s_sceneUpdateStack);
            for (var i = 0; i < s_cachedUpdateList.Count; i++)
            {
                s_cachedUpdateList[i].OnUpdateScene(i == 0);
            }
        }
    }
}
