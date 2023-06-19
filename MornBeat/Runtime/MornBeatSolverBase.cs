using System;
using MornDictionary;
using UnityEngine;

namespace MornBeat
{
    public abstract class MornBeatSolverBase<TBeatType> : MonoBehaviour where TBeatType : Enum
    {
        [SerializeField] private MornDictionary<TBeatType, MornBeatMemoSo> _beatDictionary;
        private static MornBeatSolverBase<TBeatType> s_instance;
        public static MornBeatSolverBase<TBeatType> Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                s_instance = FindObjectOfType<MornBeatSolverBase<TBeatType>>();
                if (s_instance == null)
                {
                    Debug.LogError($"{nameof(MornBeatSolverBase<TBeatType>)} is not found.");
                }

                return s_instance;
            }
        }

        internal MornBeatMemoSo this[TBeatType beatType] => _beatDictionary[beatType];

        public abstract float PlayingTime { get; }
        public abstract void OnInitialized(TBeatType beatType, AudioClip audioClip);
    }
}
