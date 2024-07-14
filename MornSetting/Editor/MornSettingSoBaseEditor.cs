using UnityEditor;
using UnityEngine;

namespace MornSetting.MornLib.MornSetting.Editor
{
    [CustomEditor(typeof(MornSettingSoBase<>), true)]
    [CanEditMultipleObjects]
    internal sealed class MornSettingSoBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("File Name -> Scene Name"))
                foreach (var t in targets)
                    ((IMornSettingSo)t).SetKey();
        }
    }
}