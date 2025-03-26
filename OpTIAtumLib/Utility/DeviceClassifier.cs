using OpTIAtumLib.Model;

namespace OpTIAtumLib.Utility
{
    public static class DeviceClassifier
    {
        /// <summary>
        /// Определяет тип устройства на основе OrderNumber.
        /// </summary>
        /// <param name="orderNumber">Артикул устройства (OrderNumber)</param>
        /// <returns>Тип устройства (DeviceClassType)</returns>
        public static DeviceClassType DetectClassType(string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
                return DeviceClassType.Undefined;

            orderNumber = orderNumber.Trim().ToUpperInvariant();

            // PLC
            if (orderNumber.StartsWith("6ES7 21") || // S7-1200
                orderNumber.StartsWith("6ES7 51") || // S7-1500
                orderNumber.StartsWith("6ES7 61") ||
                orderNumber.StartsWith("6ES7 31") || // S7-300
                orderNumber.StartsWith("6ES7 41"))   // S7-400
                return DeviceClassType.CPU;

            // HMI панели
            if (orderNumber.StartsWith("6AV2") ||    // Comfort Panel
                orderNumber.StartsWith("6AV3") ||    // Mobile Panels
                orderNumber.StartsWith("6AG1"))      // HMI панель Simatic Panel PC
                return DeviceClassType.Hmi;

            // PC станции, IPC
            if (orderNumber.StartsWith("6AV7") ||    // WinCC RT Advanced
                orderNumber.StartsWith("6ES7 901"))  // IPC
                return DeviceClassType.PcSystem;

            // Частотники, приводы и т.д.
            if (orderNumber.StartsWith("6SL"))       // SINAMICS, приводы
                return DeviceClassType.Drive;

            // Сетевые устройства (CP, Scalance, и т.д.)
            if (orderNumber.StartsWith("6GK") ||     // Scalance
                orderNumber.StartsWith("6GF") ||     // IO-Link
                orderNumber.StartsWith("6ES7 95"))   // CP-модули
                return DeviceClassType.Network;

            // Обнаружение и мониторинг
            if (orderNumber.StartsWith("6GT") ||     // RFID
                orderNumber.StartsWith("3RF"))       // Реле/Мониторинг
                return DeviceClassType.DetectingMonitoring;

            // Модули ввода/вывода ET200 (IO)
            if (orderNumber.StartsWith("6ES7 15") || // ET200SP
                orderNumber.StartsWith("6ES7 14") || // ET200MP
                orderNumber.StartsWith("6ES7 13"))   // ET200S
                return DeviceClassType.IO;

            // Источники питания
            if (orderNumber.StartsWith("6EP"))       // SITOP, блоки питания
                return DeviceClassType.Power;

            /// Полевые устройства (например, датчики AS-i, Profibus)
            if (orderNumber.StartsWith("3RG") ||     // Датчики
                orderNumber.StartsWith("7MH"))       // Весы, измерительные устройства
                return DeviceClassType.Field;

            // Всё остальное
            return DeviceClassType.Custom;
        }
    }
}


