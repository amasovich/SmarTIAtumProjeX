using OpTIAtumLib;
using OpTIAtumLib.Model;
using OpTIAtumLib.Tools;
using System;

namespace SmarTIAtumProjeX.Test
{
    internal class TestDeviceBrowse
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("[START] Запуск TIA Portal...");
                TIAConnector tia = new TIAConnector();

                tia.CreateTIAinstance(true);
                tia.OpenTIAproject(@"C:\Users\v.bereznyak\Desktop\TIA_Projects\TestProject", "TestProject");

                var devices = tia.GetDevices();
                foreach (var dev in devices)
                {
                    Console.WriteLine($"[OK] {dev.DeviceName} → {dev.OrderNumber}, {dev.FirmwareVersion}, Class: {dev.ClassType}");
                }


            }

            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] Ошибка: " + ex.Message);
            }
            Console.WriteLine("[FINISH] Все устройства выгружены. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
