using System;
using UnityEngine;

namespace MornEnum.Runtime
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
