using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornDictionary
{
    [Serializable]
    public struct MornDictionary<TKey, TValue> where TKey : Enum
    {
        [SerializeField] private List<KeyValuePairSet> _pairList;
        private Dictionary<TKey, TValue> _keyToValueDict;

        public TValue this[TKey key] => GetDictionary()[key];

        public Dictionary<TKey, TValue> GetDictionary()
        {
            if (_keyToValueDict != null)
            {
                return _keyToValueDict;
            }

            _keyToValueDict = new();
            for (var i = 0; i < _pairList.Count; i++)
            {
                _keyToValueDict.Add(_pairList[i].Key, _pairList[i].Value);
            }

            return _keyToValueDict;
        }

        [Serializable]
        private struct KeyValuePairSet
        {
            public TKey Key;
            public TValue Value;
        }
    }
}
