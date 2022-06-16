using System.Collections.Generic;
using UnityEngine;
namespace MornLib.Cores {
    [System.Serializable]
    public class MornDictionaryForSerializeField<TKey,TValue> {
        [SerializeField] private List<MornKeyValuePair> _list;
        private Dictionary<TKey,TValue> _dictionary;
        public Dictionary<TKey,TValue> GetDictionary() {
            if(_dictionary != null) return _dictionary;
            _dictionary = new Dictionary<TKey,TValue>();
            foreach(var pairs in _list) {
                if(_dictionary.TryAdd(pairs.Key,pairs.Value) == false) {
                    MornLog.Error($"Key:{pairs.Key} が重複しています");
                }
            }
            return _dictionary;
        }
        [System.Serializable]
        private struct MornKeyValuePair {
            public TKey Key;
            public TValue Value;
        }
    }
}