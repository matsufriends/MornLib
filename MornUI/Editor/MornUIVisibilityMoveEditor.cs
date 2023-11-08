using UnityEditor;
using UnityEngine;

namespace MornUI
{
    [CustomEditor(typeof(MornUIVisibilityMonoBase), true)]
    public sealed class MornUIVisibilityMoveEditor : Editor
    {
        private MornUIVisibilityMonoBase _visibility;

        private void OnEnable()
        {
            _visibility = (MornUIVisibilityMonoBase)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Show"))
            {
                _visibility.Show(true);
                EditorUtility.SetDirty(_visibility);
            }

            if (GUILayout.Button("Hide"))
            {
                _visibility.Hide(true);
                EditorUtility.SetDirty(_visibility);
            }
        }
    }
}