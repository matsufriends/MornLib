using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MornHierarchy
{
    /// <summary>MornHierarchyの設定をPreferencesに表示するEditor拡張</summary>
    public sealed class MornHierarchySettingsProvider : SettingsProvider
    {
        /// <summary>ScriptableSingleton表示用Editor</summary>
        private Editor _cachedEditor;

        /// <summary>設定画面表示パス</summary>
        private const string SettingPath = "Preferences/MornHierarchy";

        /// <summary>SettingProviderの登録</summary>
        /// <returns>自身のインスタンス</returns>
        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            return new MornHierarchySettingsProvider(SettingPath, SettingsScope.User, null);
        }

        /// <inheritdoc/>
        private MornHierarchySettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes,
            keywords)
        {
        }

        /// <inheritdoc/>
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var settings = MornHierarchySettings.instance;
            settings.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            Editor.CreateCachedEditor(settings, null, ref _cachedEditor);
        }

        /// <inheritdoc/>
        public override void OnGUI(string searchContext)
        {
            EditorGUI.BeginChangeCheck();
            _cachedEditor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                MornHierarchySettings.instance.Save();
                EditorApplication.RepaintHierarchyWindow();
            }
        }
    }
}
