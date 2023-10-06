using System;
using UniRx;
using UnityEngine;

namespace MornSetting
{
    [CreateAssetMenu(fileName = nameof(MornSettingIntSo), menuName = "MornSetting/" + nameof(MornSettingIntSo))]
    public sealed class MornSettingIntSo : MornSettingSoBase
    {
        [SerializeField] private int _defaultInt;
        private readonly Subject<int> _intSubject = new();
        public IObservable<int> OnIntChanged => _intSubject;

        public int LoadInt()
        {
            return PlayerPrefs.GetInt(Key, _defaultInt);
        }

        public void SaveInt(int value)
        {
            PlayerPrefs.SetInt(Key, value);
            PlayerPrefs.Save();
            _intSubject.OnNext(value);
        }
    }
}