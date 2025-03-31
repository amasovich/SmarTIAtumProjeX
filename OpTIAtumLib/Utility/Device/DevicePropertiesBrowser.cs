using Siemens.Engineering;
using Siemens.Engineering.HW;
using System;
using System.Reflection;

namespace OpTIAtumLib.Utility.Device
{
    /// <summary>
    /// Утилита для анализа и вывода всех свойств устройств и модулей DeviceItem.
    /// Полезно для изучения структуры проекта и отладки.
    /// </summary>
    public static class DevicePropertiesBrowser
    {
        /// <summary>
        /// Выводит в консоль все доступные свойства устройств и модулей.
        /// </summary>
        /// <param name="project">Открытый проект TIA Portal.</param>
        public static void DumpDeviceAndItemProperties(Project project)
        {
            foreach (var device in project.Devices)
            {
                Console.WriteLine("=== DEVICE ===");
                DumpProperties(device);

                foreach (var item in device.DeviceItems)
                {
                    Console.WriteLine("  --- DEVICE ITEM (1 уровень) ---");
                    DumpProperties(item, "  ");

                    DumpDeviceItemsRecursive(item, "    ");
                }

                Console.WriteLine(); // Пустая строка между устройствами
            }
        }

        public static void DumpUngroupDeviceAndItemProperties(Project project)
        {
            foreach (var device in project.UngroupedDevicesGroup.Devices)
            {
                Console.WriteLine("=== DEVICE ===");
                DumpProperties(device);

                foreach (var item in device.DeviceItems)
                {
                    Console.WriteLine("  --- DEVICE ITEM (1 уровень) ---");
                    DumpProperties(item, "  ");

                    DumpDeviceItemsRecursive(item, "    ");
                }

                Console.WriteLine(); // Пустая строка между устройствами
            }
        }

        /// <summary>
        /// Рекурсивно обходит все вложенные DeviceItem и выводит их свойства.
        /// </summary>
        private static void DumpDeviceItemsRecursive(DeviceItem item, string indent)
        {
            foreach (var child in item.DeviceItems)
            {
                Console.WriteLine($"{indent}--- DEVICE ITEM (вложенный) ---");
                DumpProperties(child, indent);
                DumpDeviceItemsRecursive(child, indent + "  ");
            }
        }

        /// <summary>
        /// Универсальный метод вывода всех публичных свойств объекта.
        /// </summary>
        private static void DumpProperties(object obj, string indent = "")
        {
            var type = obj.GetType();
            Console.WriteLine($"{indent}Тип: {type.Name}");

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    var value = prop.GetValue(obj);
                    Console.WriteLine($"{indent}{prop.Name} = {value}");
                }
                catch
                {
                    Console.WriteLine($"{indent}{prop.Name} = [Ошибка чтения]");
                }
            }
        }
    }
}
