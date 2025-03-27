using System;
using Siemens.Engineering;
using OpTIAtumLib.Service.TIASession;
using OpTIAtumLib.Service.Projects;
using OpTIAtumLib.Service.Devices;

namespace OpTIAtumLib.Core
{
    /// <summary>
    /// Фасад для работы с TIA Portal. Делегирует задачи соответствующим сервисам.
    /// </summary>
    public class TIAConnector : ITIAConnector
    {
        /// <inheritdoc/>
        public TiaPortal Instance { get; private set; }

        /// <inheritdoc/>
        public Project Project { get; private set; }

        /// <inheritdoc/>
        public ITIASessionService SessionService { get; private set; }

        /// <inheritdoc/>
        public IProjectService ProjectService { get; private set; }

        /// <inheritdoc/>
        public IDeviceService DeviceService { get; private set; }

        /// <inheritdoc/>
        public TIAConnector()
        {
            SessionService = new TIASessionService();
        }

        /// <inheritdoc/>
        public void Initialize(TiaPortal instance, Project project)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Project = project ?? throw new ArgumentNullException(nameof(project));

            ProjectService = new ProjectService(Instance);
            DeviceService = new DeviceService(Project);
        }
    }
}

