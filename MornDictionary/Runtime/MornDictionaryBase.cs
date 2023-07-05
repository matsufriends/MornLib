using System;
using System.Collections.Generic;
using UnityEngine;

namespace MornDictionary
{
    public abstract class MornDictionaryBase<TKey, TValue> : MonoBehaviour where TKey : Enum
    {
        [SerializeField] private List<TKey> _keyList;
        [SerializeField] private List<TValue> _valueList;
        private Dictionary<TKey, TValue> _keyToValueDict;

        public TValue this[TKey key] => GetDictionary()[key];

        public Dictionary<TKey, TValue> GetDictionary()
        {
            if (_keyToValueDict != null)
            {
                return _keyToValueDict;
            }

            _keyToValueDict = new Dictionary<TKey, TValue>();
            for (var i = 0; i < _keyList.Count; i++)
            {
                _keyToValueDict.Add(_keyList[i], _valueList[i]);
            }

            return _keyToValueDict;
        }
    }
}
