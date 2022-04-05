using System;
using UnityEngine;
namespace MornLib.Editor {
    public sealed class EnumToStringAttribute : PropertyAttribute {
        public readonly Type EnumType;
        public EnumToStringAttribute(Type type) {
            EnumType = type;
        }
    }
}