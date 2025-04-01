using System;

namespace OpTIAtumLib.Utility.Guard
{
    public static class Guard
    {
        /// <summary>
        /// Проверяет, инициализирован ли проект TIA.
        /// </summary>
        /// <param name="project">TIA Project</param>
        /// <exception cref="InvalidOperationException">Если проект не инициализирован.</exception>
        public static void ProjectInitialized(Siemens.Engineering.Project project)
        {
            if (project == null)
                throw new InvalidOperationException("TIA Project не инициализирован. Сначала вызовите Initialize().");
        }

        /// <summary>
        /// Проверяет, что объект не равен null.
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="paramName">Имя параметра</param>
        /// <param name="message">Сообщение об ошибке (опционально)</param>
        /// <exception cref="ArgumentNullException">Если объект равен null.</exception>
        public static void NotNull<T>(T obj, string paramName, string message = null)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName, message ?? $"Параметр '{paramName}' не может быть null.");
        }

        /// <summary>
        /// Проверяет, что строка не пуста и не состоит только из пробелов.
        /// </summary>
        /// <param name="value">Строковое значение</param>
        /// <param name="paramName">Имя параметра</param>
        /// <param name="message">Кастомное сообщение об ошибке (опционально)</param>
        /// <exception cref="ArgumentException">Если строка пуста или состоит из пробелов.</exception>
        public static void NotNullOrWhiteSpace(string value, string paramName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(message ?? $"Параметр '{paramName}' не может быть пустым или состоять только из пробелов.", paramName);
        }

        /// <summary>
        /// Проверяет логическое условие и выбрасывает InvalidOperationException, если оно ложно.
        /// </summary>
        /// <param name="condition">Условие, которое должно быть истинным.</param>
        /// <param name="message">Дополнительное сообщение исключения (опционально).</param>
        /// <exception cref="InvalidOperationException">Если условие не выполнено.</exception>
        public static void OperationValid(bool condition, string message = null)
        {
            if (!condition)
            {
                var fullMessage = "Нарушено логическое условие выполнения операции.";
                if (!string.IsNullOrWhiteSpace(message))
                    fullMessage += " " + message;

                throw new InvalidOperationException(fullMessage);
            }
        }

    }
}

