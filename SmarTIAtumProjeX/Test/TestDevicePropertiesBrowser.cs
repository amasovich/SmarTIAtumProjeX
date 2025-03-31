using OpTIAtumLib.Core;
using OpTIAtumLib.Service.Projects;
using OpTIAtumLib.Service.TIASession;
using OpTIAtumLib.Utility.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTIAtumProjeX.Test
{
    internal class TestDevicePropertiesBrowser
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("[START] Запуск TIA Portal...");

                // Создаём фасадный объект для управления TIA Portal и сервисами Openness
                var tia = new TIAConnector();

                // Запуск TIA Portal с пользовательским интерфейсом
                tia.SessionService.CreateTIAInstance(enableGuiTIA: true);

                // Получение экземпляра TIA через реализацию (без изменения интерфейса)
                var instance = ((TIASessionService)tia.SessionService).Instance;

                //Открытие проекта
                string projectName = "TestProject";
                string projectPath = @"C:\Users\v.bereznyak\Desktop\TIA_Projects\TestProject";
                //string projectName = "MK1517_Stage2OldCPU_LS_1";
                //string projectPath = @"C:\Users\v.bereznyak\Documents\Automation\Sessions\MK1517_Stage2OldCPU_LS_1";

                var project = new ProjectService(instance).OpenProject(projectPath, projectName);
                Console.WriteLine($"[INFO] Проект открыт: {projectName}");

                // Инициализация TIAConnector с открытым проектом
                tia.Initialize(instance, project);

                // Получение списка всех свойств устройств и модулей DeviceItem
                DevicePropertiesBrowser.DumpDeviceAndItemProperties(tia.Project);
                DevicePropertiesBrowser.DumpUngroupDeviceAndItemProperties(tia.Project);

            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Ошибка: " + ex.Message);
            }

            Console.WriteLine("[FINISH] Все свойства устройств выгружены. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
