using Siemens.Engineering;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
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
        /// Возвращает список моделей устройств на основе TypeIdentifier и вложенных DeviceItem.
        /// </summary>
        public static List<DeviceModel> GetUngroupedDeviceModels(Project project)
        {
            var result = new List<DeviceModel>();

            foreach (var device in project.UngroupedDevicesGroup.Devices)
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
        public static IEnumerable<DeviceItem> GetAllDeviceItems(DeviceItem item)
        {
            yield return item;
            foreach (var child in item.DeviceItems)
                foreach (var nested in GetAllDeviceItems(child))
                    yield return nested;
        }

        /// <summary>
        /// Преобразование из встроенной классификации Openness в собственный тип <see cref="DeviceClassType"/>.
        /// При значении <c>None</c> применяется автоопределение по OrderNumber.
        /// </summary>
        /// <param name="classification">Классификация от Openness (DeviceItemClassifications)</param>
        /// <param name="orderNumber">OrderNumber устройства (опционально, для автоопределения)</param>
        /// <returns>Соответствующий тип <see cref="DeviceClassType"/></returns>
        public static DeviceClassType ConvertClassification(DeviceItemClassifications classification, string orderNumber = null)
        {
            // Попробуем напрямую по enum'у
            if (Enum.TryParse(classification.ToString(), true, out DeviceClassType result) &&
                result != DeviceClassType.None)
            {
                return result;
            }

            // Попробуем определить вручную через OrderNumber
            return DeviceClassifier.DetectClassType(orderNumber);
        }

        public static void GetNetworkInterfaces(Project project, DeviceModel deviceModel)
        {
            if (project == null)
                throw new InvalidOperationException("TIA Project не инициализирован. Сначала вызовите Initialize().");

            if (deviceModel == null)
                throw new ArgumentNullException(nameof(deviceModel), "Модель устройства не может быть null.");

            if (string.IsNullOrWhiteSpace(deviceModel.Station))
                throw new ArgumentException("Имя станции (Station) в модели не задано.");

            // Найти устройство в проекте по имени станции
            var device = project.Devices.FirstOrDefault(d => d.Name == deviceModel.Station);
            if (device == null)
            {
                Console.WriteLine($"Устройство с именем '{deviceModel.Station}' не найдено в проекте.");
                return;
            }

            var interfaces = new List<string>();

            foreach (var topItem in device.DeviceItems)
            {
                foreach (var deviceItem in DeviceBrowser.GetAllDeviceItems(topItem))
                {
                    var networkInterface = deviceItem.GetService<NetworkInterface>();
                    if (networkInterface != null)
                    {
                        interfaces.Add(deviceItem.Name);
                    }
                }
            }

            // Обновление модели устройства списком найденных интерфейсов
            deviceModel.NetworkInterfaceNames = interfaces;

            // Вывод информации о найденных интерфейсах
            if (interfaces.Count > 0)
            {
                Console.WriteLine($"Найдены сетевые интерфейсы для '{deviceModel.Station}': {string.Join(", ", interfaces)}");
            }
            else
            {
                Console.WriteLine($"Сетевые интерфейсы для '{deviceModel.Station}' не найдены.");
            }
        }
    }
}
