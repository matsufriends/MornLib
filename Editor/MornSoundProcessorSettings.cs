using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MornLib.Editor
{
    public class MornSoundProcessorSettings : ScriptableSingleton<MornSoundProcessorSettings>
    {
        [HideInInspector] public MornSoundProcessorWindow Window;
        [Header("Input"), SerializeField] private List<AudioClip> _clipList;
        [Header("CutBeginningSilence"), SerializeField] private bool _isCutBeginningSilence = true;
        [Range(0, 1f), SerializeField] private float _beginningAmplitude = 0.05f;
        [SerializeField] private int _beginningOffsetSample;
        [Header("CutEndingSilence"), SerializeField] private bool _isCutEndingSilence = true;
        [Range(0, 1f), SerializeField] private float _endingAmplitude = 0.001f;
        [SerializeField] private int _endingOffsetSample;
        [Header("NormalizeAmplitude"), SerializeField] private bool _isNormalizeAmplitude = true;
        [Range(0, 1f), SerializeField] private float _normalizeAmplitude = 0.8f;
        [Header("Output"), SerializeField] private string _underAssetsFolderName = "MornSounds/Export";
        [SerializeField] private List<AudioClip> _outputList;
        public List<AudioClip> ClipList => _clipList;
        public bool IsCutBeginningSilence => _isCutBeginningSilence;
        public float BeginningAmplitude => _beginningAmplitude;
        public int BeginningOffsetSample => _beginningOffsetSample;
        public bool IsCutEndingSilence => _isCutEndingSilence;
        public float EndingAmplitude => _endingAmplitude;
        public int EndingOffsetSample => _endingOffsetSample;
        public bool IsNormalizeAmplitude => _isNormalizeAmplitude;
        public float NormalizeAmplitude => _normalizeAmplitude;
        public string UnderAssetsFolderName => _underAssetsFolderName;

        public void ClearOutput()
        {
            _outputList.Clear();
        }

        public void AddOutput(AudioClip clip)
        {
            _outputList.Add(clip);
        }
    }
}
