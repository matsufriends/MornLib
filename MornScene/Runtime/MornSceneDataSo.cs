using UnityEngine;

namespace MornScene
{
    [CreateAssetMenu(fileName = nameof(MornSceneDataSo), menuName = "MornLib/" + nameof(MornSceneDataSo))]
    public sealed class MornSceneDataSo : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        public string SceneName => _sceneName;

        public void SetSceneName(string sceneName)
        {
            _sceneName = sceneName;
        }
    }
}