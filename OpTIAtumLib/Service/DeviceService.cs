using OpTIAtumLib.Model;
using OpTIAtumLib.Utility;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using System;
using System.Linq;
using System.Collections.Generic;
using OpTIAtumLib.Interface;

namespace OpTIAtumLib.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly Project _project;

        public DeviceService(Project project)
        {
            _project = project ?? throw new ArgumentNullException(nameof(project));
        }

        /// <inheritdoc/>
        public Device AddDeviceToProject(DeviceModel deviceModel)
        {
            if (_project == null)
                throw new InvalidOperationException("TIA Project не открыт. Сначала открой или создай проект.");

            if (deviceModel == null)
                throw new ArgumentNullException(nameof(deviceModel), "Модель устройства не может быть null.");

            string typeIdentifier = deviceModel.TypeIdentifier;
            string deviceName = deviceModel.DeviceName;
            string stationName = deviceModel.Station;

            if (string.IsNullOrWhiteSpace(typeIdentifier))
                throw new ArgumentException("TypeIdentifier обязателен для создания устройства.");

            if (string.IsNullOrWhiteSpace(deviceName))
                throw new ArgumentException("DeviceName обязателен для создания устройства.");

            try
            {
                Device createdDevice = _project.Devices.CreateWithItem(typeIdentifier, deviceName, stationName);
                Logger.Info($"Устройство '{deviceName}' успешно добавлено: {typeIdentifier} → Станция: {stationName}");
                return createdDevice;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении устройства '{deviceName}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public Device AddDeviceToProject(DeviceModel deviceModel, DeviceUserGroup targetGroup)
        {
            if (_project == null)
                throw new InvalidOperationException("TIA Project не открыт. Сначала открой или создай проект.");

            if (deviceModel == null)
                throw new ArgumentNullException(nameof(deviceModel), "Модель устройства не может быть null.");

            string typeIdentifier = deviceModel.TypeIdentifier;
            string deviceName = deviceModel.DeviceName;
            string stationName = deviceModel.Station;

            if (string.IsNullOrWhiteSpace(typeIdentifier))
                throw new ArgumentException("TypeIdentifier обязателен для создания устройства.");

            if (string.IsNullOrWhiteSpace(deviceName))
                throw new ArgumentException("DeviceName обязателен для создания устройства.");

            try
            {
                Device createdDevice = targetGroup.Devices.CreateWithItem(typeIdentifier, deviceName, stationName);
                Logger.Info($"Устройство '{deviceName}' успешно добавлено в группу '{targetGroup.Name}': {typeIdentifier} → Станция: {stationName}");
                return createdDevice;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении устройства '{deviceName}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public Device AddDeviceToUngrouped(DeviceModel deviceModel)
        {
            if (_project == null)
                throw new InvalidOperationException("TIA Project не открыт. Сначала открой или создай проект.");

            if (deviceModel == null)
                throw new ArgumentNullException(nameof(deviceModel), "Модель устройства не может быть null.");

            string typeIdentifier = deviceModel.TypeIdentifier;
            string deviceName = deviceModel.DeviceName;
            string stationName = deviceModel.Station;

            if (string.IsNullOrWhiteSpace(typeIdentifier))
                throw new ArgumentException("TypeIdentifier обязателен для создания устройства.");

            if (string.IsNullOrWhiteSpace(deviceName))
                throw new ArgumentException("DeviceName обязателен для создания устройства.");

            try
            {
                var group = _project.UngroupedDevicesGroup;
                Device createdDevice = group.Devices.CreateWithItem(typeIdentifier, deviceName, stationName);

                Logger.Info($"Устройство '{deviceName}' добавлено в UngroupedDevicesGroup: {typeIdentifier} → Станция: {stationName}");
                return createdDevice;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении устройства '{deviceName}' в UngroupedDevicesGroup: {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public void AddDeviceItemToDevice(Device device, DeviceModel moduleModel)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            string typeIdentifier = moduleModel.TypeIdentifier;
            string name = moduleModel.DeviceName;
            int position = moduleModel.PositionNumber;

            var parent = device.DeviceItems
                .FirstOrDefault(di => di.GetPlugLocations().Any(pl => pl.PositionNumber == position));

            if (parent == null)
                throw new InvalidOperationException("Подходящий слот для модуля не найден.");

            if (!parent.CanPlugNew(typeIdentifier, name, position))
                throw new InvalidOperationException($"Невозможно вставить модуль '{name}' на позицию {position}.");

            try
            {
                DeviceItem module = parent.PlugNew(typeIdentifier, name, position);
                Logger.Info($"Модуль '{name}' добавлен в устройство '{device.Name}' на позицию {position}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении модуля '{name}': {ex.Message}");
                throw;
            }
        }
    }
}
