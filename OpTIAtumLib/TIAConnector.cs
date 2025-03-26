using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using OpTIAtumLib.Model;
using OpTIAtumLib.Interface;
using OpTIAtumLib.Utility;
using OpTIAtumLib.Tools;
using System.Diagnostics;

namespace OpTIAtumLib
{
    public class TIAConnector : ITIAConnector
    {
        public static TiaPortal instanceTIA;
        public static Project projectTIA;
        //private Device plcDevice;
        //public static DeviceModel deviceModel;

        /// <summary>
        /// Проверяет, открыт ли экземпляр TIA Portal через Openness.
        /// </summary>
        /// <returns>True — если экземпляр открыт и активен, иначе false.</returns>
        public bool IsTiaRunning()
        {
            return instanceTIA != null;
        }

        /// <summary>
        /// Проверяет, запущен ли экземпляр TIA Portal вне Openness (вручную).
        /// </summary>
        /// <returns>True — если TIA уже запущен как внешний процесс.</returns>
        public bool IsExternalTiaPortalRunning()
        {
            var tiaProcesses = Process.GetProcessesByName("Siemens.Automation.Portal");

            foreach (var p in tiaProcesses)
            {
                Logger.Debug($"Найден запущенный TIA Portal: PID = {p.Id}, Title = {p.MainWindowTitle}");
            }

            return tiaProcesses.Any();
        }

        /// <summary>
        /// Создаёт новый экземпляр TIA Portal V18 с пользовательским интерфейсом или без него.
        /// </summary>
        /// <param name="enableGuiTIA">
        /// Указывает, следует ли запускать TIA Portal с пользовательским интерфейсом.
        /// true — запуск с GUI (графическим интерфейсом), false — фоновый режим без отображения окна.
        /// </param>
        /// <remarks>
        /// При запуске происходит автоматическая регистрация текущего процесса в whitelist,
        /// что необходимо для работы Openness. Метод проверяет наличие уже запущенного экземпляра TIA Portal
        /// и при необходимости предлагает завершить его перед запуском нового.
        /// </remarks>
        /// <exception cref="UnauthorizedAccessException">
        /// Генерируется, если текущий процесс не имеет прав на запись в реестр при добавлении в whitelist.
        /// </exception>
        /// <example>
        /// <code>
        /// TIAConnector tia = new TIAConnector();
        /// tia.CreateTIAinstance(enableGuiTIA: true);
        /// </code>
        /// </example>
        public void CreateTIAinstance(bool enableGuiTIA)
        {
            var process = System.Diagnostics.Process.GetCurrentProcess();
            string appName = process.ProcessName;
            string appPath = process.MainModule.FileName;

            SetWhiteList(appName, appPath);

            if (IsExternalTiaPortalRunning())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[!] Обнаружен уже запущенный экземпляр TIA Portal.");
                Console.Write("Закрыть его и открыть новый экземпляр через Openness? [Y/N]: ");
                Console.ResetColor();

                var answer = Console.ReadKey().Key;
                Console.WriteLine();

                if (answer == ConsoleKey.Y)
                {
                    Logger.Info("Пользователь согласился закрыть существующий экземпляр TIA.");
                    // Убиваем все процессы TIA Portal
                    foreach (var proc in System.Diagnostics.Process.GetProcessesByName("Siemens.Automation.Portal"))
                    {
                        try
                        {
                            proc.Kill();
                            proc.WaitForExit();
                            Logger.Warn("Старый экземпляр TIA Portal завершён.");
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Ошибка при завершении процесса TIA Portal: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Logger.Info("Пользователь отказался от перезапуска TIA. Опеннес не будет запущен.");
                return;
                }
            }

            Logger.Info($"Запуск TIA Portal...");

            instanceTIA = enableGuiTIA
                ? new TiaPortal(TiaPortalMode.WithUserInterface)
                : new TiaPortal(TiaPortalMode.WithoutUserInterface);

            Logger.Info($"TIA Portal запущен в режиме: {(enableGuiTIA ? "С GUI" : "Фоновый")}");
        }

        /// <summary>
        /// Create new TIA project at given project path with given project name
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="projectName"></param>
        public void CreateTIAprject(string projectPath, string projectName)
        {
            // Create new directory info
            DirectoryInfo targetDirectory = new DirectoryInfo(projectPath);

            // Create new TIA project
              projectTIA = instanceTIA.Projects.Create(targetDirectory, projectName);
        }

        /// <summary>
        /// Open TIA project at given project path with given project name
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="projectName"></param>
        public void OpenTIAproject(string projectPath, string projectName)
        {
            // Create new file info
            FileInfo targetDirectory = new FileInfo(projectPath + "\\" + projectName + ".ap18");

            // Open exiting TIA project
            projectTIA = instanceTIA.Projects.Open(targetDirectory);
        }

        public List<DeviceModel> GetDevices()
        {
            return DeviceBrowser.GetDeviceModels(projectTIA);
        }

        /// <summary>
        /// Добавляет устройство в проект TIA Portal на основе переданной модели.
        /// Поддерживает как стандартные устройства (PLC, ET200, HMI и др.), так и GSD/GSDML.
        /// </summary>
        /// <param name="deviceModel">
        /// Модель устройства, содержащая необходимые данные:
        /// <list type="bullet">
        ///   <item><term>TypeIdentifier</term> — полный идентификатор устройства, например:
        ///     <c>OrderNumber:6ES7 518-4FP00-0AB0/V3.0</c> или <c>GSD:GSDML-...</c></item>
        ///   <item><term>DeviceName</term> — имя устройства, отображаемое в TIA проекте</item>
        ///   <item><term>Station</term> — имя станции или узла в сетевом представлении</item>
        /// </list>
        /// </param>
        /// <returns>
        /// Объект <see cref="Device"/> — добавленное устройство в проект TIA.
        /// Его можно использовать для последующего добавления модулей через <see cref="AddDeviceItemToDevice(Device, DeviceModel)"/>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Проект TIA не открыт или не создан перед добавлением устройства.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Параметр <paramref name="deviceModel"/> равен null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Отсутствуют обязательные значения: <c>TypeIdentifier</c> или <c>DeviceName</c>.
        /// </exception>

        public Device AddDeviceToProject(DeviceModel deviceModel)
        {
            if (projectTIA == null)
                throw new InvalidOperationException("TIA Project не открыт. Сначала открой или создай проект.");

            if (deviceModel == null)
                throw new ArgumentNullException(nameof(deviceModel), "Модель устройства не может быть null.");

            string typeIdentifier = deviceModel.TypeIdentifier;
            string deviceName = deviceModel.DeviceName;
            string stationName = deviceModel.Station;

            if (string.IsNullOrWhiteSpace(typeIdentifier))
                throw new ArgumentException("TypeIdentifier обязателен для создания устройства.");

            if (string.IsNullOrWhiteSpace(deviceName))
                throw new ArgumentException("DeviceName обязателен для создания устройства.");

            try
            {
                Device createdDevice = projectTIA.Devices.CreateWithItem(typeIdentifier, deviceName, stationName);
                Logger.Info($"Устройство '{deviceName}' успешно добавлено: {typeIdentifier} → Станция: {stationName}");
                return createdDevice;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении устройства '{deviceName}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Добавляет модуль (DeviceItem) в заданное устройство (Device) на указанную позицию.
        /// Используется для вставки модулей GSD/GSDML-устройств, например: входы/выходы, подмодули.
        /// </summary>
        /// <param name="device">
        /// Объект устройства, в которое будет вставлен модуль.
        /// Должен быть предварительно добавлен в проект с помощью <see cref="AddDeviceToProject(DeviceModel)"/>.
        /// </param>
        /// <param name="moduleModel">
        /// Модель модуля, содержащая TypeIdentifier (например, GSD:.../M/...),
        /// имя устройства и номер позиции (слота), в которую следует вставить модуль.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Выбрасывается, если <paramref name="device"/> равен null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Выбрасывается, если не найден подходящий слот для вставки модуля,
        /// или если вставка в указанную позицию невозможна.
        /// </exception>
        public void AddDeviceItemToDevice(Device device, DeviceModel moduleModel)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            string typeIdentifier = moduleModel.TypeIdentifier;
            string name = moduleModel.DeviceName;
            int position = moduleModel.PositionNumber;

            var parent = device.DeviceItems
                .FirstOrDefault(di => di.GetPlugLocations().Any(pl => pl.PositionNumber == position));

            if (parent == null)
                throw new InvalidOperationException("Подходящий слот для модуля не найден.");

            if (!parent.CanPlugNew(typeIdentifier, name, position))
                throw new InvalidOperationException($"Невозможно вставить модуль '{name}' на позицию {position}.");

            try
            {
                DeviceItem module = parent.PlugNew(typeIdentifier, name, position);
                Logger.Info($"Модуль '{name}' добавлен в устройство '{device.Name}' на позицию {position}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при добавлении модуля '{name}': {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Set whitelist entry for TIA Portal in registry
        /// </summary>
        /// <param name="ApplicationName"></param>
        /// <param name="ApplicationStartupPath"></param>
        public void SetWhiteList(string ApplicationName, string ApplicationStartupPath)
        {
            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey software = null;
            try
            {
                software = key.OpenSubKey(@"SOFTWARE\Siemens\Automation\Openness")
                    .OpenSubKey("18.0")
                    .OpenSubKey("Whitelist")
                    .OpenSubKey(ApplicationName + ".exe")
                    .OpenSubKey("Entry", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
            }
            catch (Exception)
            {

                //Eintrag in der Whitelist ist nicht vorhanden
                //Entry in whitelist is not available
                software = key.CreateSubKey(@"SOFTWARE\Siemens\Automation\Openness")
                    .CreateSubKey("18.0")
                    .CreateSubKey("Whitelist")
                    .CreateSubKey(ApplicationName + ".exe")
                    .CreateSubKey("Entry", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
            }

            string lastWriteTimeUtcFormatted = string.Empty;
            DateTime lastWriteTimeUtc;
            HashAlgorithm hashAlgorithm = SHA256.Create();
            FileStream stream = File.OpenRead(ApplicationStartupPath);
            byte[] hash = hashAlgorithm.ComputeHash(stream);
            // this is how the hash should appear in the .reg file
            string convertedHash = Convert.ToBase64String(hash);
            software.SetValue("FileHash", convertedHash);
            lastWriteTimeUtc = new FileInfo(ApplicationStartupPath).LastWriteTimeUtc;
            // this is how the last write time should be formatted
            lastWriteTimeUtcFormatted = lastWriteTimeUtc.ToString(@"yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            software.SetValue("DateModified", lastWriteTimeUtcFormatted);
            software.SetValue("Path", ApplicationStartupPath);
        }
    }

    public class SubnetCreator
    {
        /// <summary>
        /// Создает подсеть, подключённую к узлу.
        /// Тип подсети определяется типом интерфейса, к которому подключается узел.
        /// </summary>
        /// <param name="node">Объект узла (Node), например, соответствующий физическому интерфейсу устройства</param>
        /// <param name="subnetName">Имя создаваемой подсети</param>
        /// <returns>Созданный объект подсети</returns>
        public Subnet CreateAndConnectSubnet(Node node, string subnetName)
        {
            // Создание и подключение подсети к узлу
            Subnet subnet = node.CreateAndConnectToSubnet(subnetName);
            Console.WriteLine($"Подсеть '{subnetName}' успешно создана и подключена к узлу '{node.Name}'.");
            return subnet;
        }

        /// <summary>
        /// Создает подсеть в составе проекта без привязки к узлу.
        /// Используется идентификатор типа, например "System:Subnet.Ethernet".
        /// </summary>
        /// <param name="project">Открытый проект TIA Portal</param>
        /// <param name="subnetTypeIdentifier">Идентификатор типа подсети (например, "System:Subnet.Ethernet")</param>
        /// <param name="subnetName">Имя новой подсети</param>
        /// <returns>Созданный объект подсети</returns>
        public Subnet CreateStandaloneSubnet(Project project, string subnetTypeIdentifier, string subnetName)
        {
            // Получаем коллекцию подсетей из проекта
            SubnetComposition subnets = project.Subnets;
            // Создаем новую подсеть с заданным типом
            Subnet newSubnet = subnets.Create(subnetTypeIdentifier, subnetName);
            Console.WriteLine($"Подсеть '{subnetName}' типа '{subnetTypeIdentifier}' успешно создана в проекте.");
            return newSubnet;

            //newSubnet.SetAttribute("", Device);
        }
    }
}

