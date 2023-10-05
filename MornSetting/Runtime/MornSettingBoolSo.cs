using System;
using UniRx;
using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingBoolSo), menuName = "MornSetting/" + nameof(MornSettingBoolSo))]
    public sealed class MornSettingBoolSo : MornSettingSoBase
    {
        [SerializeField] private bool _defaultValue;
        private readonly Subject<bool> _boolSubject = new();
        public IObservable<bool> OnBoolChanged => _boolSubject;

        public bool LoadBool()
        {
            var value = PlayerPrefs.GetInt(Key, -1);
            return value < 0 ? _defaultValue : value > 0;
        }

        public void SaveBool(bool value)
        {
            PlayerPrefs.SetInt(Key, value ? 1 : 0);
            PlayerPrefs.Save();
            _boolSubject.OnNext(value);
        }
    }
}