using UnityEngine;

namespace MornLib.Cores
{
    public static class MornComponent
    {
        public static bool TryFindOnlyOneComponent<T>(out T result) where T : MonoBehaviour
        {
            var objects = Object.FindObjectsOfType<T>();
            if (objects.Length == 1)
            {
                result = objects[0];
                return true;
            }

            result = null;
            return false;
        }
    }
}
