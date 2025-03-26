using Siemens.Engineering;
using System.Collections.Generic;
using OpTIAtumLib.Model;
using Siemens.Engineering.HW;

namespace OpTIAtumLib.Interface
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
    }
}

