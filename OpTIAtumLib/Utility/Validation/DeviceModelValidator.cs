using System;
using OpTIAtumLib.Model;
using OpTIAtumLib.Utility.Guards;

namespace OpTIAtumLib.Utility.Validation
{
    /// <summary>
    /// Валидатор модели устройства <see cref="DeviceModel"/>.
    /// Проверяет корректность обязательных полей и соответствие бизнес-правилам.
    /// </summary>
    public static class DeviceModelValidator
    {
        /// <summary>
        /// Выполняет проверку модели устройства.
        /// Генерирует исключения, если модель содержит некорректные данные.
        /// </summary>
        /// <param name="deviceModel">Проверяемая модель устройства</param>
        /// <exception cref="ArgumentNullException">Если модель или её поля не заданы</exception>
        /// <exception cref="ArgumentException">Если строковые параметры некорректны</exception>
        /// <exception cref="InvalidOperationException">Если нарушены логические правила</exception>
        public static void Validate(DeviceModel deviceModel)
        {
            // Проверка, что DeviceModel задан
            Guard.NotNull(deviceModel, nameof(deviceModel));

            // Обязательные поля
            Guard.NotNullOrWhiteSpace(deviceModel.DeviceName, nameof(deviceModel.DeviceName));
            Guard.NotNullOrWhiteSpace(deviceModel.TypeIdentifier, nameof(deviceModel.TypeIdentifier));

            // GSD-устройства — проверка GSD-полей
            if (deviceModel.TypeIdentifier.StartsWith("GSD:", StringComparison.OrdinalIgnoreCase))
            {
                Guard.NotNullOrWhiteSpace(deviceModel.GsdName, nameof(deviceModel.GsdName));
                Guard.NotNullOrWhiteSpace(deviceModel.GsdType, nameof(deviceModel.GsdType));
                Guard.NotNullOrWhiteSpace(deviceModel.GsdId, nameof(deviceModel.GsdId));
            }

            // Если это модуль — проверка наличия родителя
            if (!string.IsNullOrWhiteSpace(deviceModel.ParentDeviceName))
            {
                Guard.OperationValid(deviceModel.PositionNumber >= 0, "PositionNumber должен быть ≥ 0 для модуля.");
            }
            else
            {
                // Это root-устройство (не модуль)
                Guard.OperationValid(deviceModel.PositionNumber == 0, "Root-устройство должно иметь PositionNumber == 0.");
            }
        }
    }
}
