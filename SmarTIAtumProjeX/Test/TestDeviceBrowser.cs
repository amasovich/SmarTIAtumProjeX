using System;
using OpTIAtumLib.Core;
using OpTIAtumLib.Service.TIASession;
using OpTIAtumLib.Service.Projects;
using OpTIAtumLib.Utility.Device;


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

                //Открытие проекта
                string projectName = "TestProject";
                string projectPath = @"C:\Users\v.bereznyak\Desktop\TIA_Projects\TestProject";
                //string projectName = "MK1517_Stage2OldCPU_LS_1";
                //string projectPath = @"C:\Users\v.bereznyak\Documents\Automation\Sessions\MK1517_Stage2OldCPU_LS_1";

                var project = new ProjectService(instance).OpenProject(projectPath, projectName);
                Console.WriteLine($"[INFO] Проект открыт: {projectName}");

                // Инициализация TIAConnector с открытым проектом
                tia.Initialize(instance, project);

                // Получение списка всех свойств устройств и модулей DeviceItem
                //DeviceInspector.DumpDeviceAndItemProperties(tia.Project);

                // Получение списка устройств
                var devices = DeviceBrowser.GetDeviceModels(tia.Project);

                foreach (var dev in devices)
                {
                    string typeInfo = !string.IsNullOrWhiteSpace(dev.OrderNumber)
                        ? $"{dev.OrderNumber} / {dev.FirmwareVersion}"
                        : $"{dev.GsdName} / {dev.GsdType} / {dev.GsdId}";

                    Console.WriteLine($"[OK] {dev.Station} → {dev.DeviceName} → {typeInfo}, Slot: {dev.PositionNumber}, Class: {dev.ClassType}");
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