using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace MornLib.Editor {
    public class MornSoundProcessorSettings : ScriptableSingleton<MornSoundProcessorSettings> {
        [HideInInspector] public MornSoundProcessorWindow Window;
        [Space] public List<AudioClip> ClipList;
        public bool IsCutBeginning = true;
        [Range(0,1f)] public float CutAmplitude = 0.1f;
        [Space] public bool IsNormalizeVolume = true;
        [Range(0,1f)] public float NormalizeVolume = 0.8f;
        [Space] public bool IsSaveInFolder = true;
        public string FolderName = "MornSounds";
    }
}