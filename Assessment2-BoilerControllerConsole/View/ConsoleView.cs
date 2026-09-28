using System.Text;
using Assessment2_BoilerControllerConsole.Model;
using Assessment2_BoilerControllerConsole.Model.Enums;

namespace Assessment2_BoilerControllerConsole.View
{
    public class ConsoleView
    {
        private readonly object _consoleLock = new object();
        private const int NotificationHeight = 10;
        private const int ApplicationStartLine = NotificationHeight + 1;

        public ConsoleView()
        {
            Console.Clear();
            Console.CursorVisible = true;

            DrawNotificationArea();
            DrawApplicationArea();
        }
        
        private void DrawNotificationArea()
        {
            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, 0);
                for(int i = 0; i < NotificationHeight; i++)
                {
                    ClearCurrentLine();
                }
                Console.SetCursorPosition(0, 0);
                Console.Write("===== Notifications ======");
            }
        }

        private void DrawApplicationArea()
        {
            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, ApplicationStartLine);
                Console.WriteLine("====== BOILER CONTROLLER INITIALIZED =====");
                Console.WriteLine("[1] Start Boiler Sequence");
                Console.WriteLine("[2] Stop Boiler Sequence");
                Console.WriteLine("[3] Simulate Boiler Error");
                Console.WriteLine("[4] Toggle Run Interlock");
                Console.WriteLine("[5] Reset Lockout");
                Console.WriteLine("[6] View Event log");
                Console.WriteLine("[7] Exit");
                Console.Write("Select an option: ");
            }
        }

        public MenuOptions? GetMenuOptions()
        {
            while (true)
            {
                lock (_consoleLock)
                {
                    Console.SetCursorPosition(0, ApplicationStartLine + 8);
                    ClearCurrentLine();
                    Console.Write("Select an option: ");
                }
                string input = ReadInput();

                switch (input)
                {
                    case "1":
                        return MenuOptions.StartBoiler;
                    case "2":
                        return MenuOptions.StopBoiler;
                    case "3":
                        return MenuOptions.SimulateBoilerError;
                    case "4":
                        return MenuOptions.ToggleRun;
                    case "5":
                        return MenuOptions.ResetLockout;
                    case "6":
                        return MenuOptions.ViewEventLog;
                    case "7":
                        return MenuOptions.Exit;
                    default:
                        lock (_consoleLock)
                        {
                            Console.SetCursorPosition(0, ApplicationStartLine + 8);
                            ClearCurrentLine();
                            Console.WriteLine("Choose a valid option.");
                            Console.Write("Select an option: ");
                        }
                        break;
                }
            }
        }

        public void ShowNotifications(List<Notification> notifications)
        {
            lock (_consoleLock)
            {
                int cursorTop = Console.CursorTop;
                int cursorLeft = Console.CursorLeft;

                Console.SetCursorPosition(0, 0);
                Console.WriteLine("====== NOTIFICATIONS =======");
                int displayCount = Math.Min(notifications.Count, NotificationHeight - 1);

                for(int i = 0; i < NotificationHeight - 1; i++)
                {
                    string message = string.Empty;
                    if(i < displayCount)
                    {
                        int index = notifications.Count - displayCount + i;
                        Notification notification = notifications[index];
                        message = $"{notification.TimeStamp: dd-MM-yyyy HH:mm:ss} " + notification.Message;
                    }

                    WriteLineSafely(message);
                }
                Console.SetCursorPosition(cursorLeft, cursorTop);
            }
        }

        private void WriteLineSafely(string message)
        {
            int width = Math.Max(1, Console.WindowWidth - 1);
            if(message.Length > width)
            {
                message = message.Substring(0, width);
            }
            message = message.PadRight(width);
            Console.WriteLine(message);
        }

        private void ClearCurrentLine()
        {
            int width = Math.Max(1, Console.WindowWidth - 1);
            Console.Write(new string(' ', width));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        private string ReadInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo consoleKey = Console.ReadKey(true);
                if(consoleKey.Key == ConsoleKey.Enter)
                {
                    return input.ToString().Trim();
                }
                if(consoleKey.Key == ConsoleKey.Backspace)
                {
                    if(input.Length > 0)
                    {
                        input.Remove(input.Length - 1, 1);
                        lock (_consoleLock)
                        {
                            Console.Write("\b \b");
                        }
                    }
                    continue;
                }
                if (!char.IsControl(consoleKey.KeyChar))
                {
                    input.Append(consoleKey.KeyChar);
                    lock (_consoleLock)
                    {
                        Console.Write(consoleKey.KeyChar);
                    }
                }
            }
        }

        public void DisplayLogDetails(List<Logger> recentOrders)
        {
            
        }
    }
}



