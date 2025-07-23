
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Timers;


System.Timers.Timer aTimer;
string? userInput;
int userInputInt = 0;
int menuSelection= 0;

int secondsSetpoint = 0;
int minutesSetpoint = 0;
int secondsCounter = 0;
int minutesCounter = 0;
bool pomodoroDone = false;



SetTimer();



while (menuSelection != -100)
{
    pomodoroDone = false;
    secondsCounter = 0;
    minutesCounter = 0;

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

    /*
        Configure the correct pomodoro times.
    */
    switch ((SessionType)menuSelection)
    {
        case SessionType.Pomodoro:
            secondsSetpoint = 60;
            minutesSetpoint = 1;
            break;
        case SessionType.ShortBreak:
            secondsSetpoint = 60;
            minutesSetpoint = 5;
            break;
        case SessionType.LongBreak:
            secondsSetpoint = 60;
            minutesSetpoint = 15;
            break;
    }

    // Start the timer show up the pomodoro screen
    aTimer.Enabled = true;
    while (pomodoroDone == false)
    {
        if (minutesCounter >= minutesSetpoint)
        {
            aTimer.Stop();
            Console.Beep(392, 1000);
            pomodoroDone = true;
        }
    }
}
Console.WriteLine("Terminating the application...");
aTimer.Dispose();




void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Simple Pomodoro Application");
    Console.WriteLine("\t\t [1]. Execute a pomodoro period.");
    Console.WriteLine("\t\t [2]. Execute a short rest period.");
    Console.WriteLine("\t\t [3]. Execute a long rest period.");
}

int GetUserSelection()
{
    userInputInt = -1;
    userInput = "";

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

/*
    Create a timer with a two second interval.
    and hook up the Elapsed event for the timer.
*/
void SetTimer()
{
    aTimer = new System.Timers.Timer(1000);
    aTimer.Elapsed += OnTimeEvent;
    aTimer.AutoReset = true;
}


void OnTimeEvent(Object source, ElapsedEventArgs e)
{
    Console.Clear();
    //Console.WriteLine($"Pomodoro: {e.SignalTime:HH:mm:ss.fff}");
    Console.WriteLine($"Pomodoro Setpoint: {minutesSetpoint:00}:{0:00}");
    Console.WriteLine($"Pomodoro Time: {minutesCounter:00}:{secondsCounter:00}");
    secondsCounter += 1;
    if (secondsCounter >= secondsSetpoint)
    {
        secondsCounter = 0;
        minutesCounter += 1;
    }
}

enum SessionType
{
    Pomodoro = 1,
    ShortBreak = 2,
    LongBreak = 3,
}