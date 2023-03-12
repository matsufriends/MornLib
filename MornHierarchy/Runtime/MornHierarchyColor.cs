using UnityEditor;
using UnityEngine;

namespace MornHierarchy
{
    /// <summary>Hierarchyに色を付けるコンポーネント</summary>
    public sealed class MornHierarchyColor : MonoBehaviour
    {
        /// <summary>背景の色</summary>
        [SerializeField, ColorUsage(false)] private Color _backColor;

        /// <summary>子オブジェクトの背景色にも適用するか</summary>
        [SerializeField] private bool _applyChildren = true;

        /// <summary>背景の色</summary>
        public Color BackColor => _backColor;

        /// <summary>子オブジェクトの背景色にも適用するか</summary>
        public bool ApplyChildren => _applyChildren;
#if UNITY_EDITOR
        /// <inheritdoc/>
        private void OnValidate()
        {
            EditorApplication.RepaintHierarchyWindow();
        }
#endif
    }
}
