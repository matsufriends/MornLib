using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MornEditor
{
    internal class ButtonDrawer
    {
        private readonly List<MethodInfo> _methods = new();

        public ButtonDrawer(object target)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var method in target.GetType().GetMethods(flags))
            {
                if (method.GetCustomAttribute<ButtonAttribute>() != null)
                {
                    _methods.Add(method);
                }
            }
        }

        public void DrawButtons(Object[] targets)
        {
            foreach (var method in _methods)
            {
                if (GUILayout.Button(ObjectNames.NicifyVariableName(method.Name)))
                {
                    foreach (var obj in targets)
                    {
                        method.Invoke(obj, null);
                        EditorUtility.SetDirty(obj);
                    }
                }
            }
        }
    }
}