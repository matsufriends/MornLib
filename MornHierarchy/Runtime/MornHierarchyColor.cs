using UnityEditor;
using UnityEngine;

namespace MornHierarchy
{
    public sealed class MornHierarchyColor : MonoBehaviour
    {
        [SerializeField] [ColorUsage(false)] private Color _backColor;
        [SerializeField] private bool _applyChildren = true;
        public Color BackColor => _backColor;
        public bool ApplyChildren => _applyChildren;
#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.RepaintHierarchyWindow();
        }
#endif
    }
}