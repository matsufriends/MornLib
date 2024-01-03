using System;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;

[assembly: InternalsVisibleTo("MornSetting.Editor")]

namespace MornSetting
{
    public abstract class MornSettingSoBase<T> : ScriptableObject, IMornSettingSo
    {
        [SerializeField] internal string Key;
        [SerializeField] internal T DefaultValue;
        private T _cache;
        private Subject<T> _subject;
        private Subject<T> Subject => _subject ??= new Subject<T>();
        public IObservable<T> OnValueChanged => Subject;

        void IMornSettingSo.SetKey(string key)
        {
            Key = key;
        }

        private void OnEnable()
        {
            _cache = LoadValue(true);
        }

        public T LoadValue(bool forceLoad = false)
        {
            return forceLoad ? LoadValueImpl() : _cache;
        }

        protected abstract T LoadValueImpl();

        public void SaveValue(T value, bool isImmediate = false)
        {
            _cache = value;
            SaveValueImpl(value);
            if (isImmediate)
            {
                PlayerPrefs.Save();
            }

            Subject.OnNext(value);
        }

        protected abstract void SaveValueImpl(T value);
    }
}