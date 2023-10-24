using System.Collections.Generic;
using UnityEngine;

namespace MornUI
{
    public sealed class MornUIPanelMono : MonoBehaviour
    {
        [SerializeField] private MornUIMonoBase _firstFocus;
        private HashSet<MornUIMonoBase> _uis;
        private MornUIMonoBase _currentFocus;
        private MornUIDirType _preMornUIDir;
        private MornUIDirType _lockDir;
        private float _lastInputTime;
        private float _nextCanMoveTime;
        private const float SubmitInterval = 0.1f;
        private const float ChangeFocusInitInterval = 0.3f;
        private const float ChangeFocusInterval = 0.15f;

        private void Awake()
        {
            _uis = new HashSet<MornUIMonoBase>();
            var tryAddQueue = new Queue<MornUIMonoBase>();
            tryAddQueue.Enqueue(_firstFocus);
            while (tryAddQueue.Count > 0)
            {
                var ui = tryAddQueue.Dequeue();
                if (ui == null || !_uis.Add(ui))
                {
                    continue;
                }

                ui.AddSurround(tryAddQueue.Enqueue);
            }

            Initialize(Vector2.zero);
        }

        public void Initialize(Vector2 input)
        {
            ChangeFocus(_firstFocus);
            _lockDir = input.ToDir();
        }

        private void ChangeFocus(MornUIMonoBase focus)
        {
            _currentFocus = focus;
            foreach (var ui in _uis)
            {
                if (ui == _currentFocus)
                {
                    ui.OnFocus();
                }
                else
                {
                    ui.OnUnFocus();
                }
            }
        }

        private bool CanInput()
        {
            return _lastInputTime + SubmitInterval <= Time.realtimeSinceStartup;
        }

        public void OnSubmit()
        {
            if (!CanInput())
            {
                return;
            }

            _currentFocus.OnSubmit();
            _lastInputTime = Time.realtimeSinceStartup;
        }

        public void OnCancel()
        {
            if (!CanInput())
            {
                return;
            }

            _currentFocus.OnCancel(out var nextFocus);
            if (nextFocus != null)
            {
                if (_currentFocus == nextFocus)
                {
                    OnSubmit();
                }
                else
                {
                    ChangeFocus(nextFocus);
                }
            }

            _lastInputTime = Time.realtimeSinceStartup;
        }

        public void PanelUpdate(Vector2 input)
        {
            var curDir = input.ToDir();
            if (curDir == _lockDir)
            {
                return;
            }

            _lockDir = MornUIDirType.None;
            if (_preMornUIDir != curDir)
            {
                _nextCanMoveTime = 0;
            }

            if (curDir != MornUIDirType.None && _nextCanMoveTime <= Time.realtimeSinceStartup)
            {
                _currentFocus.OnMove(input, out var nextFocus);
                if (nextFocus != null)
                {
                    ChangeFocus(nextFocus);
                    var dif = curDir != _preMornUIDir ? ChangeFocusInitInterval : ChangeFocusInterval;
                    _nextCanMoveTime = Time.realtimeSinceStartup + dif;
                    _lastInputTime = Time.realtimeSinceStartup;
                }
            }

            _preMornUIDir = curDir;
        }
    }
}