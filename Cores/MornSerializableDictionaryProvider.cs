using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MornLib.Cores {
    [System.Serializable]
    public class MornSerializableDictionaryProvider<TKey,TValue> {
        [SerializeField] private List<MornKeyValuePair> _list;
        private Dictionary<TKey,TValue> _dict;

        public Dictionary<TKey,TValue> GetDictionary() {
            return _dict ??= _list.ToDictionary(pair => pair.Key,pair => pair.Value);
        }

        [System.Serializable]
        private struct MornKeyValuePair {
            public TKey Key;
            public TValue Value;
        }
        public TValue this[TKey key] {
            get => GetDictionary()[key];
            set => GetDictionary()[key] = value;
        }
    }
}
