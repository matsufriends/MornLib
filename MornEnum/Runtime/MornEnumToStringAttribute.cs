using System;
using UnityEngine;

namespace MornEnum
{
    public sealed class MornEnumToStringAttribute : PropertyAttribute
    {
        public Type EnumType { get; }

        public MornEnumToStringAttribute(Type type)
        {
            EnumType = type;
        }
    }
}
