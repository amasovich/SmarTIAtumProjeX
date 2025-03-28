using System;
using SmarTIAtumProjeX.Test;
using SmarTIAtumProjeX.Utility;

namespace SmarTIAtumProjeX
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ConsoleWindowTweaks.SetAlwaysOnTop();

            //TestRunner.Run();
            //TestRunAddScanDevice.Run();

            //DeviceClassifierTest.Run();
            TestDeviceBrowse.Run();

            //Console.ReadKey(); // Ожидание для просмотра результатов

        }
    }
}
