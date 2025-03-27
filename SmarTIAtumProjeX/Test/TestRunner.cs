using System;
using System.IO;
using OpTIAtumLib.Core;
using OpTIAtumLib.Model;
using OpTIAtumLib.Service.TIASession;
using OpTIAtumLib.Service.Projects;
using OpTIAtumLib.Utility.Logger;


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

                Logger.Info($"Создаём Идентификаторы устройств и модулей:");

                DeviceModel plc = new DeviceModel
                {
                    DeviceName = "MDC2_K001",
                    Station = "S71500 station_1",
                    OrderNumber = "6ES7 517-3FP00-0AB0", // S7-1517
                    FirmwareVersion = "V3.0",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {plc.DeviceName} TypeId: {plc.TypeIdentifier}");

                tia.DeviceService.AddDeviceToProject(plc);

                DeviceModel etHeadStation = new DeviceModel
                {
                    DeviceName = "MDC2_K002",
                    Station = "ET 200SP station_1",
                    OrderNumber = "6ES7 155-6AU01-0CN0", // IM 155-6 PN ST
                    FirmwareVersion = "V4.2",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {etHeadStation.DeviceName} TypeId: {etHeadStation.TypeIdentifier}");
                
                var etStation = tia.DeviceService.AddDeviceToProject(etHeadStation);

                DeviceModel etDi_1 = new DeviceModel
                {
                    DeviceName = "K020",
                    //Station = "ET 200SP station_1",
                    OrderNumber = "6ES7 131-6BF01-0BA0", // DI 8x24VDC ST
                    FirmwareVersion = "V0.0",
                    IncludeFailsafe = false,
                    PositionNumber = 1
                };
                Logger.Add($"Device: {etDi_1.DeviceName} TypeId: {etDi_1.TypeIdentifier}");
                
                tia.DeviceService.AddDeviceItemToDevice(etStation, etDi_1);

                DeviceModel etFDi_1 = new DeviceModel
                {
                    DeviceName = "K035",
                    //Station = "ET 200SP station_1",
                    OrderNumber = "6ES7 136-6BA00-0CA0", // F-DI 8x24VDC HF
                    FirmwareVersion = "V1.0",
                    IncludeFailsafe = false,
                    PositionNumber = 2
                };
                Logger.Add($"Device: {etFDi_1.DeviceName} TypeId: {etFDi_1.TypeIdentifier}");

                tia.DeviceService.AddDeviceItemToDevice(etStation, etFDi_1);

                DeviceModel etDq_1 = new DeviceModel
                {
                    DeviceName = "K050",
                    //Station = "ET 200SP station_1",
                    OrderNumber = "6ES7 132-6BF01-0BA0", // DQ 8x24VDC/0.5A ST
                    FirmwareVersion = "V0.0",
                    IncludeFailsafe = false,
                    PositionNumber = 3
                };
                Logger.Add($"Device: {etDq_1.DeviceName} TypeId: {etDq_1.TypeIdentifier}");

                tia.DeviceService.AddDeviceItemToDevice(etStation, etDq_1);

                DeviceModel etFDq_1 = new DeviceModel
                {
                    DeviceName = "K066",
                    //Station = "ET 200SP station_1",
                    OrderNumber = "6ES7 136-6DB00-0CA0", // F-DQ 4x24VDC/2A PM HF
                    FirmwareVersion = "V2.0",
                    IncludeFailsafe = false,
                    PositionNumber = 4
                };
                Logger.Add($"Device: {etFDq_1.DeviceName} TypeId: {etFDq_1.TypeIdentifier}");
                
                tia.DeviceService.AddDeviceItemToDevice(etStation, etFDq_1);

                DeviceModel etServerModule_1 = new DeviceModel
                {
                    DeviceName = "Server module_1",
                    //Station = "ET 200SP station_1",
                    OrderNumber = "6ES7 193-6PA00-0AA0", // Electrical and mechanical backplane bus terminator
                    FirmwareVersion = "V1.1",
                    IncludeFailsafe = false,
                    PositionNumber = 5
                };
                Logger.Add($"Device: {etServerModule_1.DeviceName} TypeId: {etServerModule_1.TypeIdentifier}");

                tia.DeviceService.AddDeviceItemToDevice(etStation, etServerModule_1);

                DeviceModel scalance = new DeviceModel
                {
                    DeviceName = "MDC2_K005",
                    Station = "SCALANCE XB-200",
                    OrderNumber = "6GK5 216-0BA00-2AB2", // SCALANCE XB216
                    FirmwareVersion = "V4.2",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {scalance.DeviceName} TypeId: {scalance.TypeIdentifier}");

                tia.DeviceService.AddDeviceToProject(scalance);

                DeviceModel gsdScaner1 = new DeviceModel
                {
                    DeviceName = "Scaner_13_040",
                    Station = "HIKVISION-MV_1",
                    GsdName = "GSDML-V2.34-HIKVISION-MV-20210719.XML",
                    GsdType = "DAP",
                    GsdId = "DAP 3",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {gsdScaner1.DeviceName} TypeId: {gsdScaner1.TypeIdentifier}");

                tia.DeviceService.AddDeviceToUngrouped(gsdScaner1);

                DeviceModel gsdScaner2 = new DeviceModel
                {
                    DeviceName = "Scaner_13_100",
                    Station = "HIKVISION-MV_2",
                    GsdName = "GSDML-V2.34-HIKVISION-MV-20210719.XML",
                    GsdType = "DAP",
                    GsdId = "DAP 3",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {gsdScaner2.DeviceName} TypeId: {gsdScaner2.TypeIdentifier}");

                tia.DeviceService.AddDeviceToUngrouped(gsdScaner2);

                DeviceModel gsdOdotHeadModule1 = new DeviceModel
                {
                    DeviceName = "TB09_010_K002",
                    Station = "ODOT-BN8032_1",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "DAP",
                    GsdId = "DAP1",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {gsdOdotHeadModule1.DeviceName} TypeId: {gsdOdotHeadModule1.TypeIdentifier}");

                var odot1 = tia.DeviceService.AddDeviceToUngrouped(gsdOdotHeadModule1);

                DeviceModel gsdOdotHeadModule2 = new DeviceModel
                {
                    DeviceName = "TB09_070_K002",
                    Station = "ODOT-BN8032_2",
                    GsdName = "GSDML-V2.33-ODOT-BN8032-20230911.XML",
                    GsdType = "DAP",
                    GsdId = "DAP1",
                    IncludeFailsafe = false,
                    PositionNumber = 0
                };
                Logger.Add($"Device: {gsdOdotHeadModule2.DeviceName} TypeId: {gsdOdotHeadModule2.TypeIdentifier}");

                var odot2 = tia.DeviceService.AddDeviceToUngrouped(gsdOdotHeadModule2);

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
                Logger.Add($"DeviceItem: {gsdOdotImodule1.DeviceName} TypeId: {gsdOdotImodule1.TypeIdentifier}");

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
                Logger.Add($"DeviceItem: {gsdOdotImodule2.DeviceName} TypeId: {gsdOdotImodule2.TypeIdentifier}");

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
                Logger.Add($"DeviceItem: {gsdOdotOmodule1.DeviceName} TypeId: {gsdOdotOmodule1.TypeIdentifier}");

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
                Logger.Add($"DeviceItem: {gsdOdotIOmodule1.DeviceName} TypeId: {gsdOdotIOmodule1.TypeIdentifier}");

                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotImodule1);
                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotImodule2);
                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotOmodule1);
                tia.DeviceService.AddDeviceItemToDevice(odot1, gsdOdotIOmodule1);

                tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotImodule1);
                tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotImodule2);
                tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotOmodule1);
                tia.DeviceService.AddDeviceItemToDevice(odot2, gsdOdotIOmodule1);

                Console.WriteLine("[OK] Устройства успешно добавлены в проект.");
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

