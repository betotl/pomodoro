using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Pomodoro
{
    public sealed class PomodoroTimer : IDisposable
    {
        // Fields
        public enum PomodoroStatus
        {
            Running,
            Paused,
            Stopped,
        }

        // Properties
        private readonly System.Timers.Timer _timer;

        public PomodoroStatus Status { get; private set; }
        public TimeSpan Duration { get; private init; }
        public TimeSpan Elapsed { get; private set; }
        public TimeSpan Remaining { get; private set; }

        // Constructors
        public PomodoroTimer(TimeSpan duration)
        {
            this.Duration = duration;
            this.Remaining = duration;
            this.Elapsed = TimeSpan.Zero;
            _timer = new System.Timers.Timer(1000) { AutoReset = true };
            _timer.Elapsed += UpdateTimeSpan;
        }

        public void Start()
        {
            if (Status == PomodoroStatus.Running) return;
            Status = PomodoroStatus.Running;
            _timer.Start();
        }

        public void Stop()
        {
            Status = PomodoroStatus.Stopped;
            _timer.Stop();
        }

        private void UpdateTimeSpan(Object? sender, ElapsedEventArgs e)
        {
            if (Elapsed <= Duration)
            {
                Elapsed = Elapsed.Add(TimeSpan.FromSeconds(1));
            }

            if (Remaining >= TimeSpan.Zero)
            {
                Remaining = Duration - Elapsed;
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

    }
}
