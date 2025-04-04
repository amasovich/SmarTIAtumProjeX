using OpTIAtumLib.Model;
using Siemens.Engineering.HW;
using System.Collections.Generic;

namespace OpTIAtumLib.Service.Devices
{
    /// <summary>
    /// Интерфейс для сервиса добавления устройств и модулей в проект TIA Portal.
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// Добавляет устройство в проект TIA Portal на основе переданной модели.
        /// Поддерживает как стандартные устройства (PLC, ET200, HMI и др.), так и GSD/GSDML.
        /// </summary>
        /// <param name="deviceModel">
        /// Модель устройства, содержащая необходимые данные:
        /// <list type="bullet">
        ///   <item><term>TypeIdentifier</term> — полный идентификатор устройства, например:
        ///     <c>OrderNumber:6ES7 518-4FP00-0AB0/V3.0</c> или <c>GSD:GSDML-...</c></item>
        ///   <item><term>DeviceName</term> — имя устройства, отображаемое в TIA проекте</item>
        ///   <item><term>Station</term> — имя станции или узла в сетевом представлении</item>
        /// </list>
        /// </param>
        /// <returns>
        /// Объект <see cref="Device"/> — добавленное устройство в проект TIA.
        /// Его можно использовать для последующего добавления модулей через <see cref="AddDeviceItemToDevice(Device, DeviceModel)"/>
        /// </returns>
        Device AddDeviceToProject(DeviceModel deviceModel);

        /// <summary>
        /// Добавляет устройство в проект TIA Portal в указанную пользовательскую группу устройств (папку проекта).
        /// Метод расширяет стандартную вставку, позволяя разместить устройство, например, в <c>UngroupedDevicesGroup</c>.
        /// Поддерживает как стандартные устройства (PLC, ET200, HMI и др.), так и GSD/GSDML.
        /// </summary>
        /// <param name="deviceModel">
        /// Модель устройства, содержащая необходимые данные:
        /// <list type="bullet">
        ///   <item><term>TypeIdentifier</term> — полный идентификатор устройства, например:
        ///     <c>OrderNumber:6ES7 518-4FP00-0AB0/V3.0</c> или <c>GSD:GSDML-...</c></item>
        ///   <item><term>DeviceName</term> — имя устройства, отображаемое в TIA проекте</item>
        ///   <item><term>Station</term> — имя станции или узла в сетевом представлении</item>
        /// </list>
        /// </param>
        /// <param name="targetGroup">
        /// Группа устройств (папка в проекте), в которую будет добавлено устройство. Например:
        /// <see cref="Siemens.Engineering.HW.DeviceGroups.UngroupedDevicesGroup"/>.
        /// </param>
        /// <returns>
        /// Объект <see cref="Device"/> — добавленное устройство в проект TIA в указанную группу.
        /// Его можно использовать для последующего добавления модулей через <see cref="AddDeviceItemToDevice(Device, DeviceModel)"/>
        /// </returns>
        Device AddDeviceToProject(DeviceModel deviceModel, DeviceUserGroup targetGroup);

        /// <summary>
        /// Добавляет устройство в специальную группу проекта TIA Portal — "UngroupedDevicesGroup".
        /// Метод полезен при работе с GSD/GSDML-устройствами, которые по умолчанию могут не вставляться в основную структуру.
        /// </summary>
        /// <param name="deviceModel">
        /// Модель устройства, содержащая необходимые данные:
        /// <list type="bullet">
        ///   <item><term>TypeIdentifier</term> — полный идентификатор устройства, например:
        ///     <c>GSD:GSDML-V2.34-.../DAP/...</c></item>
        ///   <item><term>DeviceName</term> — имя устройства, отображаемое в TIA проекте</item>
        ///   <item><term>Station</term> — имя станции или узла в сетевом представлении</item>
        /// </list>
        /// </param>
        /// <returns>
        /// Объект <see cref="Device"/> — добавленное устройство, помещённое в "UngroupedDevicesGroup".
        /// Его можно использовать для дальнейшего конфигурирования или добавления модулей.
        /// </returns>
        /// <example>
        /// <code>
        /// var scaner = new DeviceModel
        /// {
        ///     DeviceName = "Scaner_13_040",
        ///     Station = "GSD device_1",
        ///     GsdName = "GSDML-V2.34-HIKVISION-MV-20210719.XML",
        ///     GsdType = "DAP",
        ///     GsdId = "DAP 3",
        ///     PositionNumber = 0
        /// };
        /// var device = tia.DeviceService.AddDeviceToUngrouped(scaner);
        /// </code>
        /// </example>
        Device AddDeviceToUngrouped(DeviceModel deviceModel);

        /// <summary>
        /// Добавляет модуль (DeviceItem) в заданное устройство (Device) на указанную позицию.
        /// Используется для вставки модулей GSD/GSDML-устройств, например: входы/выходы, подмодули.
        /// </summary>
        /// <param name="device">
        /// Объект устройства, в которое будет вставлен модуль.
        /// Должен быть предварительно добавлен в проект с помощью <see cref="AddDeviceToProject(DeviceModel)"/>.
        /// </param>
        /// <param name="moduleModel">
        /// Модель модуля, содержащая TypeIdentifier (например, GSD:.../M/...),
        /// имя устройства и номер позиции (слота), в которую следует вставить модуль.
        /// </param>
        void AddDeviceItemToDevice(Device device, DeviceModel moduleModel);

        /// <summary>
        /// Возвращает все устройства, добавленные в проект.
        /// </summary>
        //IEnumerable<DeviceModel> GetAllDevices();

        /// <summary>
        /// Ищет устройство по имени. Возвращает null, если не найдено.
        /// </summary>
        //DeviceModel? FindDeviceByName(string name);

        /// <summary>
        /// Возвращает устройства, подключённые к указанной подсети.
        /// </summary>
        //IEnumerable<DeviceModel> GetDevicesBySubnet(string subnetName);

        /// <summary>
        /// Удаляет устройство из проекта по имени.
        /// </summary>
        //bool RemoveDevice(string deviceName);

        /// <summary>
        /// Переименовывает устройство, если имя доступно.
        /// </summary>
        //bool RenameDevice(string currentName, string newName);

        /// <summary>
        /// Создаёт копию существующего устройства с новым именем.
        /// </summary>
        //Device CloneDevice(string name, string newName);

        /// <summary>
        /// Валидирует модель устройства с применением внутренних правил.
        /// </summary>
        //void ValidateDevice(DeviceModel model);

        /// <summary>
        /// Логически включает или отключает устройство (например, при генерации).
        /// </summary>
        //bool SetDeviceEnabled(string name, bool isEnabled);

    }
}

