using Microsoft.Win32;
using OpTIAtumLib.Interface;
using OpTIAtumLib.Utility;
using Siemens.Engineering;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;

namespace OpTIAtumLib.Services
{
    public class TIASessionService : ITIASessionService
    {
        public TiaPortal Instance { get; private set; }

        /// <inheritdoc/>
        public void CreateTIAInstance(bool enableGuiTIA)
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            string appName = process.ProcessName;
            string appPath = process.MainModule.FileName;

            SetWhiteList(appName, appPath);

            if (IsExternalTiaPortalRunning())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[!] Обнаружен уже запущенный экземпляр TIA Portal.");
                Console.Write("Закрыть его и открыть новый экземпляр через Openness? [Y/N]: ");
                Console.ResetColor();

                var answer = Console.ReadKey().Key;
                Console.WriteLine();

                if (answer == ConsoleKey.Y)
                {
                    Logger.Info("Пользователь согласился закрыть существующий экземпляр TIA.");
                    // Убиваем все процессы TIA Portal
                    foreach (var proc in System.Diagnostics.Process.GetProcessesByName("Siemens.Automation.Portal"))
                    {
                        try
                        {
                            proc.Kill();
                            proc.WaitForExit();
                            Logger.Warn("Старый экземпляр TIA Portal завершён.");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Ошибка при завершении процесса TIA Portal: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Logger.Info("Пользователь отказался от перезапуска TIA. Опеннес не будет запущен.");
                    return;
                }
            }

            Logger.Info($"Запуск TIA Portal...");

            Instance = enableGuiTIA
                ? new TiaPortal(TiaPortalMode.WithUserInterface)
                : new TiaPortal(TiaPortalMode.WithoutUserInterface);

            Logger.Info($"TIA Portal запущен в режиме: {(enableGuiTIA ? "С GUI" : "Фоновый")}");
        }

        /// <inheritdoc/>
        public bool IsTiaPortalRunning()
        {
            return Instance != null;
        }

        /// <inheritdoc/>
        public bool IsExternalTiaPortalRunning()
        {
            var tiaProcesses = Process.GetProcessesByName("Siemens.Automation.Portal");

            foreach (var p in tiaProcesses)
            {
                Logger.Debug($"Найден запущенный TIA Portal: PID = {p.Id}, Title = {p.MainWindowTitle}");
            }

            return tiaProcesses.Any();
        }

        /// <inheritdoc/>
        public void SetWhiteList(string ApplicationName, string ApplicationStartupPath)
        {
            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey software = null;
            try
            {
                software = key.OpenSubKey(@"SOFTWARE\Siemens\Automation\Openness")
                    .OpenSubKey("18.0")
                    .OpenSubKey("Whitelist")
                    .OpenSubKey(ApplicationName + ".exe")
                    .OpenSubKey("Entry", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
            }
            catch (Exception)
            {

                //Eintrag in der Whitelist ist nicht vorhanden
                //Entry in whitelist is not available
                software = key.CreateSubKey(@"SOFTWARE\Siemens\Automation\Openness")
                    .CreateSubKey("18.0")
                    .CreateSubKey("Whitelist")
                    .CreateSubKey(ApplicationName + ".exe")
                    .CreateSubKey("Entry", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
            }

            string lastWriteTimeUtcFormatted = string.Empty;
            DateTime lastWriteTimeUtc;
            HashAlgorithm hashAlgorithm = SHA256.Create();
            FileStream stream = File.OpenRead(ApplicationStartupPath);
            byte[] hash = hashAlgorithm.ComputeHash(stream);
            // this is how the hash should appear in the .reg file
            string convertedHash = Convert.ToBase64String(hash);
            software.SetValue("FileHash", convertedHash);
            lastWriteTimeUtc = new FileInfo(ApplicationStartupPath).LastWriteTimeUtc;
            // this is how the last write time should be formatted
            lastWriteTimeUtcFormatted = lastWriteTimeUtc.ToString(@"yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            software.SetValue("DateModified", lastWriteTimeUtcFormatted);
            software.SetValue("Path", ApplicationStartupPath);
        }
    }
}

