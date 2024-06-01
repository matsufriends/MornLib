using System;
using System.IO;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[assembly: InternalsVisibleTo("MornSetting.Editor")]
namespace MornSetting
{
    public abstract class MornSettingSoBase<T> : ScriptableObject, IMornSettingSo
    {
        [SerializeField] internal string Key;
        [SerializeField] internal T DefaultValue;
        private T _cache;
        private BehaviorSubject<T> _subject;
        private BehaviorSubject<T> Subject
        {
            get
            {
                if (_subject != null)
                {
                    return _subject;
                }

                _subject = new BehaviorSubject<T>(LoadValue(true));
                return _subject;
            }
        }
        public IObservable<T> OnValueChanged => Subject;

        void IMornSettingSo.SetKey(string key)
        {
            Key = key;
        }

        void IMornSettingSo.SetKey()
        {
#if UNITY_EDITOR
            var assetPath = AssetDatabase.GetAssetPath(this);
            assetPath = assetPath.Replace($"Assets/SaveData/", "");
            assetPath = Path.ChangeExtension(assetPath, null);
            Key = assetPath;
            EditorUtility.SetDirty(this);
#endif
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