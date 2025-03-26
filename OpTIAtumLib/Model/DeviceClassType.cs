namespace OpTIAtumLib.Model
{
    /// <summary>
    /// Класс устройства, используемый для логики и UI.
    /// </summary>
    public enum DeviceClassType
    {
        /// <summary>Тип не определён</summary>
        Undefined = 0,

        /// <summary>Контроллер (PLC)</summary>
        Plc,

        /// <summary>Панель оператора или HMI</summary>
        Hmi,

        /// <summary>Промышленная ПК-система или SCADA</summary>
        PcSystem,

        /// <summary>Приводы, сервомоторы, частотники</summary>
        Drive,

        /// <summary>Сетевые устройства (CP, коммутаторы)</summary>
        Network,

        /// <summary>Сенсоры, энкодеры, детекторы</summary>
        DetectingMonitoring,

        /// <summary>Модули ввода-вывода</summary>
        IO,

        /// <summary>Блоки питания</summary>
        Power,

        /// <summary>Полевые устройства (PROFIBUS/AS-i)</summary>
        Field,

        /// <summary>Произвольный/нестандартный тип</summary>
        Custom
    }
}
