using SmarTIAtumProjeX.Test;
using SmarTIAtumProjeX.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTIAtumProjeX
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ConsoleWindowTweaks.SetAlwaysOnTop();

            TestRunner.Run();
            //DeviceClassifierTest.Run();
            //TestDeviceBrowse.Run();
            //Console.ReadKey(); // Ожидание для просмотра результатов

        }
    }
}
