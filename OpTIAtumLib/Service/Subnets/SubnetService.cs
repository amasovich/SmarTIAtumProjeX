using Siemens.Engineering;
using Siemens.Engineering.HW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpTIAtumLib.Service.Subnets
{
    internal class SubnetService //: ISubnetService
    {
        /// <inheritdoc/>
        public Subnet CreateAndConnectSubnet(Node node, string subnetName)
        {
            // Создание и подключение подсети к узлу
            Subnet subnet = node.CreateAndConnectToSubnet(subnetName);
            Console.WriteLine($"Подсеть '{subnetName}' успешно создана и подключена к узлу '{node.Name}'.");
            return subnet;
        }

        public Subnet CreateStandaloneSubnet(Project project, string subnetTypeIdentifier, string subnetName)
        {
            // Получаем коллекцию подсетей из проекта
            SubnetComposition subnets = project.Subnets;
            // Создаем новую подсеть с заданным типом
            Subnet newSubnet = subnets.Create(subnetTypeIdentifier, subnetName);
            Console.WriteLine($"Подсеть '{subnetName}' типа '{subnetTypeIdentifier}' успешно создана в проекте.");
            return newSubnet;
        }
    }
}
