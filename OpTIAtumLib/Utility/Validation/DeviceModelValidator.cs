using System;
using OpTIAtumLib.Model;
using static OpTIAtumLib.Utility.Guard.Guard;

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
            NotNull(deviceModel, nameof(deviceModel));
            NotNullOrWhiteSpace(deviceModel.DeviceName, nameof(deviceModel.DeviceName));
            NotNullOrWhiteSpace(deviceModel.TypeIdentifier, nameof(deviceModel.TypeIdentifier));
            NotNullOrWhiteSpace(deviceModel.Station, nameof(deviceModel.Station));

            // Проверка для GSD-устройств
            if (deviceModel.TypeIdentifier.StartsWith("GSD:", StringComparison.OrdinalIgnoreCase))
            {
                NotNullOrWhiteSpace(deviceModel.GsdName, nameof(deviceModel.GsdName));
                NotNullOrWhiteSpace(deviceModel.GsdType, nameof(deviceModel.GsdType));
                NotNullOrWhiteSpace(deviceModel.GsdId, nameof(deviceModel.GsdId));
            }

            // Проверка номера позиции
            OperationValid(deviceModel.PositionNumber >= 0, "PositionNumber должен быть ≥ 0");
        }
    }
}
