﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornBeat
{
    public interface IMornBeatActionSettingSo
    {
        ValueTuple<Enum, string, Color>[] DisplayTuples { get; }
        Dictionary<int, MornBeatAction<TEnum>> GetDictionary<TEnum>() where TEnum : Enum;
    }

    public abstract class MornBeatActionSettingSoBase<TEnum> : ScriptableObject, IMornBeatActionSettingSo where TEnum : Enum
    {
        [SerializeField] private int _measureTick;
        [SerializeField] private List<MornBeatAction<TEnum>> _beatAction;

        public int GetMeasureTick()
        {
            return _measureTick;
        }

        public void OverrideBeatAction(List<MornBeatAction<TEnum>> beatAction)
        {
            _beatAction = beatAction;
        }

        Dictionary<int, MornBeatAction<T>> IMornBeatActionSettingSo.GetDictionary<T>()
        {
            var dict = new Dictionary<int, MornBeatAction<T>>();
            foreach (var beatAction in _beatAction)
            {
                var totalTick = beatAction.Measure * _measureTick + beatAction.Tick;
                if (dict.ContainsKey(totalTick))
                {
                    Debug.LogWarning($"重複しているTick{totalTick}があります。");
                }

                //dict.Add(totalTick, beatAction);
                var beatType = (T)(object)beatAction.BeatActionType;
                dict.Add(totalTick, new MornBeatAction<T>(beatAction.Measure, beatAction.Tick, beatType));
            }

            return dict;
        }

        public Dictionary<int, MornBeatAction<TEnum>> GenerateDictionary()
        {
            var dict = new Dictionary<int, MornBeatAction<TEnum>>();
            foreach (var beatAction in _beatAction)
            {
                var totalTick = beatAction.Measure * _measureTick + beatAction.Tick;
                if (dict.ContainsKey(totalTick))
                {
                    Debug.LogWarning($"重複しているTick{totalTick}があります。");
                }

                dict.Add(totalTick, beatAction);
            }

            return dict;
        }

        public List<(int, MornBeatAction<TEnum>)> GenerateList()
        {
            var list = new List<(int, MornBeatAction<TEnum>)>();
            foreach (var beatAction in _beatAction)
            {
                list.Add((beatAction.Measure * _measureTick + beatAction.Tick, beatAction));
            }

            return list;
        }

        public bool TryGetAction(int index, out MornBeatAction<TEnum> beatAction)
        {
            if (index < 0 || index >= _beatAction.Count)
            {
                beatAction = default(MornBeatAction<TEnum>);
                return false;
            }

            beatAction = _beatAction[index];
            return true;
        }

        public abstract ValueTuple<Enum, string, Color>[] DisplayTuples { get; }
    }
}