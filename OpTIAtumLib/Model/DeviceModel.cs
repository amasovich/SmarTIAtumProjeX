using OpTIAtumLib.Utility;
using Siemens.Engineering.HW.Features;

namespace OpTIAtumLib.Model
{
    public class DeviceModel
    {
        #region properties

        public string Station { get; set; }
        public string TemplateName { get; set; }
        public string DeviceName { get; set; }
        public string OrderNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public string GsdName { get; set; }
        public string GsdType { get; set; }
        public string GsdId { get; set; }
        public bool IncludeFailsafe { get; set; }
        public int PositionNumber { get; set; }


        //public string TypeIdentifier => $"OrderNumber:{OrderNumber}/{FirmwareVersion}";

        public string TypeIdentifier =>
        !string.IsNullOrWhiteSpace(GsdName) && !string.IsNullOrWhiteSpace(GsdType)
        ? $"GSD:{GsdName}/{GsdType}" + (string.IsNullOrWhiteSpace(GsdId) ? "" : $"/{GsdId}")
        : $"OrderNumber:{OrderNumber}" + (string.IsNullOrWhiteSpace(FirmwareVersion) ? "" : $"/{FirmwareVersion}");

        public string TypeName => $"{Station}_{PositionNumber + 1}";
        public string Name => DeviceName;

        private DeviceClassType? _classType;
        public DeviceClassType ClassType
        {
            get => _classType ?? DeviceClassifier.DetectClassType(OrderNumber);
            set => _classType = value;
        }

        #endregion // properties
    }
}

