using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace MornUI
{
    public static class MornUICore
    {
        private static InputActionReference s_moveAction;
        private static InputActionReference s_submitAction;
        private static bool s_isLocked;
        private static readonly MornUISolverMono s_solver;
        private static float s_cachedMoveTime;
        private static bool s_isFastMove;
        private static MornUIAxisDirType s_cachedAxisDir;

        private static MornUISelectableMonoBase Current
        {
            get => s_solver._current;
            set => s_solver._current = value;
        }

        static MornUICore()
        {
            var solvers = Object.FindObjectsOfType<MornUISolverMono>();
            if (solvers.Length == 0)
            {
                s_solver = new GameObject(nameof(MornUISolverMono)).AddComponent<MornUISolverMono>();
                Object.DontDestroyOnLoad(s_solver.gameObject);
            }
            else
            {
                for (var i = 1; i < solvers.Length; i++)
                {
                    Object.DestroyImmediate(solvers[i].gameObject);
                }

                s_solver = solvers[0];
                Object.DontDestroyOnLoad(s_solver.gameObject);
            }
        }

        public static void SetFocus(MornUISelectableMonoBase selectable)
        {
            if (Current != null)
            {
                Current.OnDeselected();
            }

            Current = selectable;
            selectable.OnSelected();
        }

        public static bool IsFocused(MornUISelectableMonoBase selectable)
        {
            return Current == selectable;
        }

        public static void SetLock(bool isLocked)
        {
            s_isLocked = isLocked;
        }

        public static void Update()
        {
            if (s_isLocked || Current == null)
            {
                return;
            }

            if (s_moveAction == null)
            {
                s_moveAction = ((InputSystemUIInputModule)EventSystem.current.currentInputModule).move;
            }

            if (s_submitAction == null)
            {
                s_submitAction = ((InputSystemUIInputModule)EventSystem.current.currentInputModule).submit;
            }

            if (s_submitAction.action.WasPressedThisFrame())
            {
                Current.Submit();
            }

            var moveAxis = s_moveAction.action.ReadValue<Vector2>();
            var axis = GetAxisDir(moveAxis);
            if (s_cachedAxisDir == axis)
            {
                var dif = Time.unscaledTime - s_cachedMoveTime;
                if (dif < (s_isFastMove ? s_solver._fastIntervalSeconds : s_solver._freezeSeconds))
                {
                    return;
                }

                s_isFastMove = true;
                s_cachedAxisDir = axis;
                s_cachedMoveTime = Time.unscaledTime;
                Current.Transition(axis);
            }
            else
            {
                s_isFastMove = false;
                s_cachedAxisDir = axis;
                s_cachedMoveTime = Time.unscaledTime;
                Current.Transition(axis);
            }
        }

        private static MornUIAxisDirType GetAxisDir(Vector2 moveAxis)
        {
            if (moveAxis.y > 0)
            {
                return MornUIAxisDirType.Up;
            }

            if (moveAxis.y < 0)
            {
                return MornUIAxisDirType.Down;
            }

            if (moveAxis.x > 0)
            {
                return MornUIAxisDirType.Right;
            }

            if (moveAxis.x < 0)
            {
                return MornUIAxisDirType.Left;
            }

            return MornUIAxisDirType.None;
        }
    }
}
