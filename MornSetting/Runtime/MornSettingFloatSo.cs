using System;
using UniRx;
using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingFloatSo), menuName = "MornLib/" + nameof(MornSettingFloatSo))]
    public sealed class MornSettingFloatSo : MornSettingSoBase
    {
        [SerializeField] private float _defaultFloat;
        private bool _hasCache;
        private float _cachedValue;
        private readonly Subject<float> _floatSubject = new();
        public IObservable<float> OnFloatChanged => _floatSubject;

        public float LoadFloat(bool useCache = true)
        {
            if (useCache && _hasCache)
            {
                return _cachedValue;
            }

            _hasCache = true;
            _cachedValue = PlayerPrefs.GetFloat(Key, _defaultFloat);
            return _cachedValue;
        }

        public void SaveFloat(float value)
        {
            _hasCache = true;
            _cachedValue = value;
            PlayerPrefs.SetFloat(Key, value);
            PlayerPrefs.Save();
            _floatSubject.OnNext(value);
        }
    }
}
