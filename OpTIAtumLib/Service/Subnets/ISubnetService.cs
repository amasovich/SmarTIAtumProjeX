using OpTIAtumLib.Model;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpTIAtumLib.Service.Subnets
{
    /// <summary>
    /// Интерфейс сервиса для создания и управления подсетями в TIA Portal.
    /// </summary>
    public interface ISubnetService
    {
        /// <summary>
        /// Создаёт подсеть в проекте TIA Portal на основе переданной модели.
        /// </summary>
        /// <param name="subnetModel">
        /// Модель подсети, содержащая имя и тип (PROFINET, PROFIBUS и т.д.).
        /// Поле <c>TypeIdentifier</c> автоматически формируется на основе типа.
        /// </param>
        /// <returns>
        /// Объект <see cref="Subnet"/>, представляющий созданную подсеть.
        /// </returns>
        Subnet CreateSubnet(SubnetModel subnetModel);

        //Задача: Создать подсеть и сразу подключить её к Node (сетевому интерфейсу устройства/модуля).
        //Subnet CreateAndConnectToNode(Node node, string subnetName);

        //Задача: Подключить DeviceItem, имеющий NetworkInterface, к уже существующей подсети.
        //void ConnectDeviceItemToSubnet(DeviceItem deviceItem, Subnet subnet);

        //Задача: Найти все Node, которые можно подключать к подсетям (используется для отладки и отображения).
        //List<Node> GetAllConnectableNodes(Project project);

        //Задача: Если подсеть уже существует — вернуть её.Если нет — создать.
        //Subnet EnsureOrCreateSubnet(Project project, string typeIdentifier, string subnetName);




    }
}
