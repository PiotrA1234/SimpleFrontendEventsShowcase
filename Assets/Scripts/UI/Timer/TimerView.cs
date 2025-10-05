using System;
using TMPro;
using UI.ViewControllerBase;
using UnityEngine;

namespace UI.Timer
{
    public class TimerView : ViewBase
    {
        [field: SerializeField] public TextMeshProUGUI Text { get; set; }
        private event Action _onEverySecond;
        public event Action OnEverySecond
        {
            add => _onEverySecond += value;
            remove => _onEverySecond -= value;
        }
        
        private void Update()
        {
            _onEverySecond?.Invoke();
        }
    }
}