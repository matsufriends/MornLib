using System;
using UnityEngine;

namespace MornLib.Attribute
{
    public sealed class MornEnumToStringAttribute : PropertyAttribute
    {
        public readonly Type EnumType;

        public MornEnumToStringAttribute(Type type)
        {
            EnumType = type;
        }
    }
}
