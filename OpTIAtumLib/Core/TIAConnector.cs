// Copyright 2025 © «COMITAS» / © Vladimir Bereznyak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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

