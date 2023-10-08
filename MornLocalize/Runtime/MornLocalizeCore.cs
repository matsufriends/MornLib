using System;
using UniRx;

namespace MornLocalize
{
    public static class MornLocalizeCore
    {
        private static readonly ReactiveProperty<MornLocalizeLanguageType> _languageChangedRp = new();
        public static IObservable<MornLocalizeLanguageType> OnLanguageChanged => _languageChangedRp;

        public static void ChangeLanguage(MornLocalizeLanguageType mornLocalizeLanguageType)
        {
            _languageChangedRp.Value = mornLocalizeLanguageType;
        }
    }
}