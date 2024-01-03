using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace MornBeat
{
    [CreateAssetMenu(fileName = nameof(MornBeatMemoSo), menuName = "MornBeat/" + nameof(MornBeatMemoSo))]
    public sealed class MornBeatMemoSo : ScriptableObject
    {
        [Serializable]
        private struct BpmAndTimeInfo
        {
            public double Bpm;
            public double Time;
        }

        [SerializeField] private bool _isLoop;
        [SerializeField] private List<float> _timingList;
        [SerializeField] private List<BpmAndTimeInfo> _bpmAndTimeInfoList;
        [SerializeField] private int _measureTickCount = 8;
        [SerializeField] private int _beatCount = 4;
        [SerializeField] private double _interval = 0.000001d;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float volume = 1f;
        [SerializeField] private float _offset;
        public bool IsLoop => _isLoop;
        public int MeasureTickCount => _measureTickCount;
        public int BeatCount => _beatCount;
        public int TickSum => _timingList.Count;
        public AudioClip Clip => _clip;
        public float Volume => volume;
        internal float Offset => _offset;

        internal float GetBeatTiming(int index)
        {
            if (index < 0 || TickSum <= index)
            {
                return Mathf.Infinity;
            }

            return _timingList[index];
        }

        internal void MakeBeat()
        {
            Assert.IsNotNull(_clip);
            var beat = 0d;
            var time = 0d;
            _interval = Math.Max(0.000001f, _interval);
            _timingList.Clear();
            _timingList.Add(0);
            var length = _clip.length;
            while (time < length)
            {
                var bpm = GetBpm(time);
                var dif = bpm / 60 * _measureTickCount / _beatCount * _interval;
                if (Math.Floor(beat) < Math.Floor(beat + dif))
                {
                    _timingList.Add((float)time % length);
                }

                beat += dif;
                time += _interval;
            }

            var remove = _timingList.Count % _measureTickCount;
            for (var i = 0; i < remove; i++)
            {
                _timingList.RemoveAt(_timingList.Count - 1);
            }
        }

        public double GetBpm(double time)
        {
            switch (_bpmAndTimeInfoList.Count)
            {
                case 0:
                    return 60;
                case 1:
                    return _bpmAndTimeInfoList[0].Bpm;
            }

            if (time < _bpmAndTimeInfoList[0].Time)
            {
                return _bpmAndTimeInfoList[0].Bpm;
            }

            for (var i = 1; i < _bpmAndTimeInfoList.Count; i++)
            {
                if (_bpmAndTimeInfoList[i].Time <= time)
                {
                    continue;
                }

                var begin = _bpmAndTimeInfoList[i - 1];
                var end = _bpmAndTimeInfoList[i];
                var t1 = MornBeatUtil.InverseLerp(begin.Time, end.Time, time);
                return MornBeatUtil.Lerp(begin.Bpm, end.Bpm, t1);
            }

            return _bpmAndTimeInfoList[^1].Bpm;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MornBeatMemoSo))]
    internal sealed class BeatMemoSoEditor : Editor
    {
        private MornBeatMemoSo _mornBeatMemo;

        private void OnEnable()
        {
            _mornBeatMemo = (MornBeatMemoSo)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("MakeBeat"))
            {
                _mornBeatMemo.MakeBeat();
            }
        }
    }
#endif
}
