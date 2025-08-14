using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace pomodoro
{
    public enum SessionType
    {
        Pomodoro = 1,
        ShortBreak = 2,
        LongBreak = 3,
    }

    internal class Application
    {
        private int menuSelection = 0;
        private SessionType sessionType;
        // Tried to add a hug string, but WindowsConsole do not support it. 
        //private string hug = "\u0028\u3063\u25D5\u203F\u25D5\u0029\u3063";


        public void Run()
        {

            while (menuSelection != -100)
            {
                ShowMenu();
                menuSelection = GetUserSelection();
                if (menuSelection < 0)
                {
                    Console.WriteLine("User input is not valid.");
                    continue;
                }
                else if (menuSelection == 0 || menuSelection > 3)
                {
                    Console.WriteLine("Selection not in menu. Try a value from 1 - 3");
                    continue;
                }

                // Create the correct session
                sessionType = menuSelection switch
                {
                    1 => SessionType.Pomodoro,
                    2 => SessionType.ShortBreak,
                    3 => SessionType.LongBreak,
                };

                // Create New Session
                PomodoroSession session = new PomodoroSession(sessionType);
                PomodoroTimer timer = new PomodoroTimer(session.Duration);

                // -- Hook up delegates --
                // I just learn how to use delegates and anonymous functions.
                // ManualResetEventSlim, didn't knew it's functionality, I added
                // this to avoid using a while loop that burns my procesor.
                using var done = new ManualResetEventSlim(false);
                timer.RegisterWithPomodoroTimerHanlder(PrintPomodoroTimer);
                timer.ResgisterWithPomodoroCompleteHanlder(() =>
                {
                    Console.WriteLine($"\n Done!");
                    Console.Beep(392, 500);
                    done.Set();
                });

                Console.Clear();
                Console.CursorVisible = false;
                timer.Start();
                done.Wait();

                Console.WriteLine("\nPress Enter to return to menu...");
                Console.ReadLine();

            }
        }



        private void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Simple Pomodoro Application");
            Console.WriteLine("\t\t [1]. Execute a pomodoro period.");
            Console.WriteLine("\t\t [2]. Execute a short rest period.");
            Console.WriteLine("\t\t [3]. Execute a long rest period.");
        }

        private int GetUserSelection()
        {
            int userInputInt = -1;
            string userInput = "";

            Console.WriteLine("What do you want to do? ");
            userInput = Console.ReadLine();
            if (userInput != null)
            {
                if (userInput.Trim().ToLower() == "exit")
                    return -100;
                if (int.TryParse(userInput, out userInputInt))
                    return userInputInt;
            }
            return -1;
        }

        private void PrintPomodoroTimer(TimeSpan elapsed, TimeSpan remaining)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("****** POMODORO ******\n");
            Console.WriteLine($"Elapsed: {elapsed.ToString()}");
            Console.WriteLine($"Remaining: {remaining.ToString()}");
        }
    }
}
