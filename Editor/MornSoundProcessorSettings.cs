using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MornLib.Editor
{
    public class MornSoundProcessorSettings : ScriptableSingleton<MornSoundProcessorSettings>
    {
        [HideInInspector] public MornSoundProcessorWindow Window;

        [Header("Input")] //Input
        public List<AudioClip> ClipList;

        [Header("CutBeginningSilence")] //CutBeginning
        public bool IsCutBeginningSilence = true;

        [Range(0, 1f)] public float BeginningAmplitude = 0.01f;
        public int BeginningOffsetSample;

        [Header("CutEndingSilence")] //CutEnding
        public bool IsCutEndingSilence = true;

        [Range(0, 1f)] public float EndingAmplitude = 0.001f;
        public int EndingOffsetSample;

        [Header("NormalizeAmplitude")] //NormalizeAmplitude
        public bool IsNormalizeAmplitude = true;

        [Range(0, 1f)] public float NormalizeAmplitude = 0.8f;

        [Header("Output")] //Output
        public bool IsSaveInUnderAssetsFolder = true;

        public string UnderAssetsFolderName = "MornSounds";
        public List<AudioClip> OutputList;
    }
}