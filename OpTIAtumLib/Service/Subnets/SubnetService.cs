using OpTIAtumLib.Model;
using OpTIAtumLib.Utility.Device;
using OpTIAtumLib.Utility.Logger;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpTIAtumLib.Service.Subnets
{
    internal class SubnetService : ISubnetService
    {
        private readonly Project _project;

        public SubnetService(Project project)
        {
            _project = project ?? throw new ArgumentNullException(nameof(project));
        }

        /// <inheritdoc/>
        public Subnet CreateSubnet(SubnetModel subnetModel)
        {
            if (_project == null)
                throw new InvalidOperationException("TIA Project не инициализирован. Сначала вызовите Initialize().");

            if (subnetModel == null)
                throw new ArgumentNullException(nameof(subnetModel), "Модель устройства не может быть null.");

            string subnetName = subnetModel.SubnetName;
            string typeIdentifier = subnetModel.TypeIdentifier;

            if (string.IsNullOrWhiteSpace(subnetName))
                throw new ArgumentException("Имя подсети не может быть пустым.", nameof(subnetModel));

            if (string.IsNullOrWhiteSpace(typeIdentifier))
                throw new ArgumentException("Тип подсети не определён в модели.", nameof(subnetModel));

            try
            {
                Subnet newSubnet = _project.Subnets.Create(typeIdentifier, subnetName);

                Logger.Create($"Подсеть '{subnetName}' типа '{typeIdentifier}' успешно создана.");
                return newSubnet;
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при создании подсети '{subnetName}': {ex.Message}");
                throw;
            }
        }
    }
}
