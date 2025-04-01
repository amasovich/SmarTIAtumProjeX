using System;
using System.IO;
using OpTIAtumLib.Core;
using OpTIAtumLib.Model;
using OpTIAtumLib.Service.Projects;
using OpTIAtumLib.Service.TIASession;
using OpTIAtumLib.Utility.Logger;
using OpTIAtumLib.Utility.Device;
using System.Collections.Generic;

namespace SmarTIAtumProjeX.Test
{
    internal class TestCreateProfinetSubnet
    {
        public static void Run()
        {
            try
            {
                Logger.Info("Запуск теста создания подсетей и подключения интерфейсов...");

                var tia = new TIAConnector();
                tia.SessionService.CreateTIAInstance(enableGuiTIA: true);
                var instance = ((TIASessionService)tia.SessionService).Instance;

                string projectName = "SubnetTestProject";
                string projectPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TIA_Projects");

                if (!Directory.Exists(projectPath))
                    Directory.CreateDirectory(projectPath);

                var project = new ProjectService(instance).CreateProject(projectPath, projectName);
                tia.Initialize(instance, project);

                Logger.Info("Создаём устройство с интерфейсами и подсетями:");
                DeviceModel plc = new DeviceModel
                {
                    DeviceName = "MDC2_K001",
                    Station = "S71500 station_1",
                    OrderNumber = "6ES7 517-3FP00-0AB0",
                    FirmwareVersion = "V3.0",
                    IncludeFailsafe = false,
                    PositionNumber = 0,
                    NetworkInterfaceNames = new List<string> { "PROFINET interface_1", "PROFINET interface_2" },
                    SubnetNames = new List<string> { "PROFINET_1", "PROFINET_2" },
                    SubnetTypes = new List<string> { "PROFINET", "PROFINET" }
                };
                Logger.Add($"Device: {plc.DeviceName} TypeId: {plc.TypeIdentifier}");

                tia.DeviceService.AddDeviceToProject(plc);

                Logger.Info("Создаём подсети:");
                for (int i = 0; i < plc.SubnetNames.Count; i++)
                {
                    SubnetModel subnet = new SubnetModel
                    {
                        SubnetName = plc.SubnetNames[i],
                        SubnetType = plc.SubnetTypes[i]
                    };
                    Logger.Add($"Subnet: {subnet.SubnetName} TypeId: {subnet.TypeIdentifier}");

                    tia.SubnetService.CreateSubnet(subnet);
                }

                tia.SubnetService.ConnectDeviceToSubnet(plc);

            }
            catch (Exception ex)
            {
                Logger.Error("Ошибка при создании подсети или подключении интерфейсов: " + ex.Message);
            }

            Logger.Info("Тест завершён. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}