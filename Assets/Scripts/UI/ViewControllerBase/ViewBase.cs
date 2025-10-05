using System;
using System.Threading;
using UnityEngine;

namespace UI.ViewControllerBase
{
    public abstract class ViewBase : MonoBehaviour
    {
        private CancellationTokenSource _cts = new();
        private event Action _onDestroy;
        
        public CancellationTokenSource CancellationToken => _cts;
        public event Action OnDestroyAction
        {
            add => _onDestroy += value;
            remove => _onDestroy -= value;
        }
        
        private void OnDestroy()
        {
            _cts.Cancel();
            _cts.Dispose();
            _onDestroy?.Invoke();
        }
    }
}