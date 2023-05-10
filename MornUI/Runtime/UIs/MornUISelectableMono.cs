using System;
using MornAttribute;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MornUI
{
    public sealed class MornUISelectableMono : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] [ReadOnly] private MornUIBehaviourMonoBase[] _behaviourList;
        [Header("Graphic")]
        [SerializeField] private bool _isChangeEnabled = true;
        [SerializeField] private Graphic _selectedGraphic;
        [SerializeField] [HideIf(nameof(_isChangeEnabled))] private Color _selectedColor = Color.white;
        [SerializeField] [HideIf(nameof(_isChangeEnabled))] private Color _deselectedColor = Color.gray;
        [Header("Navigation")]
        [SerializeField] private MornUISelectableMono _up;
        [SerializeField] private MornUISelectableMono _down;
        [SerializeField] private MornUISelectableMono _right;
        [SerializeField] private MornUISelectableMono _left;
        [SerializeField] private MornUISelectableMono _submit;

        internal void SetBehaviours()
        {
            _behaviourList = GetComponents<MornUIBehaviourMonoBase>();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            SetGraphic(false);
        }

        private void SetGraphic(bool isSelected)
        {
            if (_selectedGraphic == null)
            {
                return;
            }

            if (_isChangeEnabled)
            {
                _selectedGraphic.enabled = isSelected;
            }
            else
            {
                _selectedGraphic.enabled = true;
                _selectedGraphic.color = isSelected ? _selectedColor : _deselectedColor;
            }
        }

        internal void OnDeselected()
        {
            SetGraphic(false);
        }

        internal void OnSelected()
        {
            SetGraphic(true);
            foreach (var behaviour in _behaviourList)
            {
                behaviour.Selected();
            }
        }

        internal bool TryMove(MornUIAxisDirType axis)
        {
            var toTransition = true;
            foreach (var behaviour in _behaviourList)
            {
                behaviour.OnMove(axis, out var canTransition);
                toTransition &= canTransition;
            }

            if (toTransition == false)
            {
                return false;
            }

            var element = axis switch
            {
                MornUIAxisDirType.Up => _up,
                MornUIAxisDirType.Down => _down,
                MornUIAxisDirType.Right => _right,
                MornUIAxisDirType.Left => _left,
                MornUIAxisDirType.None => null,
                _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null),
            };
            if (element == null)
            {
                return false;
            }

            Assert.IsTrue(MornUICore.IsFocused(this));
            MornUICore.SetFocus(element);
            return true;
        }

        internal void OnSubmit()
        {
            var toTransition = true;
            foreach (var behaviour in _behaviourList)
            {
                behaviour.OnSubmit(out var canTransition);
                toTransition &= canTransition;
            }

            if (toTransition && _submit != null)
            {
                Assert.IsTrue(MornUICore.IsFocused(this));
                MornUICore.SetFocus(_submit);
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(MornUISelectableMono))] [CanEditMultipleObjects]
    public sealed class MornUISelectableEditor : Editor
    {
        //SetBehaviours関数を呼ぶボタンを追加する
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("SetBehaviours"))
            {
                foreach (var t in targets)
                {
                    var selectableMono = (MornUISelectableMono)t;
                    selectableMono.SetBehaviours();
                    EditorUtility.SetDirty(selectableMono);
                }
            }
        }
    }
#endif
}
