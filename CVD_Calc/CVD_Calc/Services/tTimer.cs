using System;
using System.Threading;
using Xamarin.Forms;
namespace CVD_Calc.Services
{
    class tTimer
    {
        private readonly TimeSpan _timeSpan;
        private readonly Action _callback;
        private CancellationTokenSource _cancellation;
        public tTimer(TimeSpan timespan, Action callback)
        {
            _timeSpan = timespan;
            _callback = callback;
            _cancellation = new CancellationTokenSource();
        }
        public void Start()
        {
            var cts = _cancellation;
            Device.StartTimer(_timeSpan, () => {
                if (cts.IsCancellationRequested) return false;
                _callback.Invoke(); return true;
            });
        }
        public void Stop()
        {
            Interlocked.Exchange(ref _cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}
