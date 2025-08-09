using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pomodoro
{
    internal sealed class PomodoroSession
    {
        public enum SessionType
        {
            Pomodoro = 1,
            ShortBreak = 2,
            LongBreak = 3,
        }
        public SessionType Type { get; }
        public TimeSpan Duration { get; }


        public PomodoroSession(SessionType sessionType)
        {
            Type = sessionType;
            Duration = Type switch
            {
                SessionType.Pomodoro => TimeSpan.FromMinutes(25),
                SessionType.ShortBreak => TimeSpan.FromMinutes(5),
                SessionType.LongBreak => TimeSpan.FromMinutes(15),
                _ => TimeSpan.FromMinutes(25)
            };
        }
    }
}
