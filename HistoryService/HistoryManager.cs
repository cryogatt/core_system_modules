using History.Entities;
using Infrastructure.Container.Entities;
using Infrastructure.Container.Services;
using Infrastructure.History.DTOs;
using Infrastructure.History.Entities;
using Infrastructure.History.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HistoryService
{
    public class HistoryManager : IHistoryManager
    {
        private readonly IHistoryRepository Repository;
        private readonly IContainerRepository ContainerRepository;

        public HistoryManager(IHistoryRepository repository, IContainerRepository containerRepository)
        {
            Repository = repository;
            ContainerRepository = containerRepository;
        }

        /// <summary>
        ///     Add a points record to the database.
        /// </summary>
        public void AddCheckpoint(Point point)
        {
            // Set incept date if required.
            if (Repository.IsInceptDateSet(point.ContainerId))
                point.IsBeingStoredForFirstTime();

            // Stage
            Repository.Add(point);
        }

        /// <summary>
        ///     Add a points record to the database.
        /// </summary>
        public void AddCheckpoints(IEnumerable<Point> points)
        {
            // Set incept date if required.
            foreach (var point in points)
                if (Repository.IsInceptDateSet(point.ContainerId))
                    point.IsBeingStoredForFirstTime();

            // Stage
            Repository.AddRange(points);
        }

        /// <summary>
        ///     Add a points record to the database.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="what"></param>
        /// <param name="where"></param>
        /// <param name="userId"></param>
        public void AddCheckpoint(Container container, string what, string where, int userId)
        {
            // Update the history
            AddCheckpoint(new Point(
                0,
                container.Id,
                container.Uid,
                container.Description,
                container.ContainerIdentId,
                container.ParentContainerStorageId,
                container.StorageIndex,
                container.ParentContainerLocationId,
                container.LocationIndex,
                container.Flags,
                container.InceptDate.GetValueOrDefault(),
                userId,
                DateTime.Now,
                what,
                where));
        }

        /// <summary>
        ///     Add points records to the database.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="what"></param>
        /// <param name="where"></param>
        /// <param name="userId"></param>
        public void AddCheckpoints(IEnumerable<Container> containers, string what, string where, int userId)
        {
            // Update the history
            var points = containers
                            .Select(container =>
                                        new Point(
                                            0,
                                            container.Id,
                                            container.Uid,
                                            container.Description,
                                            container.ContainerIdentId,
                                            container.ParentContainerStorageId,
                                            container.StorageIndex,
                                            container.ParentContainerLocationId,
                                            container.LocationIndex,
                                            container.Flags,
                                            container.InceptDate.GetValueOrDefault(),
                                            userId,
                                            DateTime.Now,
                                            what,
                                            where));

            AddCheckpoints(points);
        }

        /// <summary>
        ///     Get containers full history.
        /// </summary>
        /// <param name="containerUid"></param>
        /// <returns></returns>
        public HistoryResponse GetContainerHistory(string containerUid)
        {
            // Get data records
            var checkpoints = Repository.GetContainersHistory(containerUid)
                .ToList();

            // Convert to responsebodies
            var responseBodies = checkpoints
                .Select(c => new HistoryResponseBody(
                    c.User.Username,
                    c.Reason,
                    c.Location,
                    c.Mark))
                    .ToList();

            return new HistoryResponse(
                responseBodies.Count(),
                responseBodies);
        }
        
        /// <summary>
        ///     Mark siblings not found in given containers as missing.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="siblings"></param>
        public void MarkMissingContainers(List<Container> containers, int userId, string where, List<Container> siblings)
        {
            // Find all missing containers
            var missingContainers = siblings
                .Where(s => !containers.Any(c => c.Uid == s.Uid))
                .ToList();

            var parent = Repository.Get<Container>(missingContainers.First().ParentContainerStorageId.Value);

            // Mark them as missing
            AddCheckpoints(missingContainers, "Missing from " + parent.Description, where, userId);

            // Remove from storage
            missingContainers.ForEach(c => c.ParentContainerStorageId = null);

            Repository.UpdateEntities(missingContainers);

            var updatedStatuses = missingContainers
                                        .Select(c => 
                                            new ContainerStatus(
                                            0,
                                            c.Uid,
                                            "Missing"));

            SetContainerStatus(updatedStatuses, "", userId);
        }

        public void SetContainerStatus(IEnumerable<ContainerStatus> containerStatuses)
        {
            foreach (var containerStatus in containerStatuses)
            {
                var container = ContainerRepository.GetContainer(containerStatus.ContainerUid);

                if (container == null)
                    throw new ArgumentException($"Container with uid {containerStatus.ContainerUid} not in db");

                Repository.SetContainerStatus(containerStatus);
            }
        }

        public void SetContainerStatus(IEnumerable<ContainerStatus> containerStatuses, string userLocation, int userId)
        {
            foreach (var containerStatus in containerStatuses)
            {
                var container = ContainerRepository.GetContainer(containerStatus.ContainerUid);

                if (container == null)
                    throw new ArgumentException($"Container with uid {containerStatus.ContainerUid} not in db");

                Repository.SetContainerStatus(containerStatus);

                var reason = "Status Changed: " + containerStatus.Status;

                AddCheckpoint(container, reason, userLocation, userId);
            }
        }
    }
}
