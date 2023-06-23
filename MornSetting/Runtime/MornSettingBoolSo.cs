using System;
using UniRx;
using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingBoolSo), menuName = "MornLib/" + nameof(MornSettingBoolSo))]
    public sealed class MornSettingBoolSo : MornSettingSoBase
    {
        [SerializeField] private bool _defaultValue;
        private bool _hasCache;
        private bool _cachedValue;
        private readonly Subject<bool> _boolSubject = new();
        public IObservable<bool> OnBoolChanged => _boolSubject;

        public bool LoadBool(bool useCache = true)
        {
            if (useCache && _hasCache)
            {
                return _cachedValue;
            }

            _hasCache = true;
            var value = PlayerPrefs.GetInt(Key, -1);
            _cachedValue = value < 0 ? _defaultValue : value > 0;
            return _cachedValue;
        }

        public void SaveBool(bool value)
        {
            _hasCache = true;
            _cachedValue = value;
            PlayerPrefs.SetInt(Key, value ? 1 : 0);
            PlayerPrefs.Save();
            _boolSubject.OnNext(value);
        }
    }
}
