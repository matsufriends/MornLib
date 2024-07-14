using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MornLib.Cores
{
    public class MornTaskCanceller
    {
        private readonly CancellationToken _onDestroyToken;
        private CancellationTokenSource _source = new();

        public MornTaskCanceller(GameObject gameObject)
        {
            _onDestroyToken = gameObject.GetCancellationTokenOnDestroy();
            Token = CancellationTokenSource.CreateLinkedTokenSource(_onDestroyToken, _source.Token).Token;
        }

        public CancellationToken Token { get; private set; }

        public void Cancel()
        {
            _source.Cancel();
            _source.Dispose();
            _source = new CancellationTokenSource();
            Token = CancellationTokenSource.CreateLinkedTokenSource(_onDestroyToken, _source.Token).Token;
        }
    }
}