using MornEditor;
using MornEnum;
using UnityEditor;
using UnityEngine;

namespace MornLocalize
{
    [InitializeOnLoad]
    public static class MornLocalizeEditor
    {
        static MornLocalizeEditor()
        {
            MornEditorCore.RegisterOnGUI(
                nameof(MornLocalizeEditor),
                () =>
                {
                    foreach (var languageType in MornEnumUtil<MornLocalizeLanguageType>.Values)
                    {
                        var buttonLabel = MornEnumUtil<MornLocalizeLanguageType>.CachedToString(languageType);
                        if (GUILayout.Button(buttonLabel))
                        {
                            var texts = Object.FindObjectsOfType<MornLocalizeTextMono>(true);
                            foreach (var text in texts)
                            {
                                text.ApplyLanguage(languageType);
                                EditorUtility.SetDirty(text);
                            }
                        }
                    }
                });
        }
    }
}