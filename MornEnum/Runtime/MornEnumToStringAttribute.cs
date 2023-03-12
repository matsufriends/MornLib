using System;
using UnityEngine;

namespace MornEnum.Runtime
{
    /// <summary>enumをstringで保存するAttribute</summary>
    public sealed class MornEnumToStringAttribute : PropertyAttribute
    {
        /// <summary>enumのType</summary>
        public Type EnumType { get; }

        /// <summary>コンストラクタ</summary>
        /// <param name="type">enumのType</param>
        public MornEnumToStringAttribute(Type type)
        {
            EnumType = type;
        }
    }
}
