using Serilog;

namespace backend.Utils
{
    public enum LogLevels
    {
        Information, Warning, Error
    }
    public static class PrintLogger
    {
        public static void PrintLog(string message, LogLevels level)
        {
            switch (level)
            {
                case LogLevels.Information:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Info: " + message);
                    Log.Information(message);
                    break;
                case LogLevels.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning: " + message);
                    Log.Warning(message);
                    break;
                case LogLevels.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: " + message);
                    Log.Error(message);
                    break;
                default:
                    break;
            }
        }
    }
}
