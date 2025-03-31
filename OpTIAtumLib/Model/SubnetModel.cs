using System;

namespace OpTIAtumLib.Model
{
    /// <summary>
    /// Модель описания подсети для конфигурации в TIA Portal.
    /// Используется для генерации и подключения подсетей к устройствам.
    /// </summary>
    public class SubnetModel
    {
        #region properties

        /// <summary>
        /// Имя подсети (логическое имя, отображаемое в TIA Portal)
        /// </summary>
        public string SubnetName { get; set; }

        /// <summary>
        /// Упрощённое имя типа подсети (например, "PROFINET", "PROFIBUS", "IE", "Ethernet")
        /// </summary>
        public string SubnetType { get; set; }

        /// <summary>
        /// Генерирует полное имя идентификатора типа подсети для Openness API.
        /// Пример: PROFINET это "System:Subnet.Ethernet"
        /// </summary>
        public string TypeIdentifier
        {
            get
            {
                switch (SubnetType)
                {
                    case "PROFINET":
                        return "System:Subnet.Ethernet";
                    case "PROFIBUS":
                        return "System:Subnet.Profibus";
                    case "MPI":
                        return "System:Subnet.Mpi";
                    case "ASI":
                        return "System:Subnet.Asi";
                    default:
                        throw new InvalidOperationException($"Неизвестный тип подсети: {SubnetType}");
                }
            }
        }


        #endregion // properties
    }
}

