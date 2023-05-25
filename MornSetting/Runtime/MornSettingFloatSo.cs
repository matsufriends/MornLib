using System;
using UniRx;
using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingFloatSo), menuName = "MornLib/" + nameof(MornSettingFloatSo))]
    public sealed class MornSettingFloatSo : MornSettingSoBase
    {
        [SerializeField] private float _defaultFloat;
        private readonly Subject<float> _floatSubject = new();
        public IObservable<float> OnFloatChanged => _floatSubject;
        public float LoadFloat()
        {
            return PlayerPrefs.GetFloat(Key, _defaultFloat);
        }
        public void SaveFloat(float value)
        {
            PlayerPrefs.SetFloat(Key, value);
            PlayerPrefs.Save();
            _floatSubject.OnNext(value);
        }
    }
}
