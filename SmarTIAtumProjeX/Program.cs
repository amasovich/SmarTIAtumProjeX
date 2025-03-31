// Copyright 2025 © «COMITAS» / © Vladimir Bereznyak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Siemens.Engineering;
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

            TestCreateProfinetSubnet.Run();

            //DeviceClassifierTest.Run();
            //TestDeviceBrowse.Run();
            //TestDevicePropertiesBrowser.Run();

            //Console.ReadKey(); // Ожидание для просмотра результатов

        }
    }
}
