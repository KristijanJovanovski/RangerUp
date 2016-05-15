
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RangerUp
{
    public class HiResTimer
    {
        Stopwatch stopwatch;

        public HiResTimer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Reset();
        }

        public long ElapsedMilliseconds
        {
            get { return stopwatch.ElapsedMilliseconds; }
        }

        public void Start()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Reset();
                stopwatch.Start();
            }
        }

        public void Stop()
        {
            stopwatch.Stop();
        }
        public void Continue()
        {
            stopwatch.Start();
        }

        public bool isActive()
        {
            return stopwatch.IsRunning;
        }
    }
}
