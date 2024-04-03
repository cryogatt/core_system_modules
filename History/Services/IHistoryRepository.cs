using System.Collections.Generic;
using Common;
using History.Entities;
using Infrastructure.History.Entities;

namespace Infrastructure.History.Services
{
    public interface IHistoryRepository : IRepository
    {
        /// <summary>
        ///     Has the sample been stored?
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        bool IsInceptDateSet(int containerId);

        /// <summary>
        ///     Get containers full history.
        /// </summary>
        /// <param name="string containerUid"></param>
        /// <returns></returns>
        List<Point> GetContainersHistory(string containerUid);

        /// <summary>
        ///     Get latest status of container.
        /// </summary>
        /// <param name="containerStatus"></param>
        void SetContainerStatus(ContainerStatus containerStatus);
    }
}
