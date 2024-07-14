using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MornSetting
{
    public class MornSettingPreviewWindow : EditorWindow
    {
        private readonly List<MornSettingBoolSo> _boolSettings = new();
        private readonly List<MornSettingFloatSo> _floatSettings = new();
        private readonly List<MornSettingIntSo> _intSettings = new();
        private readonly List<MornSettingStringSo> _stringSettings = new();

        private Vector2 _scroll;

        private void OnGUI()
        {
            if (GUILayout.Button("Gather Settings"))
            {
                GatherSettings(_boolSettings);
                GatherSettings(_intSettings);
                GatherSettings(_floatSettings);
                GatherSettings(_stringSettings);
            }

            if (GUILayout.Button("Reset Key"))
            {
                ResetKey(_boolSettings);
                ResetKey(_intSettings);
                ResetKey(_floatSettings);
                ResetKey(_stringSettings);
            }

            using (var scrollScope = new EditorGUILayout.ScrollViewScope(_scroll))
            {
                GUILayout.Label("Bool Settings");
                foreach (var setting in _boolSettings)
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.ObjectField(setting, typeof(MornSettingBoolSo), false);
                        EditorGUILayout.TextField(setting.Key);
                        EditorGUILayout.LabelField(setting.DefaultValue.ToString());
                        EditorGUILayout.LabelField(setting.LoadValue().ToString());
                    }

                GUILayout.Space(15);

                GUILayout.Label("Int Settings");
                foreach (var setting in _intSettings)
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.ObjectField(setting, typeof(MornSettingIntSo), false);
                        EditorGUILayout.TextField(setting.Key);
                        EditorGUILayout.IntField(setting.DefaultValue);
                        EditorGUILayout.IntField(setting.LoadValue());
                    }

                GUILayout.Space(15);

                GUILayout.Label("Float Settings");
                foreach (var setting in _floatSettings)
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.ObjectField(setting, typeof(MornSettingFloatSo), false);
                        EditorGUILayout.TextField(setting.Key);
                        EditorGUILayout.FloatField(setting.DefaultValue);
                        EditorGUILayout.FloatField(setting.LoadValue());
                    }

                GUILayout.Space(15);

                GUILayout.Label("String Settings");
                foreach (var setting in _stringSettings)
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.ObjectField(setting, typeof(MornSettingStringSo), false);
                        EditorGUILayout.TextField(setting.Key);
                        EditorGUILayout.LabelField(setting.DefaultValue);
                        EditorGUILayout.LabelField(setting.LoadValue());
                    }

                GUILayout.Space(15);

                _scroll = scrollScope.scrollPosition;
            }
        }

        [MenuItem("MornLib/" + nameof(MornSettingPreviewWindow))]
        private static void ShowWindow()
        {
            GetWindow<MornSettingPreviewWindow>();
        }

        private void GatherSettings<T>(List<T> list) where T : Object
        {
            list.Clear();
            var settings = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            foreach (var setting in settings)
            {
                var path = AssetDatabase.GUIDToAssetPath(setting);
                var so = AssetDatabase.LoadAssetAtPath<T>(path);
                list.Add(so);
            }
        }

        private void ResetKey<T>(List<T> list) where T : Object
        {
            foreach (var obj in list) ((IMornSettingSo)obj).SetKey();
        }
    }
}