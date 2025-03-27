using System;
using System.IO;
using OpTIAtumLib.Core;
using OpTIAtumLib.Model;
using OpTIAtumLib.Service.TIASession;
using OpTIAtumLib.Service.Projects;
using OpTIAtumLib.Utility.Logger;


namespace SmarTIAtumProjeX.Test
{
    internal class TestRunAddScanDevice
    {
        public static void Run()
        {
            try
            {
                // Создаём фасадный объект для управления TIA Portal и сервисами Openness
                var tia = new TIAConnector();

                // Запуск TIA Portal
                tia.SessionService.CreateTIAInstance(enableGuiTIA: true);

                // Получаем ссылку на созданный экземпляр TIA (через приведение)
                var instance = ((TIASessionService)tia.SessionService).Instance;

                string projectName = "TestProject";
                string projectPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TIA_Projects");

                if (!Directory.Exists(projectPath))
                    Directory.CreateDirectory(projectPath);

                // Создаём проект
                var project = new ProjectService(instance).CreateProject(projectPath, projectName);

                // Инициализируем фасад TIAConnector
                tia.Initialize(instance, project);

                Logger.Info("Создаём Идентификаторы устройств и модулей:");

                DeviceModel gsdScaner1 = new DeviceModel
                {
                    DeviceName = "Scaner_13_040",
                    //Station = "GSD device_1",
                    GsdName = "GSDML-V2.34-HIKVISION-MV-20210719.XML",
                    GsdType = "DAP",
                    GsdId = "DAP 3",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {gsdScaner1.DeviceName} TypeId: {gsdScaner1.TypeIdentifier}");

                DeviceModel gsdScaner2 = new DeviceModel
                {
                    DeviceName = "Scaner_13_100",
                    //Station = "GSD device_2",
                    GsdName = "GSDML-V2.34-HIKVISION-MV-20210719.XML",
                    GsdType = "DAP",
                    GsdId = "DAP 3",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {gsdScaner2.DeviceName} TypeId: {gsdScaner2.TypeIdentifier}");

                tia.DeviceService.AddDeviceToProject(gsdScaner1);
                tia.DeviceService.AddDeviceToProject(gsdScaner1);
                tia.DeviceService.AddDeviceToUngrouped(gsdScaner2);

                Logger.Info("Устройства успешно добавлены в проект.");
            }
            catch (Exception ex)
            {
                Logger.Debug("[ERROR] Ошибка: " + ex.Message);
            }

            Logger.Info("Тест завершён. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
