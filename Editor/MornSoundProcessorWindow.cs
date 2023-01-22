using MornLib.Extensions;
using UnityEditor;
using UnityEngine;

namespace MornLib.Editor
{
    public sealed class MornSoundProcessorWindow : EditorWindow
    {
        private static UnityEditor.Editor s_editor;

        [MenuItem("Morn/SoundProcessor")]
        private static void Open()
        {
            Init();
        }

        private static void Init()
        {
            var instance = MornSoundProcessorSettings.instance;
            if (instance.Window == null)
            {
                instance.Window = CreateInstance<MornSoundProcessorWindow>();
            }

            instance.Window.Show();
            instance.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            UnityEditor.Editor.CreateCachedEditor(instance, null, ref s_editor);
        }

        private void OnGUI()
        {
            if (s_editor == null)
            {
                Init();
            }

            EditorGUI.BeginChangeCheck();
            s_editor.OnInspectorGUI();
            var instance = MornSoundProcessorSettings.instance;
            if ((instance.IsCutBeginningSilence || instance.IsNormalizeAmplitude) && GUILayout.Button("Generate"))
            {
                var length = instance.ClipList.Count;
                instance.ClearOutput();
                for (var i = 0; i < length; i++)
                {
                    var clip = instance.ClipList[i];
                    EditorUtility.DisplayProgressBar("変換中", clip.name, i * 1f / length);
                    instance.AddOutput(SaveClip(ConvertClip(clip)));
                }

                EditorUtility.ClearProgressBar();
                Debug.Log($"{length}件の変換が終わりました");
            }
        }

        private static AudioClip ConvertClip(AudioClip clip)
        {
            var instance = MornSoundProcessorSettings.instance;
            if (instance.IsCutBeginningSilence)
            {
                clip = CutBeginningSilence(clip);
            }

            if (instance.IsCutEndingSilence)
            {
                clip = CutEndingSilence(clip);
            }

            if (instance.IsNormalizeAmplitude)
            {
                clip = Normalize(clip);
            }

            return clip;
        }

        private static AudioClip CutBeginningSilence(AudioClip clip)
        {
            var instance = MornSoundProcessorSettings.instance;
            return MornSoundProcessor.CutBeginningSilence(clip, instance.BeginningOffsetSample, instance.BeginningAmplitude);
        }

        private static AudioClip CutEndingSilence(AudioClip clip)
        {
            var instance = MornSoundProcessorSettings.instance;
            return MornSoundProcessor.CutEndSilence(clip, instance.EndingOffsetSample, instance.EndingAmplitude);
        }

        private static AudioClip Normalize(AudioClip clip)
        {
            var instance = MornSoundProcessorSettings.instance;
            return MornSoundProcessor.NormalizeAmplitude(clip, instance.NormalizeAmplitude);
        }

        private static AudioClip SaveClip(AudioClip clip)
        {
            var instance = MornSoundProcessorSettings.instance;
            var dirs = instance.UnderAssetsFolderName.Split('/');
            var combinePath = "Assets";
            foreach (var dir in dirs)
            {
                if (AssetDatabase.IsValidFolder($"{combinePath}/{dir}") == false)
                {
                    AssetDatabase.CreateFolder(combinePath, dir);
                    Debug.Log($"フォルダー {combinePath}/{dir} を作成しました");
                }

                combinePath += $"/{dir}";
            }

            var path = $"Assets/{instance.UnderAssetsFolderName}/{clip.name}_Converted.wav";
            MornSoundProcessor.SaveAudioClipToWave(clip, path);
            AssetDatabase.Refresh(ImportAssetOptions.Default);
            return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        }
    }
}
