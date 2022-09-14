using MornLib.Cores;
using UnityEditor;
using UnityEngine;
namespace MornLib.Editor {
    public sealed class MornSoundProcessorWindow : EditorWindow {
        private static UnityEditor.Editor s_editor;
        [MenuItem("Morn/SoundProcessor")]
        private static void Open() {
            Init();
        }
        private static void Init() {
            var instance = MornSoundProcessorSettings.instance;
            if(instance.Window == null) {
                instance.Window = CreateInstance<MornSoundProcessorWindow>();
            }
            instance.Window.Show();
            instance.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
            UnityEditor.Editor.CreateCachedEditor(instance,null,ref s_editor);
        }
        private void OnGUI() {
            if(s_editor == null) Init();
            EditorGUI.BeginChangeCheck();
            s_editor.OnInspectorGUI();
            var instance = MornSoundProcessorSettings.instance;
            if((instance.IsCutBeginning || instance.IsNormalizeVolume) && GUILayout.Button("Generate")) {
                var length = instance.ClipList.Count;
                for(var i = 0;i < length;i++) {
                    var clip = instance.ClipList[i];
                    EditorUtility.DisplayProgressBar("変換中",clip.name,i * 1f / length);
                    SaveClip(ConvertClip(clip));
                }
                EditorUtility.ClearProgressBar();
                Debug.Log($"{length}件の変換が終わりました");
                AssetDatabase.Refresh(ImportAssetOptions.Default);
            }
        }
        private static AudioClip ConvertClip(AudioClip clip) {
            var instance = MornSoundProcessorSettings.instance;
            if(instance.IsCutBeginning && instance.IsNormalizeVolume) {
                var newClip = MornSoundProcessor.CutBeginningSilence(clip,instance.CutAmplitude);
                newClip = MornSoundProcessor.NormalizeAmplitude(newClip,instance.NormalizeVolume);
                return newClip;
            } else if(instance.IsCutBeginning) {
                return MornSoundProcessor.CutBeginningSilence(clip,instance.CutAmplitude);
            } else if(instance.IsNormalizeVolume) {
                return MornSoundProcessor.NormalizeAmplitude(clip,instance.NormalizeVolume);
            }
            return clip;
        }
        private static void SaveClip(AudioClip clip) {
            var instance = MornSoundProcessorSettings.instance;
            if(instance.IsSaveInFolder) {
                if(AssetDatabase.IsValidFolder($"Assets/{instance.FolderName}") == false) {
                    AssetDatabase.CreateFolder("Assets",instance.FolderName);
                }
                var path = $"Assets/{instance.FolderName}/{clip.name}_Converted.wav";
                MornSoundProcessor.SaveAudioClipToWave(clip,path);
            } else {
                var path = AssetDatabase.GetAssetPath(clip);
                var lastIndex = path.LastIndexOf('.');
                MornSoundProcessor.SaveAudioClipToWave(clip,$"{path.Substring(0,lastIndex)}_Converted.wav");
            }
        }
    }
}