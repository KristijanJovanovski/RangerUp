using System.Diagnostics;

namespace RangerUp
{
    public class HiResTimer
    {
        readonly Stopwatch _stopwatch;

        public HiResTimer()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Reset();
        }

        public long ElapsedMilliseconds
        {
            get { return _stopwatch.ElapsedMilliseconds; }
        }

        public void Start()
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Reset();
                _stopwatch.Start();
            }
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }
        public void Continue()
        {
            _stopwatch.Start();
        }

        public bool IsActive()
        {
            return _stopwatch.IsRunning;
        }
    }
}
