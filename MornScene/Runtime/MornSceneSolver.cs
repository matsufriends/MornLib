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
            foreach (var pair in _sceneDictionary.GetDictionary())
            {
                pair.Value.SetSceneActive(pair.Key, false);
            }

            MornSceneCore.ChangeScene(_firstScene);
        }

        private void Update()
        {
            MornSceneCore.UpdateScene();
        }
    }
}
