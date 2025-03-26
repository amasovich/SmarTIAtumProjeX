using Siemens.Engineering;
using System;
using System.IO;
using OpTIAtumLib.Utility;
using OpTIAtumLib.Interface;

namespace OpTIAtumLib.Services
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
            var targetDirectory = new DirectoryInfo(projectPath);

            if (!targetDirectory.Exists)
                targetDirectory.Create();

            var project = _tiaPortal.Projects.Create(targetDirectory, projectName);
            Logger.Info($"Проект '{projectName}' успешно создан по пути '{projectPath}'.");
            return project;
        }

        /// <inheritdoc/>
        public Project OpenProject(string projectPath, string projectName)
        {
            var filePath = Path.Combine(projectPath, projectName + ProjectExtension);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Проект не найден по указанному пути.", filePath);

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
