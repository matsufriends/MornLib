using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MornSerializableDictionary
{
    [Serializable]
    public sealed class MornSerializableDictionaryProvider<TKey, TValue>
    {
        [SerializeField] private List<MornKeyValuePair> _list;
        private Dictionary<TKey, TValue> _dict;

        public Dictionary<TKey, TValue> GetDictionary()
        {
            return _dict ??= _list.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        [Serializable]
        private struct MornKeyValuePair
        {
            public TKey Key;
            public TValue Value;
        }

        //MornSerializableDictionaryのEditor拡張
    }
}