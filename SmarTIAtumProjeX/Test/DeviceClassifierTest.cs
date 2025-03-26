using System;
using System.Collections.Generic;
using OpTIAtumLib.Model;
using OpTIAtumLib.Utility;

namespace SmarTIAtumProjeX.Test
{
    public static class DeviceClassifierTest
    {
        public static void Run()
        {
            Console.WriteLine("[TEST] Проверка определения ClassType по OrderNumber\n");

            var testCases = new Dictionary<string, DeviceClassType>
            {
                { "6ES7 518-4FP00-0AB0", DeviceClassType.CPU },
                { "6ES7 155-6AU01-0BN0", DeviceClassType.IO },
                { "6AV2123-4DB03-0AX0", DeviceClassType.Hmi },
                { "6AV7861-0AA00-1AA0", DeviceClassType.PcSystem },
                { "6GK7443-1EX30-0XE0", DeviceClassType.Network },
                { "6SL3210-1PE23-8UL0", DeviceClassType.Drive },
                { "6GT2002-0ED00", DeviceClassType.DetectingMonitoring },
                { "6EP1437-3BA00", DeviceClassType.Power },
                { "3RG4013-0AB00", DeviceClassType.Field },
                { "XYZ123456789", DeviceClassType.Custom }
            };

            foreach (var test in testCases)
            {
                var detected = DeviceClassifier.DetectClassType(test.Key);
                bool result = detected == test.Value;

                string status = result ? "[OK]" : "[FAIL]";
                Console.WriteLine($"{status} '{test.Key}' → Ожидается: {test.Value}, Обнаружено: {detected}");
            }

            Console.WriteLine("\n[TEST] Завершено.\n");
        }
    }
}

