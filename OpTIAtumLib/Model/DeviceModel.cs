﻿using OpTIAtumLib.Utility.Device;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpTIAtumLib.Model
{
    /// <summary>
    /// Представляет модель устройства TIA Portal, содержащую ключевую информацию
    /// для создания и конфигурации устройства и его модулей.
    /// </summary>
    public class DeviceModel
    {
        #region main data

        /// <summary>
        /// Название станции (например, "S71500 station_1").
        /// </summary>
        public string Station { get; set; }

        /// <summary>
        /// Имя устройства (отображаемое имя в проекте).
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Артикул устройства (Order Number), например "6ES7 517-3FP00-0AB0".
        /// Применимо для стандартных устройств.
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Версия прошивки устройства (например, "V3.0").
        /// </summary>
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Название GSD-файла для GSD-устройств.
        /// </summary>
        public string GsdName { get; set; }

        /// <summary>
        /// Тип GSD-устройства (например, "DAP", "M").
        /// </summary>
        public string GsdType { get; set; }

        /// <summary>
        /// Идентификатор GSD-устройства (например, "DAP3", "IDM_BT124F").
        /// </summary>
        public string GsdId { get; set; }

        /// <summary>
        /// Указывает, включает ли устройство Failsafe функциональность.
        /// </summary>
        public bool IncludeFailsafe { get; set; }

        /// <summary>
        /// Позиция устройства в станции (например, 0, 1, 2).
        /// </summary>
        public int PositionNumber { get; set; }

        /// <summary>
        /// Имя родительского устройства, если текущая модель — модуль.
        /// Используется при импорте из внешних данных для привязки.
        /// </summary>
        public string ParentDeviceName { get; set; }

        #endregion // main data

        #region network data

        /// <summary>
        /// Имена интерфейсов устройства, которые следует подключить к подсетям.
        /// Если указано несколько, подключение выполняется ко всем по имени.
        /// </summary>
        public List<string> NetworkInterfaceNames { get; set; }

        /// <summary>
        /// Список имён портов (например, "Port_1", "Port_2"), доступных в интерфейсе.
        /// Можно использовать для более точного подключения.
        /// </summary>
        public List<string> NetworkPortNames { get; set; }

        /// <summary>
        /// Имена подсетей, к которым должно быть подключено устройство.
        /// Каждое имя будет сопоставлено с соответствующим сетевым интерфейсом.
        /// </summary>
        public List<string> SubnetNames { get; set; }

        /// <summary>
        /// Упрощённые имена типов подсетей, соответствующие списку подсетей (например, "PROFINET", "PROFIBUS", "MPI", "ASI").
        /// </summary>
        public List<string> SubnetTypes { get; set; }

        #endregion // network data

        #region additional data

        /// <summary>
        /// Генерируемый TypeIdentifier на основе GSD или OrderNumber.
        /// </summary>
        public string TypeIdentifier =>
        !string.IsNullOrWhiteSpace(GsdName) && !string.IsNullOrWhiteSpace(GsdType)
        ? $"GSD:{GsdName}/{GsdType}" + (string.IsNullOrWhiteSpace(GsdId) ? "" : $"/{GsdId}")
        : $"OrderNumber:{OrderNumber}" + (string.IsNullOrWhiteSpace(FirmwareVersion) ? "" : $"/{FirmwareVersion}");

        private DeviceClassType? _classType;

        /// <summary>
        /// Класс устройства (PLC, HMI, IO и т.д.).
        /// Автоматически определяется по OrderNumber, если не задан вручную.
        /// </summary>
        public DeviceClassType ClassType
        {
            get => _classType ?? DeviceClassifier.DetectClassType(OrderNumber);
            set => _classType = value;
        }

        /// <summary>
        /// Генерирует список полных имён идентификаторов типов подсетей для Openness API.
        /// Например, для типа PROFINET вернёт "System:Subnet.Ethernet".
        /// </summary>
        public List<string> TypeSubnetIdentifier
        {
            get
            {
                if (SubnetTypes == null || SubnetTypes.Count == 0)
                    throw new InvalidOperationException("Отсутствуют типы подсетей для генерации идентификаторов.");

                return SubnetTypes.Select(type =>
                {
                    switch (type)
                    {
                        case "PROFINET":
                            return "System:Subnet.Ethernet";
                        case "PROFIBUS":
                            return "System:Subnet.Profibus";
                        case "MPI":
                            return "System:Subnet.Mpi";
                        case "ASI":
                            return "System:Subnet.Asi";
                        default:
                            throw new InvalidOperationException($"Неизвестный тип подсети: {type}");
                    }
                }).ToList();
            }
        }

        /// <summary>
        /// Произвольное текстовое описание устройства.
        /// Может отображаться в UI, логах, Excel.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Логическая группа устройства (например, "Шкаф 1").
        /// Не связана с TIA группами.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Включено ли устройство для генерации.
        /// Если false — строка будет проигнорирована.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        #endregion // additional data
    }
}

