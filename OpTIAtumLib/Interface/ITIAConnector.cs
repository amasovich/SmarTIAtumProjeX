using OpTIAtumLib.Model;
using Siemens.Engineering.HW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpTIAtumLib.Interface
{
    internal interface ITIAConnector
    {
        void CreateTIAinstance(bool enableGuiTIA);
        void CreateTIAprject(string projectPath, string projectName);
        void OpenTIAproject(string projectPath, string projectName);
        Device AddDeviceToProject(DeviceModel deviceModel);
        void AddDeviceItemToDevice(Device device, DeviceModel moduleModel);
    }
}
