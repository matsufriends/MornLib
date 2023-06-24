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
        public IObservable<string> OnStringChanged => _stringSubject;

        public string LoadString()
        {
            return PlayerPrefs.GetString(Key, _defaultString);
        }

        public void SaveFloat(string value)
        {
            PlayerPrefs.SetString(Key, value);
            PlayerPrefs.Save();
            _stringSubject.OnNext(value);
        }
    }
}
