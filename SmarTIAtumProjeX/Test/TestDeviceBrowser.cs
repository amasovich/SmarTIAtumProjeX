using OpTIAtumLib;
using OpTIAtumLib.Model;
using OpTIAtumLib.Services;
using System;

namespace SmarTIAtumProjeX.Test
{
    internal class TestDeviceBrowse
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("[START] Запуск TIA Portal...");

                // Создаём фасадный объект для управления TIA Portal и сервисами Openness
                var tia = new TIAConnector();

                // Запуск TIA Portal с пользовательским интерфейсом
                tia.SessionService.CreateTIAInstance(enableGuiTIA: true);

                // Получение экземпляра TIA через реализацию (без изменения интерфейса)
                var instance = ((TIASessionService)tia.SessionService).Instance;

                // Открытие проекта
                string projectName = "TestProject";
                string projectPath = @"C:\Users\v.bereznyak\Desktop\TIA_Projects\TestProject";
                var project = new ProjectService(instance).OpenProject(projectPath, projectName);
                Console.WriteLine($"[INFO] Проект открыт: {projectName}");

                // Инициализация TIAConnector с открытым проектом
                tia.Initialize(instance, project);

                // Получение списка устройств через сервис
                var devices = tia.DeviceService.GetDevices();
                foreach (var dev in devices)
                {
                    Console.WriteLine($"[OK] {dev.DeviceName} → {dev.OrderNumber}, {dev.FirmwareVersion}, Class: {dev.ClassType}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Ошибка: " + ex.Message);
            }

            Console.WriteLine("[FINISH] Все устройства выгружены. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}

