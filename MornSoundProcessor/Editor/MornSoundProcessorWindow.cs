using UnityEditor;
using UnityEngine;

namespace MornSoundProcessor
{
    internal sealed class MornSoundProcessorWindow : EditorWindow
    {
        private static Editor s_editor;

        [MenuItem("MornLib/MornSoundProcessor")]
        private static void Open()
        {
            Init();
        }

        private static void Init()
        {
            var instance = MornSoundProcessorSettings.instance;
            instance.Init();
            instance.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            Editor.CreateCachedEditor(instance, null, ref s_editor);
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
            if ((instance.UseCutBeginningSilence || instance.UseNormalizeAmplitude) && GUILayout.Button("Generate"))
            {
                var length = instance.ClipList.Count;
                instance.ClearResult();
                for (var i = 0; i < length; i++)
                {
                    var clip = instance.ClipList[i];
                    EditorUtility.DisplayProgressBar("変換中", clip.name, i * 1f / length);
                    instance.AddResult(SaveClip(ConvertClip(clip)));
                }

                EditorUtility.ClearProgressBar();
                Debug.Log($"{length}件の変換が終わりました");
            }
        }

        private static AudioClip ConvertClip(AudioClip clip)
        {
            var instance = MornSoundProcessorSettings.instance;
            if (instance.UseCutBeginningSilence)
            {
                clip = CutBeginningSilence(clip);
            }

            if (instance.UseCutEndingSilence)
            {
                clip = CutEndingSilence(clip);
            }

            if (instance.UseNormalizeAmplitude)
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
            return MornSoundProcessor.CutEndingSilence(clip, instance.EndingOffsetSample, instance.EndingAmplitude);
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
            MornSoundProcessor.SaveAudioClipToWav(clip, path);
            AssetDatabase.Refresh(ImportAssetOptions.Default);
            return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        }
    }
}