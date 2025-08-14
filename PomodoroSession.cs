using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pomodoro
{
    internal sealed class PomodoroSession
    {
        public SessionType Type { get; }
        public TimeSpan Duration { get; }

        public PomodoroSession(SessionType sessionType)
        {
            Type = sessionType;
            Duration = Type switch
            {
                SessionType.Pomodoro => TimeSpan.FromMinutes(25),
                SessionType.ShortBreak => TimeSpan.FromMinutes(1),
                SessionType.LongBreak => TimeSpan.FromMinutes(15),
                _ => TimeSpan.FromMinutes(25)
            };
        }
    }
}
