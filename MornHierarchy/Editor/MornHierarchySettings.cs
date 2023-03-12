using UnityEditor;
using UnityEngine;

namespace MornHierarchy
{
    /// <summary>MornHierarchyの設定ファイル</summary>
    [FilePath(nameof(MornHierarchySettings) + ".asset", FilePathAttribute.Location.PreferencesFolder)]
    public sealed class MornHierarchySettings : ScriptableSingleton<MornHierarchySettings>
    {
        /// <summary>Tagを可視化するか</summary>
        [SerializeField] private bool _showTag = true;

        /// <summary>透明度</summary>
        [SerializeField, Range(0, 1)] private float _transparent = 0.3f;

        /// <summary>Tagを可視化するか</summary>
        public bool ShowTag => _showTag;

        /// <summary>透明度</summary>
        public float Transparent => _transparent;

        /// <summary>設定を永続化する</summary>
        public void Save()
        {
            Save(true);
        }
    }
}
