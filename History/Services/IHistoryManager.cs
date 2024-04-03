using History.Entities;
using Infrastructure.History.DTOs;
using Infrastructure.History.Entities;
using System.Collections.Generic;

namespace Infrastructure.History.Services
{
    public interface IHistoryManager
    {
        /// <summary>
        ///     Add a points record to the datbase.
        /// </summary>
        void AddCheckpoint(Point point);

        /// <summary>
        ///     Add a points record to the datbase.
        /// </summary>
        void AddCheckpoint(Container.Entities.Container container, string what, string where, int userId);

        /// <summary>
        ///     Add points records to the database.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="what"></param>
        /// <param name="where"></param>
        /// <param name="userId"></param>
        void AddCheckpoints(IEnumerable<Container.Entities.Container> containers, string what, string where, int userId);

        /// <summary>
        ///     Get containers full history.
        /// </summary>
        /// <param name="containerUid">the container uid</param>
        /// <returns>List of history response data</returns>
        HistoryResponse GetContainerHistory(string containerUid);

        /// <summary>
        ///     Mark containers as missing
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="siblings"></param>
        void MarkMissingContainers(List<Container.Entities.Container> containers, int userId, string where, List<Container.Entities.Container> siblings);

        /// <summary>
        ///    Set status of containers.
        /// </summary>
        /// <param name="containerStatus"></param>
        void SetContainerStatus(IEnumerable<ContainerStatus> containerStatuses, string userLocation, int userId);
    }
}
