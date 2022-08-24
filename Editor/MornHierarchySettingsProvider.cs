using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace MornLib.Editor {
    public sealed class MornHierarchySettingsProvider : SettingsProvider {
        private UnityEditor.Editor _editor;
        private const string _settingPath = "Project/MornHierarchy";
        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider() {
            return new MornHierarchySettingsProvider(_settingPath,SettingsScope.Project,null);
        }
        private MornHierarchySettingsProvider(string path,SettingsScope scopes,IEnumerable<string> keywords) : base(path,scopes,keywords) { }
        public override void OnActivate(string searchContext,VisualElement rootElement) {
            var preferences = MornHierarchySettings.instance;
            preferences.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable; // ScriptableSingletonを編集可能にする（本文で補足）
            // 設定ファイルの標準のインスペクターのエディタを生成
            UnityEditor.Editor.CreateCachedEditor(preferences,null,ref _editor);
        }
        public override void OnGUI(string searchContext) {
            EditorGUI.BeginChangeCheck();
            // 設定ファイルの標準のインスペクターを表示
            _editor.OnInspectorGUI();
            if(EditorGUI.EndChangeCheck()) {
                // 差分があったら保存
                MornHierarchySettings.instance.Save();
            }
        }
    }
}