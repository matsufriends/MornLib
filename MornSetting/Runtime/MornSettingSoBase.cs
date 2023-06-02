using UnityEngine;

namespace MornSetting
{
    public abstract class MornSettingSoBase : ScriptableObject
    {
        [SerializeField] private string _key;
        protected string Key => _key;
    }
}
