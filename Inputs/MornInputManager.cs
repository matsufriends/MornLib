using System;
using System.Collections.Generic;
using MornLib.Cores;
using MornLib.Singletons;
using UniRx;
using UnityEngine;

namespace MornLib.Inputs
{
    public class MornInputManager : MornSingleton<MornInputManager>
    {
        private class InputChecker<TArg>
        {
            private readonly Func<TArg, bool> _getInputAction;
            private readonly HashSet<TArg> _useKeyHashSet = new();
            private readonly HashSet<TArg> _updateHashSet = new();
            private readonly HashSet<TArg> _myUpdateHashSet = new();

            public InputChecker(Func<TArg, bool> getInputAction)
            {
                _getInputAction = getInputAction;
            }

            public void UpdateImpl()
            {
                foreach (var key in _useKeyHashSet)
                {
                    if (_getInputAction(key))
                    {
                        _updateHashSet.Add(key);
                    }
                }
            }

            public void MyUpdateImpl()
            {
                _myUpdateHashSet.Clear();
                foreach (var key in _updateHashSet)
                {
                    _myUpdateHashSet.Add(key);
                }

                _updateHashSet.Clear();
            }

            public void RegisterKey(TArg buttonName)
            {
                _useKeyHashSet.Add(buttonName);
            }

            public bool GetInput(TArg buttonName)
            {
                return _myUpdateHashSet.Contains(buttonName);
            }
        }

        private readonly InputChecker<string> _buttonDown = new(Input.GetButtonDown);
        private readonly InputChecker<string> _button = new(Input.GetButton);
        private readonly InputChecker<string> _buttonUp = new(Input.GetButtonUp);
        private readonly InputChecker<KeyCode> _keyDown = new(Input.GetKeyDown);
        private readonly InputChecker<KeyCode> _key = new(Input.GetKey);
        private readonly InputChecker<KeyCode> _keyUp = new(Input.GetKeyUp);
        private readonly InputChecker<int> _mouseUp = new(Input.GetMouseButtonUp);
        private readonly InputChecker<int> _mouse = new(Input.GetMouseButton);
        private readonly InputChecker<int> _mouseDown = new(Input.GetMouseButtonDown);

        protected override void Instanced()
        {
            _mouseUp.RegisterKey(0);
            _mouseUp.RegisterKey(1);
            _mouseUp.RegisterKey(2);
            _mouse.RegisterKey(0);
            _mouse.RegisterKey(1);
            _mouse.RegisterKey(2);
            _mouseDown.RegisterKey(0);
            _mouseDown.RegisterKey(1);
            _mouseDown.RegisterKey(2);
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(MornApp.QuitDisposable);
        }

        private void Update()
        {
            _buttonDown.UpdateImpl();
            _button.UpdateImpl();
            _buttonUp.UpdateImpl();
            _keyDown.UpdateImpl();
            _key.UpdateImpl();
            _keyUp.UpdateImpl();
            _mouseUp.UpdateImpl();
            _mouse.UpdateImpl();
            _mouseDown.UpdateImpl();
        }

        public void MyUpdate()
        {
            _buttonDown.MyUpdateImpl();
            _button.MyUpdateImpl();
            _buttonUp.MyUpdateImpl();
            _keyDown.MyUpdateImpl();
            _key.MyUpdateImpl();
            _keyUp.MyUpdateImpl();
            _mouseUp.MyUpdateImpl();
            _mouse.MyUpdateImpl();
            _mouseDown.MyUpdateImpl();
        }

        public void RegisterButton(string buttonName)
        {
            _buttonUp.RegisterKey(buttonName);
            _button.RegisterKey(buttonName);
            _buttonDown.RegisterKey(buttonName);
        }

        public void RegisterKey(KeyCode keyName)
        {
            _keyDown.RegisterKey(keyName);
            _key.RegisterKey(keyName);
            _keyUp.RegisterKey(keyName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return _buttonUp.GetInput(buttonName);
        }

        public bool GetButton(string buttonName)
        {
            return _button.GetInput(buttonName);
        }

        public bool GetButtonDown(string buttonName)
        {
            return _buttonDown.GetInput(buttonName);
        }

        public bool GetKeyUp(KeyCode keyName)
        {
            return _keyUp.GetInput(keyName);
        }

        public bool GetKey(KeyCode keyName)
        {
            return _key.GetInput(keyName);
        }

        public bool GetKeyDown(KeyCode keyName)
        {
            return _keyDown.GetInput(keyName);
        }

        public bool GetMouseUp(int button)
        {
            return _mouseUp.GetInput(button);
        }

        public bool GetMouse(int button)
        {
            return _mouse.GetInput(button);
        }

        public bool GetMouseDown(int button)
        {
            return _mouseDown.GetInput(button);
        }

        public float GetAxis(string axisName)
        {
            return Input.GetAxis(axisName);
        }

        public float GetAxisRaw(string axisName)
        {
            return Input.GetAxis(axisName);
        }
    }
}
