using System;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;

[assembly: InternalsVisibleTo("MornSetting.Editor")]
namespace MornSetting
{
    public abstract class MornSettingSoBase<T> : ScriptableObject , IMornSettingSo
    {
        [SerializeField] internal string Key;
        [SerializeField] internal T DefaultValue;
        private BehaviorSubject<T> _subject;
        private BehaviorSubject<T> Subject => _subject ??= new BehaviorSubject<T>(DefaultValue);
        public IObservable<T> OnValueChanged => Subject;

        void IMornSettingSo.SetKey(string key)
        {
            Key = key;
        }

        public T LoadValue() => LoadValueImpl();
        protected abstract T LoadValueImpl();

        public void SaveValue(T value)
        {
            SaveValueImpl(value);
            Subject.OnNext(value);
        }

        protected abstract void SaveValueImpl(T value);
    }
}