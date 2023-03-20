using UnityEditor;
using UnityEngine;

namespace MornHierarchy
{
    [FilePath(nameof(MornHierarchySettings) + ".asset", FilePathAttribute.Location.PreferencesFolder)]
    internal sealed class MornHierarchySettings : ScriptableSingleton<MornHierarchySettings>
    {
        [SerializeField] private bool _showTag = true;
        [SerializeField, Range(0, 1)] private float _transparent = 0.3f;
        public bool ShowTag => _showTag;
        public float Transparent => _transparent;

        public void Save()
        {
            Save(true);
        }
    }
}
