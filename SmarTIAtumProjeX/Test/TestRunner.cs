using OpTIAtumLib;
using OpTIAtumLib.Model;
using OpTIAtumLib.Services;
using OpTIAtumLib.Utility;
using System;
using System.IO;

namespace SmarTIAtumProjeX.Test
{
    internal class TestRunner
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
                
                DeviceModel plc = new DeviceModel
                {
                    DeviceName = "MDC2_K001",
                    Station = "S71500 station_1",
                    OrderNumber = "6ES7 517-3FP00-0AB0", // S7-1517
                    FirmwareVersion = "V3.0",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Create($"Device: {plc.DeviceName} TypeId: {plc.TypeIdentifier}");

                DeviceModel etStation = new DeviceModel
                {
                    DeviceName = "MDC2_K002",
                    Station = "ET 200SP station_1",
                    OrderNumber = "6ES7 155-6AU01-0CN0", // IM 155-6 PN ST
                    FirmwareVersion = "V4.2",
                    IncludeFailsafe = false,
                    PositionNumber = 1
                };
                Logger.Create($"Device: {etStation.DeviceName} TypeId: {etStation.TypeIdentifier}");

                DeviceModel scalance = new DeviceModel
                {
                    DeviceName = "MDC2_K005",
                    Station = "SCALANCE XB-200",
                    OrderNumber = "6GK5 216-0BA00-2AB2", // SCALANCE XB216
                    FirmwareVersion = "V4.2",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Create($"Device: {scalance.DeviceName} TypeId: {scalance.TypeIdentifier}");

                //DeviceModel gsdScaner = new DeviceModel
                //{
                //    DeviceName = "Scaner_13_040",
                //    Station = "GSD_Scan_13_040",
                //    GsdName = "GSDML-V2.34-HIKVISION-MV-20210719.XML",
                //    GsdType = "DAP",
                //    GsdId = "DAP3",
                //    IncludeFailsafe = false,
                //    PositionNumber = 0
                //};
                //Logger.Create($"Device: {gsdScaner.DeviceName} TypeId: {gsdScaner.TypeIdentifier}");

                DeviceModel gsdOdotHeadModule1 = new DeviceModel
                {
                    DeviceName = "TB09_010_K002",
                    Station = "GSD device_1",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "DAP",
                    GsdId = "DAP1",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Create($"Device: {gsdOdotHeadModule1.DeviceName} TypeId: {gsdOdotHeadModule1.TypeIdentifier}");

                DeviceModel gsdOdotHeadModule2 = new DeviceModel
                {
                    DeviceName = "TB09_070_K002",
                    Station = "GSD device_2",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "DAP",
                    GsdId = "DAP1",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Create($"Device: {gsdOdotHeadModule2.DeviceName} TypeId: {gsdOdotHeadModule2.TypeIdentifier}");

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

                DeviceModel gsdOdotImodule2 = new DeviceModel
                {
                    DeviceName = "BT-124F_2",
                    //Station = "GSD device_1",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "M",
                    GsdId = "IDM_BT124F",
                    IncludeFailsafe = false,
                    PositionNumber = 2
                };
                Logger.Create($"DeviceItem: {gsdOdotImodule2.DeviceName} TypeId: {gsdOdotImodule2.TypeIdentifier}");

                DeviceModel gsdOdotOmodule1 = new DeviceModel
                {
                    DeviceName = "BT-222F_1",
                    //Station = "GSD device_1",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "M",
                    GsdId = "IDM_BT222F",
                    IncludeFailsafe = false,
                    PositionNumber = 3
                };
                Logger.Create($"DeviceItem: {gsdOdotOmodule1.DeviceName} TypeId: {gsdOdotOmodule1.TypeIdentifier}");

                DeviceModel gsdOdotIOmodule1 = new DeviceModel
                {
                    DeviceName = "BT-623F_1",
                    //Station = "GSD device_1",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "M",
                    GsdId = "IDM_BT623F",
                    IncludeFailsafe = false,
                    PositionNumber = 4
                };
                Logger.Create($"DeviceItem: {gsdOdotIOmodule1.DeviceName} TypeId: {gsdOdotIOmodule1.TypeIdentifier}");


                tia.DeviceService.AddDeviceToProject(plc);
                tia.DeviceService.AddDeviceToProject(scalance);
                tia.DeviceService.AddDeviceToProject(etStation);

                //tia.AddDeviceToProject(gsdScaner);

                var odot1 = tia.DeviceService.AddDeviceToProject(gsdOdotHeadModule1);
                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotImodule1);
                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotImodule2);
                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotOmodule1);
                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotIOmodule1);

                var odot2 = tia.DeviceService.AddDeviceToProject(gsdOdotHeadModule2);
                tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotImodule1);
                tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotImodule2);
                tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotOmodule1);
                tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotIOmodule1);

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

