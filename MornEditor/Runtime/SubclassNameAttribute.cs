using System;

namespace MornEditor
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SubclassNameAttribute : Attribute
    {
        public string Name { get; }

        public SubclassNameAttribute(string name)
        {
            Name = name;
        }
    }
}