using System;
using UnityEngine;

namespace MornEnum
{
    public sealed class MornEnumToStringAttribute : PropertyAttribute
    {
        public MornEnumToStringAttribute(Type type)
        {
            EnumType = type;
        }

        public Type EnumType { get; }
    }
}