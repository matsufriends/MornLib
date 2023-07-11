using System.Collections.Generic;
using UnityEngine;

namespace MornScene
{
    [RequireComponent(typeof(MornSceneDictionaryMono))]
    public sealed class MornSceneSolver : MonoBehaviour
    {
        //変数名はEditor拡張よりシリアライズされるので要注意
        [SerializeField] private MornSceneDictionaryMono _sceneDictionary;
        [SerializeField] private MornSceneDataSo _firstScene;
        private readonly Dictionary<MornSceneDataSo, MornSceneMonoBase> _nameToSceneDict = new();
        private static MornSceneSolver s_instance;

        internal MornSceneMonoBase this[MornSceneDataSo sceneName] => _nameToSceneDict[sceneName];

        public static MornSceneSolver Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                s_instance = FindObjectOfType<MornSceneSolver>();
                if (s_instance == null)
                {
                    Debug.LogError($"{nameof(MornSceneSolver)} is not found.");
                }

                return s_instance;
            }
        }

        private void OnDestroy()
        {
            MornSceneCore.Reset();
        }

        private void Reset()
        {
            _sceneDictionary = GetComponent<MornSceneDictionaryMono>();
        }

        /*
        //関数名がEditor拡張より指定されているため要注意
        public void HideAll()
        {
            Assert.IsTrue(_keyList.Count == _valueList.Count);
            for (var i = 0; i < _valueList.Count; i++)
            {
                var sceneType = _keyList[i];
                var scene = _valueList[i];
                if (scene != null)
                {
                    scene.SetSceneActive(sceneType, false);
                }
            }

            Debug.Log("All Scenes Hidden.");
        }

        //関数名がEditor拡張より指定されているため要注意
        public void ChangeScene(string sceneName)
        {
            Assert.IsTrue(_keyList.Count == _valueList.Count);
            for (var i = 0; i < _valueList.Count; i++)
            {
                var sceneType = _keyList[i];
                var scene = _valueList[i];
                if (scene != null)
                {
                    scene.SetSceneActive(sceneType, sceneType.ToString() == sceneName);
                }
            }

            Debug.Log($"Scene Changed to {sceneName}.");
        }
        */

        private void Awake()
        {
            _nameToSceneDict.Clear();
            foreach (var pair in _sceneDictionary.GetDictionary())
            {
                _nameToSceneDict.Add(pair.Key, pair.Value);
                pair.Value.Initialize(pair.Key);
            }
        }

        private void Start()
        {
            MornSceneCore.ChangeScene(_firstScene);
        }

        private void Update()
        {
            MornSceneCore.UpdateScene();
        }
    }
}
