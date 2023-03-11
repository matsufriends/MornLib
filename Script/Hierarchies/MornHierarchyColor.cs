using UnityEditor;
using UnityEngine;

namespace MornLib.Hierarchies
{
    public class MornHierarchyColor : MonoBehaviour
    {
        [ColorUsage(false)] public Color BackColor;
        public bool ApplyChildren;
#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.RepaintHierarchyWindow();
        }
#endif
    }
}
