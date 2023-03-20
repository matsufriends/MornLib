using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MornHierarchy
{
    internal sealed class MornHierarchySettingsProvider : SettingsProvider
    {
        private Editor _cachedEditor;
        private const string SettingPath = "Preferences/MornHierarchy";

        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            return new MornHierarchySettingsProvider(SettingPath, SettingsScope.User, null);
        }

        private MornHierarchySettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes,
            keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var settings = MornHierarchySettings.instance;
            settings.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            Editor.CreateCachedEditor(settings, null, ref _cachedEditor);
        }

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
