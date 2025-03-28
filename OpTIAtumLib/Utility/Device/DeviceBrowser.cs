using Siemens.Engineering;
using Siemens.Engineering.HW;
using OpTIAtumLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpTIAtumLib.Utility.Device
{
    /// <summary>
    /// Утилита для извлечения информации об устройствах и модулях из проекта TIA.
    /// </summary>
    public static class DeviceBrowser
    {
        /// <summary>
        /// Возвращает список моделей устройств на основе TypeIdentifier и вложенных DeviceItem.
        /// </summary>
        public static List<DeviceModel> GetDeviceModels(Project project)
        {
            var result = new List<DeviceModel>();

            foreach (var device in project.Devices)
            {
                string station = device.Name;

                foreach (DeviceItem topItem in device.DeviceItems)
                {
                    foreach (var deviceItem in GetAllDeviceItems(topItem))
                    {
                        string typeIdentifier = deviceItem.TypeIdentifier;
                        string orderNumber = null;
                        string firmwareVersion = null;
                        string gsdName = null;
                        string gsdType = null;
                        string gsdId = null;

                        if (!string.IsNullOrEmpty(typeIdentifier) && typeIdentifier.StartsWith("OrderNumber:"))
                        {
                            var parts = typeIdentifier.Replace("OrderNumber:", "").Split('/');
                            orderNumber = parts.ElementAtOrDefault(0);
                            firmwareVersion = parts.ElementAtOrDefault(1);
                        }
                        else if (!string.IsNullOrEmpty(typeIdentifier) && typeIdentifier.StartsWith("GSD:"))
                        {
                            var parts = typeIdentifier.Replace("GSD:", "").Split('/');
                            gsdName = parts.ElementAtOrDefault(0);
                            gsdType = parts.ElementAtOrDefault(1);
                            gsdId = parts.ElementAtOrDefault(2);
                        }

                        var model = new DeviceModel
                        {
                            Station = station,
                            DeviceName = deviceItem.Name,
                            OrderNumber = orderNumber,
                            FirmwareVersion = firmwareVersion,
                            GsdName = gsdName,
                            GsdType = gsdType,
                            GsdId = gsdId,
                            PositionNumber = deviceItem.PositionNumber,
                            ClassType = ConvertClassification(deviceItem.Classification)
                        };

                        result.Add(model);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Рекурсивный обход всех DeviceItem.
        /// </summary>
        private static IEnumerable<DeviceItem> GetAllDeviceItems(DeviceItem item)
        {
            yield return item;
            foreach (var child in item.DeviceItems)
                foreach (var nested in GetAllDeviceItems(child))
                    yield return nested;
        }

        /// <summary>
        /// Преобразование из Openness классификации в DeviceClassType проекта.
        /// </summary>
        private static DeviceClassType ConvertClassification(DeviceItemClassifications classification)
        {
            return Enum.TryParse(classification.ToString(), true, out DeviceClassType result)
                ? result
                : DeviceClassType.Undefined;
        }

        public static List<DeviceModel> GetUngroupedDeviceModels(Project project)
        {
            var result = new List<DeviceModel>();

            // Пытаемся найти группу UngroupedDevicesGroup
            var ungroupedGroup = project.DeviceGroups.Find("UngroupedDevicesGroup");
            if (ungroupedGroup == null)
                
                return result;

            foreach (var device in project.Devices)
            {
                string station = device.Name;

                foreach (DeviceItem topItem in device.DeviceItems)
                {
                    foreach (var deviceItem in GetAllDeviceItems(topItem))
                    {
                        string typeIdentifier = deviceItem.TypeIdentifier;
                        string orderNumber = null;
                        string firmwareVersion = null;
                        string gsdName = null;
                        string gsdType = null;
                        string gsdId = null;

                        if (!string.IsNullOrEmpty(typeIdentifier) && typeIdentifier.StartsWith("OrderNumber:"))
                        {
                            var parts = typeIdentifier.Replace("OrderNumber:", "").Split('/');
                            orderNumber = parts.ElementAtOrDefault(0);
                            firmwareVersion = parts.ElementAtOrDefault(1);
                        }
                        else if (!string.IsNullOrEmpty(typeIdentifier) && typeIdentifier.StartsWith("GSD:"))
                        {
                            var parts = typeIdentifier.Replace("GSD:", "").Split('/');
                            gsdName = parts.ElementAtOrDefault(0);
                            gsdType = parts.ElementAtOrDefault(1);
                            gsdId = parts.ElementAtOrDefault(2);
                        }

                        var model = new DeviceModel
                        {
                            Station = station,
                            DeviceName = deviceItem.Name,
                            OrderNumber = orderNumber,
                            FirmwareVersion = firmwareVersion,
                            GsdName = gsdName,
                            GsdType = gsdType,
                            GsdId = gsdId,
                            PositionNumber = deviceItem.PositionNumber,
                            ClassType = ConvertClassification(deviceItem.Classification)
                        };

                        result.Add(model);
                    }
                }
            }

            return result;
        }
    }
}
