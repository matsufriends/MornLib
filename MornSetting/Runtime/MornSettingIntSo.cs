using System;
using UniRx;
using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingIntSo), menuName = "MornLib/" + nameof(MornSettingIntSo))]
    public sealed class MornSettingIntSo : MornSettingSoBase
    {
        [SerializeField] private int _defaultInt;
        private bool _hasCache;
        private int _cachedValue;
        private readonly Subject<int> _intSubject = new();
        public IObservable<int> OnIntChanged => _intSubject;

        public int LoadInt(bool useCache = true)
        {
            if (useCache && _hasCache)
            {
                return _cachedValue;
            }

            _hasCache = true;
            _cachedValue = PlayerPrefs.GetInt(Key, _defaultInt);
            return _cachedValue;
        }

        public void SaveInt(int value)
        {
            _hasCache = true;
            _cachedValue = value;
            PlayerPrefs.SetInt(Key, value);
            PlayerPrefs.Save();
            _intSubject.OnNext(value);
        }
    }
}
