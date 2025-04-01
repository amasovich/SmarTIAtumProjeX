using System;
using System.Collections.Generic;
using System.IO;

namespace OpTIAtumLib.Utility.Logger
{
    public static class Logger
    {
        private static readonly string logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "SmarTIAtumLog.txt");

        // Настройки уровней логирования
        public static bool EnableDebugConsoleOutput { get; set; } = true;

        private static readonly Dictionary<string, ConsoleColor> DefaultColors = new Dictionary<string, ConsoleColor>()
        {
            { "INFO", ConsoleColor.Green },
            { "ERROR", ConsoleColor.Red },
            { "WARN", ConsoleColor.Yellow },
            { "DEBUG", ConsoleColor.Gray },
            { "CREATE", ConsoleColor.Green },
            { "ADD", ConsoleColor.Green }
        };

        public static void Info(string message, ConsoleColor? color = null) => Log("INFO", message, color);
        public static void Error(string message, ConsoleColor? color = null) => Log("ERROR", message, color);
        public static void Warn(string message, ConsoleColor? color = null) => Log("WARN", message, color);
        public static void Add(string message, ConsoleColor? color = null) => Log("ADD", message, color);
        public static void Create(string message, ConsoleColor? color = null) => Log("CREATE", message, color);
        public static void Debug(string message, ConsoleColor? color = null) => Log("DEBUG", message, color);

        private static void Log(string level, string message, ConsoleColor? color = null)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string fullMessage = $"[{level}] {timestamp} - {message}";

            bool isDebug = level == "DEBUG";

            if (!isDebug || EnableDebugConsoleOutput)
            {
                var outputColor = color ?? (DefaultColors.ContainsKey(level) ? DefaultColors[level] : ConsoleColor.White);
                Console.ForegroundColor = outputColor;
                Console.WriteLine(fullMessage);
                Console.ResetColor();
            }

            try
            {
                File.AppendAllText(logFilePath, fullMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGGER ERROR] Не удалось записать лог в файл: {ex.Message}");
            }
        }
    }
}
