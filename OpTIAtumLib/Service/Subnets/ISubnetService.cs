using Siemens.Engineering;
using Siemens.Engineering.HW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpTIAtumLib.Service.Subnets
{
    internal interface ISubnetService
    {
        /// <summary>
        /// Создает подсеть, подключённую к узлу.
        /// Тип подсети определяется типом интерфейса, к которому подключается узел.
        /// </summary>
        /// <param name="node">Объект узла (Node), например, соответствующий физическому интерфейсу устройства</param>
        /// <param name="subnetName">Имя создаваемой подсети</param>
        /// <returns>Созданный объект подсети</returns>
        Subnet CreateAndConnectSubnet(Node node, string subnetName);

        /// <summary>
        /// Создает подсеть в составе проекта без привязки к узлу.
        /// Используется идентификатор типа, например "System:Subnet.Ethernet".
        /// </summary>
        /// <param name="project">Открытый проект TIA Portal</param>
        /// <param name="subnetTypeIdentifier">Идентификатор типа подсети (например, "System:Subnet.Ethernet")</param>
        /// <param name="subnetName">Имя новой подсети</param>
        /// <returns>Созданный объект подсети</returns>
        Subnet CreateStandaloneSubnet(Project project, string subnetTypeIdentifier, string subnetName);

        //Задача: Создать изолированную подсеть (например, PROFINET, PROFIBUS).
        Subnet CreateStandaloneSubnet(string typeIdentifier, string name);

        //Задача: Создать подсеть и сразу подключить её к Node (сетевому интерфейсу устройства/модуля).
        Subnet CreateAndConnectToNode(Node node, string subnetName);

        //Задача: Подключить DeviceItem, имеющий NetworkInterface, к уже существующей подсети.
        void ConnectDeviceItemToSubnet(DeviceItem deviceItem, Subnet subnet);

        //Задача: Найти все Node, которые можно подключать к подсетям (используется для отладки и отображения).
        List<Node> GetAllConnectableNodes(Project project);

        //Задача: Если подсеть уже существует — вернуть её.Если нет — создать.
        Subnet EnsureOrCreateSubnet(Project project, string typeIdentifier, string subnetName);



    }
}
