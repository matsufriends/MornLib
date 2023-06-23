using System;
using UniRx;
using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingStringSo), menuName = "MornLib/" + nameof(MornSettingStringSo))]
    public sealed class MornSettingStringSo : MornSettingSoBase
    {
        [SerializeField] private string _defaultString;
        private readonly Subject<string> _stringSubject = new();
        private bool _hasCache;
        private string _cachedValue;
        public IObservable<string> OnStringChanged => _stringSubject;

        public string LoadString(bool useCache)
        {
            if (useCache && _hasCache)
            {
                return _cachedValue;
            }

            _hasCache = true;
            _cachedValue = PlayerPrefs.GetString(Key, _defaultString);
            return _cachedValue;
        }

        public void SaveFloat(string value)
        {
            _hasCache = true;
            _cachedValue = value;
            PlayerPrefs.SetString(Key, value);
            PlayerPrefs.Save();
            _stringSubject.OnNext(value);
        }
    }
}
