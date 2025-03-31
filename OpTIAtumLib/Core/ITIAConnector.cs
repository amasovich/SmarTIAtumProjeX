using Siemens.Engineering;
using OpTIAtumLib.Service.TIASession;
using OpTIAtumLib.Service.Projects;
using OpTIAtumLib.Service.Devices;
using OpTIAtumLib.Service.Subnets;

namespace OpTIAtumLib.Core
{
    /// <summary>
    /// Интерфейс фасада для доступа к сервисам TIA Portal и управления проектом.
    /// </summary>
    public interface ITIAConnector
    {
        /// <summary>
        /// Экземпляр TIA Portal, используемый для работы с API Openness.
        /// </summary>
        TiaPortal Instance { get; }

        /// <summary>
        /// Активный проект TIA Portal (открытый или созданный).
        /// </summary>
        Project Project { get; }

        /// <summary>
        /// Сервис управления сессией TIA Portal и регистрацией в whitelist.
        /// </summary>
        ITIASessionService SessionService { get; }

        /// <summary>
        /// Сервис для создания и открытия проектов TIA Portal.
        /// </summary>
        IProjectService ProjectService { get; }

        /// <summary>
        /// Сервис для добавления устройств и модулей в проект.
        /// </summary>
        IDeviceService DeviceService { get; }

        ISubnetService SubnetService { get; }

        /// <summary>
        /// Метод инициализации фасада после запуска TIA и открытия/создания проекта.
        /// </summary>
        /// <param name="instance">Экземпляр TIA Portal</param>
        /// <param name="project">Проект TIA Portal</param>
        void Initialize(TiaPortal instance, Project project);
    }
}

