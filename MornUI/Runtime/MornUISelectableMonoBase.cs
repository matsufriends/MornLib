using System;
using UnityEngine;
using UnityEngine.UI;

namespace MornUI
{
    public abstract class MornUISelectableMonoBase : MonoBehaviour
    {
        [Header("Graphic")]
        [SerializeField] protected Graphic _selectableGraphic;
        [SerializeField] private Color _selectedColor = Color.white;
        [SerializeField] private Color _deselectedColor = Color.gray;
        [Header("Navigation")]
        [SerializeField] private MornUISelectableMonoBase _up;
        [SerializeField] private MornUISelectableMonoBase _down;
        [SerializeField] private MornUISelectableMonoBase _right;
        [SerializeField] private MornUISelectableMonoBase _left;

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (_selectableGraphic != null)
            {
                _selectableGraphic.color = _deselectedColor;
            }

            OnValidateImpl();
        }

        protected virtual void OnValidateImpl()
        {
        }

        internal void OnDeselected()
        {
            if (_selectableGraphic != null)
            {
                _selectableGraphic.color = _deselectedColor;
            }

            OnDeselectedImpl();
        }

        protected virtual void OnDeselectedImpl()
        {
        }

        internal void OnSelected()
        {
            if (_selectableGraphic != null)
            {
                _selectableGraphic.color = _selectedColor;
            }

            OnSelectedImpl();
        }

        protected virtual void OnSelectedImpl()
        {
        }

        internal virtual void Transition(MornUIAxisDirType axis)
        {
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
                return;
            }

            MornUICore.SetFocus(element);
        }

        public abstract void Submit();
    }
}
