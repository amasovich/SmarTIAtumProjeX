using System;
using System.IO;
using OpTIAtumLib.Core;
using OpTIAtumLib.Model;
using OpTIAtumLib.Service.Projects;
using OpTIAtumLib.Service.TIASession;
using OpTIAtumLib.Utility.Logger;
using OpTIAtumLib.Utility.Device;

namespace SmarTIAtumProjeX.Test
{
    internal class TestCreateProfinetSubnet
    {
        public static void Run()
        {
            try
            {
                Logger.Info("Запуск теста создания подсети...");

                // Создание фасада TIA
                var tia = new TIAConnector();
                tia.SessionService.CreateTIAInstance(enableGuiTIA: true);
                var instance = ((TIASessionService)tia.SessionService).Instance;

                string projectName = "SubnetTestProject";
                string projectPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TIA_Projects");

                if (!Directory.Exists(projectPath))
                    Directory.CreateDirectory(projectPath);

                var project = new ProjectService(instance).CreateProject(projectPath, projectName);
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

                //просмотр текущих интерфейсов в ПЛК
                DeviceBrowser.UpdateNetworkInterfaces(tia.Project, plc);

                Logger.Info($"Создаём подсеть:");
                SubnetModel Profinet1 = new SubnetModel
                {
                    SubnetName = "PROFINET1",
                    SubnetType = "PROFINET"
                };
                Logger.Add($"Subnet: {Profinet1.SubnetName} TypeId: {Profinet1.TypeIdentifier}");

                // Создание подсети
                var subnetProfinet1 = tia.SubnetService.CreateSubnet(Profinet1);

            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при создании подсети: " + ex.Message);
            }

            Logger.Info("Тест завершён. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
