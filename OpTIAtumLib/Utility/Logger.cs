using System;
using System.IO;

namespace OpTIAtumLib.Utility
{
    public static class Logger
    {
        private static readonly string logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "SmarTIAtumLog.txt");

        public static void Info(string message) => Log("INFO", message);
        public static void Error(string message) => Log("ERROR", message);
        public static void Warn(string message) => Log("WARN", message);
        public static void Debug(string message) => Log("DEBUG", message);
        public static void Create(string message) => Log("CREATE", message);

        private static void Log(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string fullMessage = $"[{level}] {timestamp} - {message}";

            // Вывод в консоль
            Console.WriteLine(fullMessage);

            // Запись в файл
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

