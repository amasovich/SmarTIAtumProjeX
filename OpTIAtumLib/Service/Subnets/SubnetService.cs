using OpTIAtumLib.Model;
using OpTIAtumLib.Utility.Device;
using OpTIAtumLib.Utility.Guards;
using OpTIAtumLib.Utility.Logger;
using Siemens.Engineering;
using Siemens.Engineering.HmiUnified.HmiLogging.HmiLoggingCommon;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using System;
using System.Linq;

namespace OpTIAtumLib.Service.Subnets
{
    internal class SubnetService : ISubnetService
    {
        private readonly Project _project;

        public SubnetService(Project project)
        {
            _project = project ?? throw new ArgumentNullException(nameof(project));
        }

        /// <inheritdoc/>
        public Subnet CreateSubnet(SubnetModel subnetModel)
        {
            Guard.ProjectInitialized(_project);
            Guard.NotNull(subnetModel, nameof(subnetModel));

            string subnetName = subnetModel.SubnetName;
            string typeIdentifier = subnetModel.TypeIdentifier;

            Guard.NotNullOrWhiteSpace(subnetName, nameof(subnetName));
            Guard.NotNullOrWhiteSpace(typeIdentifier, nameof(typeIdentifier));

            Logger.Debug($"Попытка создать подсеть '{subnetName}' с типом '{typeIdentifier}'.");

            try
            {
                Subnet newSubnet = _project.Subnets.Create(typeIdentifier, subnetName);

                Logger.Create($"Подсеть '{subnetName}' типа '{typeIdentifier}' успешно создана.");
                return newSubnet;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при создании подсети '{subnetName}': {ex.Message}");
                throw;
            }
        }

        public void ConnectDeviceToSubnet(DeviceModel deviceModel)
        {
            Guard.ProjectInitialized(_project);
            Guard.NotNull(deviceModel, nameof(deviceModel));

            Logger.Debug($"Запуск подключения устройства '{deviceModel.Station}' к подсетям...");

            Guard.OperationValid(
                deviceModel.NetworkInterfaceNames != null &&
                deviceModel.SubnetNames != null &&
                deviceModel.SubnetTypes != null,
                "Отсутствуют полные данные для подключения интерфейсов и подсетей.");

            Guard.OperationValid(
                deviceModel.NetworkInterfaceNames.Count == deviceModel.SubnetNames.Count &&
                deviceModel.NetworkInterfaceNames.Count == deviceModel.SubnetTypes.Count,
                "Количество интерфейсов, подсетей и типов подсетей не совпадает.");

            var tiaDevice = _project.Devices.FirstOrDefault(d => d.Name == deviceModel.Station);

            Guard.OperationValid(tiaDevice != null, $"Устройство с именем '{deviceModel.Station}' не найдено в проекте.");

            Logger.Debug($"Найдено устройство TIA: '{tiaDevice.Name}'");

            foreach (var interfaceName in deviceModel.NetworkInterfaceNames.Select((name, index) => new { name, index }))
            {
                var subnetName = deviceModel.SubnetNames[interfaceName.index];
                var tiaSubnet = _project.Subnets.FirstOrDefault(s => s.Name == subnetName);

                Logger.Debug($"Обработка пары: Интерфейс = '{interfaceName.name}', Подсеть = '{subnetName}'");

                Guard.OperationValid(tiaSubnet != null, $"Подсеть с именем '{subnetName}' не найдена в проекте.");

                NetworkInterface networkInterface = null;

                foreach (var topItem in tiaDevice.DeviceItems)
                {
                    var allItems = DeviceBrowser.GetAllDeviceItems(topItem).ToList();
                    Logger.Debug($"Всего найдено DeviceItem: {allItems.Count}");

                    foreach (var di in allItems)
                    {
                        Logger.Debug($"DeviceItem: '{di.Name}' | Type: '{di.GetType().Name}'");

                        var iface = di.GetService<NetworkInterface>();
                        if (iface != null)
                        {
                            Logger.Debug($"Найден интерфейс: '{di.Name}' / Mode: '{iface.InterfaceOperatingMode}'");
                            if (di.Name == interfaceName.name)
                            {
                                networkInterface = iface;
                                break;
                            }
                        }
                    }

                    if (networkInterface != null)
                        break;
                }

                Guard.OperationValid(networkInterface != null, $"Интерфейс '{interfaceName.name}' не найден у устройства '{deviceModel.Station}'.");

                var node = networkInterface.Nodes.FirstOrDefault();

                Guard.OperationValid(node != null, $"У интерфейса '{interfaceName.name}' устройства '{deviceModel.Station}' отсутствуют узлы для подключения.");

                try
                {
                    Logger.Debug($"Выполняем подключение: '{interfaceName.name}' → '{subnetName}'");

                    node.ConnectToSubnet(tiaSubnet);

                    Logger.Info($"Устройство '{deviceModel.Station}' успешно подключено к подсети '{subnetName}' через интерфейс '{interfaceName.name}'.");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Ошибка при подключении интерфейса '{interfaceName.name}' к подсети '{subnetName}' у устройства '{deviceModel.Station}': {ex.Message}");
                    throw;
                }
            }
        }
    }
}
