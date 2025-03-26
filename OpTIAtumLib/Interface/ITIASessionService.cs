namespace OpTIAtumLib.Interface
{
    /// <summary>
    /// Интерфейс для сервиса управления экземпляром TIA Portal и whitelist.
    /// </summary>
    public interface ITIASessionService
    {
        /// <summary>
        /// Создаёт экземпляр TIA Portal V18 с графическим интерфейсом или без него.
        /// Также регистрирует приложение в whitelist, если необходимо.
        /// </summary>
        /// <param name="enableGuiTIA">
        /// true — запуск с пользовательским интерфейсом (GUI),
        /// false — фоновый режим без окна.
        /// </param>
        void CreateTIAInstance(bool enableGuiTIA);

        /// <summary>
        /// Проверяет, создан ли экземпляр TIA Portal через Openness.
        /// </summary>
        /// <returns>True — если экземпляр TIA уже создан, иначе false.</returns>
        bool IsTiaPortalRunning();

        /// <summary>
        /// Проверяет, запущен ли TIA Portal вручную (в обход Openness).
        /// </summary>
        /// <returns>True — если найден внешний процесс TIA Portal, иначе false.</returns>
        bool IsExternalTiaPortalRunning();

        /// <summary>
        /// Добавляет запись о текущем приложении в whitelist для Openness.
        /// </summary>
        /// <param name="ApplicationName">Имя исполняемого файла (без .exe).</param>
        /// <param name="ApplicationStartupPath">Полный путь к .exe-файлу приложения.</param>
        void SetWhiteList(string ApplicationName, string ApplicationStartupPath);
    }
}


