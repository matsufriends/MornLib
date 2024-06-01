using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MornTween
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class MornTweenCanvasGroupFadeMono : MonoBehaviour
    {
        [SerializeField] private UIBehaviour receiver;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            
            var token = this.GetCancellationTokenOnDestroy();
            receiver.OnPointerEnterAsObservable().Subscribe(_ => canvasGroup.FadeFillAsync(0.1f,cancellationToken:token).Forget()).AddTo(this);
            receiver.OnPointerExitAsObservable().Subscribe(_ => canvasGroup.FadeClearAsync(0.1f,cancellationToken:token).Forget()).AddTo(this);
        }
    }
}