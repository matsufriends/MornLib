using UnityEngine;

namespace MornScene
{
    [CreateAssetMenu(fileName = nameof(MornSceneDataSo), menuName = nameof(MornSceneDataSo))]
    public sealed class MornSceneDataSo : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        internal string SceneName => _sceneName;
    }
}
