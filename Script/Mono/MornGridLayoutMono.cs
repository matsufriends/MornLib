using UnityEngine;

namespace MornLib.Mono
{
    public class MornGridLayoutMono : MonoBehaviour
    {
        [SerializeField] private Vector3 _basePos;
        [SerializeField] private Vector3 _dif;

        [ContextMenu("Sort")]
        public void Sort()
        {
            var childCount = 0;
            for (var i = 0; i < transform.childCount; i++)
                childCount += transform.GetChild(i).gameObject.activeSelf ? 1 : 0;

            var offset = -_dif * (childCount - 1) / 2f;
            for (var i = childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                child.transform.localPosition = offset + _dif * i;
            }
        }
    }
}