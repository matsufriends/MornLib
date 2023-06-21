using System;
using MornDictionary;
using UnityEngine;

namespace MornBeat
{
    public abstract class MornBeatSolverMonoBase<TBeatType> : MonoBehaviour where TBeatType : Enum
    {
        [Header("MakeBeat")] [SerializeField] private MornDictionary<TBeatType, MornBeatMemoSo> _beatDictionary;
        private static MornBeatSolverMonoBase<TBeatType> s_instance;

        internal MornBeatMemoSo this[TBeatType beatType] => _beatDictionary[beatType];

        internal static MornBeatSolverMonoBase<TBeatType> Instance
        {
            get
            {
                if (s_instance != null)
                {
                    return s_instance;
                }

                s_instance = FindObjectOfType<MornBeatSolverMonoBase<TBeatType>>();
                if (s_instance == null)
                {
                    Debug.LogError($"{nameof(MornBeatSolverMonoBase<TBeatType>)} is not found.");
                }

                return s_instance;
            }
        }
        internal float MusicPlayingTimeImpl => MusicPlayingTime;

        internal void OnInitializeBeatImpl(TBeatType beatType)
        {
            OnInitializedBeat(beatType, _beatDictionary[beatType].Clip, _beatDictionary[beatType].IsLoop);
        }

        private void OnDestroy()
        {
            MornBeatCore.Reset();
        }

        protected abstract float MusicPlayingTime { get; }
        protected abstract void OnInitializedBeat(TBeatType beatType, AudioClip clip, bool isLoop);
    }
}
