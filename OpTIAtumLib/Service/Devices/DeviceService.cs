using OpTIAtumLib.Model;
using OpTIAtumLib.Utility.Logger;
using OpTIAtumLib.Utility.Guards;
using OpTIAtumLib.Utility.Validation;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using System;
using System.Linq;
using Siemens.Engineering.HmiUnified.HmiLogging.HmiLoggingCommon;

namespace OpTIAtumLib.Service.Devices
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
            Guard.ProjectInitialized(_project);
            DeviceModelValidator.Validate(deviceModel);

            string typeIdentifier = deviceModel.TypeIdentifier;
            string deviceName = deviceModel.DeviceName;
            string stationName = deviceModel.Station;

            Logger.Debug($"Добавление устройства '{deviceName}' типа '{typeIdentifier}' на станцию '{stationName}' в корень проекта.");

            try
            {
                Device createdDevice = _project.Devices.CreateWithItem(typeIdentifier, deviceName, stationName);
                Logger.Create($"Устройство '{deviceName}' успешно добавлено: {typeIdentifier} → Станция: {stationName}");
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
            Guard.ProjectInitialized(_project);
            DeviceModelValidator.Validate(deviceModel);

            string typeIdentifier = deviceModel.TypeIdentifier;
            string deviceName = deviceModel.DeviceName;
            string stationName = deviceModel.Station;

            Logger.Debug($"Добавление устройства '{deviceName}' типа '{typeIdentifier}' на станцию '{stationName}' в корень проекта.");

            try
            {
                Device createdDevice = targetGroup.Devices.CreateWithItem(typeIdentifier, deviceName, stationName);
                Logger.Create($"Устройство '{deviceName}' успешно добавлено в группу '{targetGroup.Name}': {typeIdentifier} → Станция: {stationName}");
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
            Guard.ProjectInitialized(_project);
            DeviceModelValidator.Validate(deviceModel);

            string typeIdentifier = deviceModel.TypeIdentifier;
            string deviceName = deviceModel.DeviceName;
            string stationName = deviceModel.Station;

            Logger.Debug($"Добавление устройства '{deviceName}' типа '{typeIdentifier}' на станцию '{stationName}' в корень проекта.");

            try
            {
                var group = _project.UngroupedDevicesGroup;
                Device createdDevice = group.Devices.CreateWithItem(typeIdentifier, deviceName, stationName);

                Logger.Create($"Устройство '{deviceName}' добавлено в UngroupedDevicesGroup: {typeIdentifier} → Станция: {stationName}");
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
            Guard.ProjectInitialized(_project);
            Guard.NotNull(device, nameof(device));
            DeviceModelValidator.Validate(moduleModel);

            string typeIdentifier = moduleModel.TypeIdentifier;
            string name = moduleModel.DeviceName;
            int position = moduleModel.PositionNumber;

            Logger.Debug($"Инициализация добавления модуля '{name}' в устройство '{device.Name}' на позицию {position}, тип '{typeIdentifier}'.");

            var parent = device.DeviceItems
                .FirstOrDefault(di => di.GetPlugLocations().Any(pl => pl.PositionNumber == position));

            Guard.OperationValid(parent != null, "Подходящий слот для модуля не найден.");
            Guard.OperationValid(parent.CanPlugNew(typeIdentifier, name, position), $"Невозможно вставить модуль '{name}' на позицию {position}.");

            Logger.Debug($"Попытка вставить модуль '{name}' в слот {position} устройства '{device.Name}'.");

            try
            {
                DeviceItem module = parent.PlugNew(typeIdentifier, name, position);
                Logger.Create($"Модуль '{name}' добавлен в устройство '{device.Name}' на позицию {position}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении модуля '{name}': {ex.Message}");
                throw;
            }
        }
    }
}
