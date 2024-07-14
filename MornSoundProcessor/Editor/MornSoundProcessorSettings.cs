using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace MornSoundProcessor
{
    internal sealed class MornSoundProcessorSettings : ScriptableSingleton<MornSoundProcessorSettings>
    {
        [SerializeField] [HideInInspector] private MornSoundProcessorWindow _window;
        [SerializeField] [Header("Input")] private List<AudioClip> _clipList;

        [SerializeField] [Header("CutBeginningSilence")]
        private bool _useCutBeginningSilence = true;

        [SerializeField] [Range(0, 1f)] private float _beginningAmplitude = 0.05f;
        [SerializeField] private int _beginningOffsetSample;

        [SerializeField] [Header("CutEndingSilence")]
        private bool _useCutEndingSilence = true;

        [SerializeField] [Range(0, 1f)] private float _endingAmplitude = 0.001f;
        [SerializeField] private int _endingOffsetSample;

        [SerializeField] [Header("NormalizeAmplitude")]
        private bool _useNormalizeAmplitude = true;

        [SerializeField] [Range(0, 1f)] private float _normalizeAmplitude = 0.8f;
        [SerializeField] [Header("Output")] private string _underAssetsFolderName = "MornSounds/Export";

        [FormerlySerializedAs("_outputList")] [SerializeField]
        private List<AudioClip> _resultList;

        internal IReadOnlyList<AudioClip> ClipList => _clipList;
        internal bool UseCutBeginningSilence => _useCutBeginningSilence;
        internal float BeginningAmplitude => _beginningAmplitude;
        internal int BeginningOffsetSample => _beginningOffsetSample;
        internal bool UseCutEndingSilence => _useCutEndingSilence;
        internal float EndingAmplitude => _endingAmplitude;
        internal int EndingOffsetSample => _endingOffsetSample;
        internal bool UseNormalizeAmplitude => _useNormalizeAmplitude;
        internal float NormalizeAmplitude => _normalizeAmplitude;
        internal string UnderAssetsFolderName => _underAssetsFolderName;

        internal void Init()
        {
            if (_window == null)
            {
                _window = CreateInstance<MornSoundProcessorWindow>();
                _window.titleContent = new GUIContent("MornSoundProcessor");
            }

            _window.Show();
        }

        internal void ClearResult()
        {
            _resultList.Clear();
        }

        internal void AddResult(AudioClip clip)
        {
            _resultList.Add(clip);
        }
    }
}