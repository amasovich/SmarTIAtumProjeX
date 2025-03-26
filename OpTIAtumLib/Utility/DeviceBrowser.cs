using System.Collections.Generic;
using System.Linq;
using OpTIAtumLib.Model;
using OpTIAtumLib.Utility;
using Siemens.Engineering;
using Siemens.Engineering.HW;

namespace OpTIAtumLib.Tools
{
    /// <summary>
    /// Вспомогательный класс для получения информации об устройствах из проекта TIA.
    /// </summary>
    internal static class DeviceBrowser
    {
        /// <summary>
        /// Возвращает список всех устройств проекта как список DeviceModel.
        /// </summary>
        /// <param name="project">Открытый проект TIA</param>
        /// <returns>Список устройств в виде моделей DeviceModel</returns>
        public static List<DeviceModel> GetDeviceModels(Project project)
        {
            var result = new List<DeviceModel>();

            if (project == null)
                return result;

            foreach (var device in project.Devices)
            {
                string order = GetAttribute(device, "OrderNumber");
                string version = GetAttribute(device, "Version");

                var model = new DeviceModel
                {
                    DeviceName = device.Name,
                    OrderNumber = order,
                    FirmwareVersion = version,
                    Station = device.Name, // можно заменить, если есть более точное имя станции
                    ClassType = DeviceClassifier.DetectClassType(order)
                };

                result.Add(model);
            }

            return result;
        }

        private static string GetAttribute(Device device, string attributeName)
        {
            try
            {
                var item = device.DeviceItems.FirstOrDefault();
                if (item != null && item.GetAttribute(attributeName) is string value)
                    return value;
            }
            catch { }

            return string.Empty;
        }
    }
}
