using System;
using UI.ViewControllerBase;

namespace UI.Timer
{
    public class TimerViewController : ViewControllerBase<TimerView>
    {
        private long _endTimestampTicks;
        private event Action _onFinished;
        
        public TimerViewController(TimerView view) : base(view)
        {
        }

        public TimerViewController StartCountdown(long endTimestampTicks)
        {
            SetActive(true);
            _view.OnEverySecond -= Refresh;
            _view.OnEverySecond += Refresh;
            _endTimestampTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks
                                 + endTimestampTicks * TimeSpan.TicksPerSecond;
            
            Refresh();
            return this;
        }
        
        public void SetOnFinished(Action onFinished)
        {
            _onFinished -= onFinished;
            _onFinished += onFinished;
        }

        public void SetActive(bool value)
        {
            _view.gameObject.SetActive(value);
        }

        private void Refresh()
        {
            TimeSpan remaining = new TimeSpan(_endTimestampTicks - DateTime.UtcNow.Ticks);
            if (remaining.TotalSeconds < 0)
            {
                remaining = TimeSpan.Zero;
            }
            else if (remaining.TotalSeconds == 0)
            {
                _onFinished?.Invoke();
                _onFinished = null;
            }

            string formatted;

            int totalDays = (int)remaining.TotalDays;
            if (totalDays > 0)
            {
                formatted = $"{totalDays}d {remaining.Hours}h";
            }
            else
            {
                formatted = remaining.ToString(@"hh\:mm\:ss");
            }

            _view.Text.text = formatted;
        }

    }
}