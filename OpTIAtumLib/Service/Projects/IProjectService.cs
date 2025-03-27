using Siemens.Engineering;

namespace OpTIAtumLib.Service.Projects
{
    /// <summary>
    /// Интерфейс для сервиса управления проектами TIA Portal.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Создаёт новый проект TIA Portal по заданному пути и имени.
        /// </summary>
        /// <param name="projectPath">Путь к директории проекта</param>
        /// <param name="projectName">Имя проекта</param>
        /// <returns>Созданный объект Project (Siemens.Engineering.Project)</returns>

        Project CreateProject(string projectPath, string projectName);

        /// <summary>
        /// Открывает существующий проект TIA Portal.
        /// </summary>
        /// <param name="projectPath">Путь к директории</param>
        /// <param name="projectName">Имя проекта</param>
        /// <returns>Объект открытого проекта</returns>
        Project OpenProject(string projectPath, string projectName);
    }
}

