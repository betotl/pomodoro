using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace pomodoro
{
    public sealed class PomodoroTimer : IDisposable
    {
        // Declares
        public enum PomodoroStatus
        {
            Running,
            Stopped,
            Done,
        }
        // Remember that a delegate signature, is the definition of a sealed class.
        // That means that this is just a definition (type) that needs instantiation.
        // Just like we do in the private field called _listOfHandlers, remember that 
        // it is instantiated at the constructor with the default value.
        public delegate void PomodoroTimerHandlder(TimeSpan elapsed, TimeSpan remaining);
        public delegate void PomodoroCompleteHandler();

        // Properties
        private readonly System.Timers.Timer _timer;
        private PomodoroTimerHandlder _tickHandlers; // This is an instance of the delegate.
        private PomodoroCompleteHandler _completeHanlders;

        public void RegisterWithPomodoroTimerHanlder(PomodoroTimerHandlder handler) =>
            _tickHandlers = handler;
        public void ResgisterWithPomodoroCompleteHanlder(PomodoroCompleteHandler handler) => 
            _completeHanlders = handler;
        

        public PomodoroStatus Status { get; private set; }
        public TimeSpan Duration { get; private init; }
        public TimeSpan Elapsed { get; private set; }
        public TimeSpan Remaining { get; private set; }

        // Constructors
        public PomodoroTimer(TimeSpan duration)
        {
            this.Duration = (duration <= TimeSpan.Zero) ? new TimeSpan(0).Add(TimeSpan.FromMinutes(5)) : duration;
            this.Remaining = Duration;
            this.Elapsed = TimeSpan.Zero;
            this.Status = PomodoroStatus.Stopped;
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
            _completeHanlders?.Invoke();
        }

        private void UpdateTimeSpan(Object? sender, ElapsedEventArgs e)
        {
            if (Elapsed < Duration)
            {
                Elapsed = Elapsed.Add(TimeSpan.FromSeconds(1));
                Remaining = Duration - Elapsed;
                _tickHandlers?.Invoke(Elapsed, Remaining);
            }
            else
            {
                Stop();
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

    }
}
