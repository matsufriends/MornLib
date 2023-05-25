using UnityEngine;

namespace MornSetting
{
    public abstract class MornSettingSoBase : ScriptableObject
    {
        protected string Key => GetInstanceID()
           .ToString();
    }
}
