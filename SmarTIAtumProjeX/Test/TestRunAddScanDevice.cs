using OpTIAtumLib;
using OpTIAtumLib.Model;
using OpTIAtumLib.Services;
using OpTIAtumLib.Utility;
using System;
using System.IO;
using System.Linq;

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
                Console.WriteLine($"[DIR] Проект создан: {projectName}");

                // Инициализируем фасад TIAConnector
                tia.Initialize(instance, project);

                Console.WriteLine($"[INFO] Создаём Идентификаторы устройств и модулей:");

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
                Logger.Create($"Device: {gsdScaner1.DeviceName} TypeId: {gsdScaner1.TypeIdentifier}");

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
                Logger.Create($"Device: {gsdScaner2.DeviceName} TypeId: {gsdScaner2.TypeIdentifier}");

                tia.DeviceService.AddDeviceToProject(gsdScaner1);
                tia.DeviceService.AddDeviceToUngrouped(gsdScaner2);

                DeviceModel gsdOdotHeadModule1 = new DeviceModel
                {
                    DeviceName = "TB09_010_K002",
                    //Station = "GSD device_1",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "DAP",
                    GsdId = "DAP1",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Create($"Device: {gsdOdotHeadModule1.DeviceName} TypeId: {gsdOdotHeadModule1.TypeIdentifier}");

                var odot1 = tia.DeviceService.AddDeviceToUngrouped(gsdOdotHeadModule1);

                DeviceModel gsdOdotImodule1 = new DeviceModel
                {
                    DeviceName = "BT-124F_1",
                    //Station = "GSD device_1",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "M",
                    GsdId = "IDM_BT124F",
                    IncludeFailsafe = false,
                    PositionNumber = 1
                };
                Logger.Create($"DeviceItem: {gsdOdotImodule1.DeviceName} TypeId: {gsdOdotImodule1.TypeIdentifier}");

                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotImodule1);

                //tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotIOmodule1);

                Console.WriteLine("[OK] Устройства успешно добавлены в проект.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Ошибка: " + ex.Message);
            }

            Console.WriteLine("[FINISH] Тест завершён. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
