using Siemens.Engineering;
using System;
using System.IO;
using OpTIAtumLib.Utility.Logger;
using OpTIAtumLib.Utility.Guards;

namespace OpTIAtumLib.Service.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly TiaPortal _tiaPortal;
        private const string ProjectExtension = ".ap18"; // Расширение файла проекта

        public ProjectService(TiaPortal tiaPortal)
        {
            _tiaPortal = tiaPortal ?? throw new ArgumentNullException(nameof(tiaPortal));
        }

        /// <inheritdoc/>
        public Project CreateProject(string projectPath, string projectName)
        {
            Guard.NotNullOrWhiteSpace(projectPath, nameof(projectPath));
            Guard.NotNullOrWhiteSpace(projectName, nameof(projectName));

            Logger.Debug($"Создание проекта '{projectName}' по пути '{projectPath}'...");

            var targetDirectory = new DirectoryInfo(projectPath);

            if (!targetDirectory.Exists)
                targetDirectory.Create();

            try
            {
                var project = _tiaPortal.Projects.Create(targetDirectory, projectName);
                Logger.Create($"Проект '{projectName}' успешно создан по пути '{projectPath}'.");
                return project;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при создании проекта '{projectName}': {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public Project OpenProject(string projectPath, string projectName)
        {
            Guard.NotNullOrWhiteSpace(projectPath, nameof(projectPath));
            Guard.NotNullOrWhiteSpace(projectName, nameof(projectName));

            var filePath = Path.Combine(projectPath, projectName + ProjectExtension);

            Logger.Debug($"Попытка открыть проект '{projectName}' из '{filePath}'...");

            if (!File.Exists(filePath))
            {
                Logger.Error($"Проект не найден по пути: '{filePath}'");
                throw new FileNotFoundException("Проект не найден по указанному пути.", filePath);
            }

            try
            {
                var project = _tiaPortal.Projects.Open(new FileInfo(filePath));
                Logger.Info($"Проект '{projectName}' успешно открыт.");
                return project;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при открытии проекта '{projectName}': {ex.Message}");
                throw;
            }
        }
    }
}
