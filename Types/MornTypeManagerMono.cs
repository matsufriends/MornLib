using System;
using MornLib.Singletons;
using UniRx;
using UnityEngine;

namespace MornLib.Types
{
    public sealed class MornTypeManagerMono : MornSingletonMono<MornTypeManagerMono>
    {
        private bool _isActive;
        private readonly Subject<char> _inputChar = new();
        public IObservable<char> OnInputChar => _inputChar;

        protected override void MyAwake()
        {
        }

        public void SetIsActive(bool isActive)
        {
            _isActive = isActive;
        }

        private void OnGUI()
        {
            if (_isActive && Event.current.type == EventType.KeyDown)
            {
                var c = KeyCodeToChar(Event.current.keyCode);
                if (c != '\0')
                {
                    _inputChar.OnNext(c);
                }
            }
        }

        private static char KeyCodeToChar(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.A: return 'a';
                case KeyCode.B: return 'b';
                case KeyCode.C: return 'c';
                case KeyCode.D: return 'd';
                case KeyCode.E: return 'e';
                case KeyCode.F: return 'f';
                case KeyCode.G: return 'g';
                case KeyCode.H: return 'h';
                case KeyCode.I: return 'i';
                case KeyCode.J: return 'j';
                case KeyCode.K: return 'k';
                case KeyCode.L: return 'l';
                case KeyCode.M: return 'm';
                case KeyCode.N: return 'n';
                case KeyCode.O: return 'o';
                case KeyCode.P: return 'p';
                case KeyCode.Q: return 'q';
                case KeyCode.R: return 'r';
                case KeyCode.S: return 's';
                case KeyCode.T: return 't';
                case KeyCode.U: return 'u';
                case KeyCode.V: return 'v';
                case KeyCode.W: return 'w';
                case KeyCode.X: return 'x';
                case KeyCode.Y: return 'y';
                case KeyCode.Z: return 'z';
                case KeyCode.Minus: return 'ー';
                default: return '\0';
            }
        }
    }
}
