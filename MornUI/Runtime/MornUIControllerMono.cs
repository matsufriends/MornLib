using System.Collections.Generic;
using UnityEngine;

namespace MornUI
{
    public sealed class MornUIControllerMono : MonoBehaviour
    {
        [SerializeField] private MornUIMonoBase _firstFocus;
        private HashSet<MornUIMonoBase> _uis;
        private MornUIMonoBase _currentFocus;
        private MornUIDirType _preDir;
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
        }

        public void RegisterInitialInput(Vector2 input)
        {
            _lockDir = input.ToDir();
        }

        public void ResetFocus()
        {
            ChangeFocus(_firstFocus, true);
        }

        private void ChangeFocus(MornUIMonoBase focus, bool isInitial)
        {
            if (_currentFocus == focus)
            {
                return;
            }

            foreach (var ui in _uis)
            {
                if (ui == _currentFocus)
                {
                    ui.OnUnFocus(isInitial);
                }
                else if (ui == focus)
                {
                    ui.OnFocus(isInitial);
                }
            }

            _currentFocus = focus;
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
                    ChangeFocus(nextFocus, false);
                }
            }

            _lastInputTime = Time.realtimeSinceStartup;
        }

        public void PanelUpdate(Vector2 input)
        {
            var curDir = input.ToDir();
            if (_lockDir != MornUIDirType.None && curDir == _lockDir)
            {
                return;
            }

            _lockDir = MornUIDirType.None;
            if (_preDir != curDir)
            {
                _nextCanMoveTime = 0;
            }

            if (curDir != MornUIDirType.None && _nextCanMoveTime <= Time.realtimeSinceStartup)
            {
                _currentFocus.OnMove(input, out var nextFocus);
                if (nextFocus != null)
                {
                    ChangeFocus(nextFocus, false);
                    var dif = curDir != _preDir ? ChangeFocusInitInterval : ChangeFocusInterval;
                    _nextCanMoveTime = Time.realtimeSinceStartup + dif;
                    _lastInputTime = Time.realtimeSinceStartup;
                }
            }

            _preDir = curDir;
        }
    }
}