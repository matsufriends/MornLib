using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("MornSetting.Editor")]
namespace MornSetting
{
    public abstract class MornSettingSoBase : ScriptableObject
    {
        [SerializeField] private string _key;
        protected string Key => _key;

        internal void SetKey(string key)
        {
            _key = key;
        }
    }
}