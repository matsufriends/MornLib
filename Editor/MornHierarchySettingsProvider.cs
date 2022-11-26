using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MornLib.Editor
{
    public sealed class MornHierarchySettingsProvider : SettingsProvider
    {
        private UnityEditor.Editor _editor;
        private const string _settingPath = "Project/MornHierarchy";

        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            return new MornHierarchySettingsProvider(_settingPath, SettingsScope.Project, null);
        }

        private MornHierarchySettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(
            path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var preferences = MornHierarchySettings.instance;
            preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            UnityEditor.Editor.CreateCachedEditor(preferences, null, ref _editor);
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUI.BeginChangeCheck();
            _editor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                MornHierarchySettings.instance.Save();
                EditorApplication.RepaintHierarchyWindow();
            }
        }
    }
}