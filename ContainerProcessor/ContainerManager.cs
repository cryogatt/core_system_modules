using Infrastructure.Container.DTOs;
using Infrastructure.Container.Services;
using System.Linq;
using Infrastructure.Container.Entities;
using ContainerLevels;
using ContainerTypes;
using System;
using System.Collections.Generic;
using Infrastructure.History.Services;
using Infrastructure.History.Entities;
using Cryogatt.RFID.Trace;

namespace ContainerService
{
    public class ContainerManager : IContainerManager
    {
        #region Private Properties

        /// <summary>
        ///     Data access to repository.
        /// </summary>
        private readonly IContainerRepository Repository;

        /// <summary>
        ///     For audit.
        /// </summary>
        private readonly IHistoryManager HistoryManager;

        /// <summary>
        ///     Used for uid creation.
        /// </summary>
        private static readonly Random Random = new Random();

        #endregion

        #region Constructors

        public ContainerManager(IContainerRepository repository, IHistoryManager historyManager)
        {
            Repository = repository;
            HistoryManager = historyManager;
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        ///     Add a new record. If containerResponseBody.Uid is not provided generated fake uid.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public int AddContainers(ContainerResponseBody containerResponseBody, int userId, string parentContainerUid = null, 
            IRuleContainerLevelCalculator ruleContainerLevelCalculator = null)
        {
            if (parentContainerUid == null && userId == 0)
                throw new Exception("Parameters not set");

            // If not provided assume container is Non-RFID enabled.
            if (containerResponseBody.Uid == null)
                containerResponseBody.Uid = GenerateUid();

            // Add the container with the appropiate parent id
            var id = (parentContainerUid == null) 
                ? AddContainerToUserLocation(containerResponseBody, userId, ruleContainerLevelCalculator)
                : AddContainer(containerResponseBody, userId, parentContainerUid);

            // Add contents as well
            if (containerResponseBody.ContainsQtty != 0)
                AddContainerContents(containerResponseBody, id, userId);

            return id;
        }

        /// <summary>
        ///     Get container by uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Container IContainerManager.GetContainer(string uid)
        {
            return Repository.GetContainer(uid);
        }

        /// <summary>
        ///     Get list of containers by their uid.
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        public IEnumerable<Container> GetContainers(IEnumerable<string> uids)
        {
            return Repository.GetContainers(uids);
        }

        /// <summary>
        ///     Get container by uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ContainerResponseBody GetContainerResponseBody(string uid)
        {
            return Repository.GetContainerReponseBody(uid);
         }

        /// <summary>
        ///     Get the contents of a container.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ContainerResponse GetContainerContentsResponse(string uid)
        {
            // Create repsonse body
            var children = Repository.GetStorageChildrenResponseBodies(uid);

            if (children == null)
                return null;

            // Create and return response
            return new ContainerResponse(
                children.Count(),
                children); 
        }

        /// <summary>
        ///     Get the contents from a container.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IEnumerable<Container> GetContainerContents(string uid)
        {
            // Get record
            var parent = Repository.GetContainerInlcudeRelationships(uid);

            // Does exist?
            if (parent == null)
                return null;

            // Has contents?
            if (parent.StorageChildren == null || parent.StorageChildren.Count == 0)
                return null;

            // Create repsonse body
            return parent.StorageChildren;
        }

        /// <summary>
        ///     Get the container record of a users site.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Container GetUsersSiteContainer(string username)
        {
            return Repository.GetUsersSiteContainer(username);
        }

        /// <summary>
        ///     Get the contents of the users site.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ContainerResponse GetUsersSiteContents(int userId)
        {
            // Get container
            var site = Repository.GetUsersSiteContainer(userId);

            // Create repsonse body
            var children = Repository.GetStorageChildrenResponseBodies(site.Uid);

            // Create and retrun response
            return new ContainerResponse(
                children.Count(),
                children.ToList());
        }

        /// <summary>
        ///     Get the contents of the users site.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ContainerResponse GetUsersSiteContents(string username)
        {
            // Get container
            var site = Repository.GetUsersSiteContainer(username);

            // Create repsonse body
            var children = Repository.GetStorageChildrenResponseBodies(site.Uid);

            // Create and retrun response
            return new ContainerResponse(
                children.Count(),
                children);
        }

        /// <summary>
        ///     Update the container record
        /// </summary>
        /// <param name="containerResponseBody"></param>
        /// <returns></returns>
        public void UpdateContainer(ContainerResponseBody containerResponseBody, int userId)
        {
            if (containerResponseBody == null)
                throw new ArgumentException();

            var container = Repository.GetContainer(containerResponseBody.Uid);

            // Throw if not found
            if (container == null)
                throw new Exception("container not found");
            // Update record
            container.Update(containerResponseBody);
            
            Repository.Update(container);

            // Update history - TODO Right now only name can change.
            Audit(
                userId,
                container, 
                Repository.Get<Container>(container.Id),
                "Renamed from " + containerResponseBody.Description + " to " + container.Description);
        }

        /// <summary>
        ///     Get all parents of a container.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ContainerResponse GetContainersParents(string uid)
        {
            var container = Repository.GetContainer(uid);

            // Return null if not found
            if (container == null)
                return null;

            var resp = new List<ContainerResponseBody>();
            // Get parent
            var parent = Repository.Get<Container>(container.ParentContainerStorageId.GetValueOrDefault());
            var parentContainerResponseBody = Repository.GetContainerReponseBody(container.ParentContainerStorageId.GetValueOrDefault());

            bool allParentsFound = false;
            // Loop through each parent of container and add them to the list until the root level is found
            while (!allParentsFound)
            {
                resp.Add(parentContainerResponseBody);
                if (!(parent.ParentContainerStorageId == null || parent.ParentContainerStorageId == 0))
                {
                    parent = Repository.Get<Container>(parent.ParentContainerStorageId.GetValueOrDefault());
                    parentContainerResponseBody = Repository.GetContainerReponseBody(parent.Id);

                    // If parent is the location
                    if (parentContainerResponseBody.TagIdent >> 16 == GeneralTypes.LOCATION_TYPE_ID)
                    {
                        allParentsFound = true;
                        break;
                    }
                }
                else
                {
                    allParentsFound = true;
                }
            }

            return new ContainerResponse(resp.Count, resp);
        }

        /// <summary>
        ///     Get the level of the given containers contents.
        ///     Assumes contents are all of same level.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="ruleContainerLevelCalculator"></param>
        /// <returns></returns>
        ContainerLevelTypes IContainerManager.GetContainerLevel(string parentUid, IRuleContainerLevelCalculator ruleContainerLevelCalculator)
        {
            // Get record
            var parent = Repository.GetContainerInlcudeRelationships(parentUid);

            // Does exist?
            if (parent == null)
                throw new Exception("Uid not in database: " + parentUid);

            // Has contents?
            if (parent.StorageChildren == null || parent.StorageChildren.Count == 0)
                return 0;

            var childIdent = Repository.Get<ContainerIdent>(parent.StorageChildren.First().ContainerIdentId);

            // Get the level of the first child.
            return ruleContainerLevelCalculator.GetLevel(childIdent.TagIdent >> 16);
        }

        /// <summary>
        ///     Get the level for a given container ident.
        /// </summary>
        /// <param name="tagIdent"></param>
        /// <param name="ruleContainerLevelCalculator"></param>
        /// <returns></returns>
        public ContainerLevelTypes GetContainerLevel(int tagIdent, IRuleContainerLevelCalculator ruleContainerLevelCalculator)
        {
            // Find record
            var subtype = ContainerIdentTypes.Subtypes
                .Where(c => c.TagIdent == tagIdent)
                .Single();

            // Exists (has default value)
            if (subtype.TagIdent == 0)
                return 0;

            // Get the level of the first child.
            return ruleContainerLevelCalculator.GetLevel(tagIdent >> 16);
        }

        /// <summary>
        ///     Add containers to a users pick list.
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="userId"></param>
        public void AddContainersToPickList(List<string> uids, int userId)
        {
            var containers = uids
                .Select(uid => Repository.GetContainer(uid));

            // if all uids are in the container database
            if (containers.Any(c => c == null))
                throw new Exception("Container(s) passed not in database!");

            // Get the location of user
            var location = Repository.GetUsersSiteContainer(userId);

            // Get the users pick list Id
            var listId = Repository.GetUserPickListId(userId);

            foreach (var container in containers)
            {
                if (container.ParentContainerStorageId == (null) || container.ParentContainerStorageId == 0)
                    continue;

                // If container is on the same site as user - TODO add message for container belonging to another site
                if (!IsContainerOnSite(container, location))
                    continue;
                                
                // Add to list
                Repository.AddContainerToPicklist(container.Id, listId);               
            }
        }

        public void AddContainersToPickList(List<int> containerIds, int userId)
        {
            var containers = containerIds
                .Select(id => Repository.Get<Container>(id));

            // if all uids are in the container database
            if (containers.Any(c => c == null))
                throw new Exception("Container(s) passed not in database!");

            // Get the location of user
            var location = Repository.GetUsersSiteContainer(userId);

            // Get the users pick list Id
            var listId = Repository.GetUserPickListId(userId);

            foreach (var container in containers)
            {
                if (container.ParentContainerStorageId == (null) || container.ParentContainerStorageId == 0)
                    continue;

                // If container is on the same site as user - TODO add message for container belonging to another site
                if (!IsContainerOnSite(container, location))
                    continue;

                // Add to list
                Repository.AddContainerToPicklist(container.Id, listId);
            }
        }

        /// <summary>
        ///     Delete list of uids from the pick list.
        /// </summary>
        /// <param name="uids"></param>
        public void DeletePicklistEntries(List<string> uids)
        {
            var containers = uids
                .Select(uid => Repository.GetContainer(uid))
                .ToList();

            if (containers.Any(c => c == null))
                throw new Exception("One of more uids not regonised!");

            containers.ForEach(container => 
                Repository.DeletePicklistEntry(container.Id));
        }

        /// <summary>
        ///     Audit containers.
        /// </summary>
        /// <param name="containers"></param>
        public void AuditContainers(List<Container> containers, int userId, string where)
        {
            // Check all items belong to the same parent
            if (!containers.TrueForAll(c => c.ParentContainerStorageId == containers.First().ParentContainerStorageId))
                throw new Exception("Cannot audit containers with different parents");

            // Get parent
            var parent = Repository.Get<Container>(containers.First().ParentContainerStorageId.GetValueOrDefault());

            // Get all siblings
            var siblings = GetContainerContents(parent.Uid).ToList();

            // If number of items in parent does not match siblings of container
            if (containers.Count() != siblings.Count())
                HistoryManager.MarkMissingContainers(containers, userId, where, siblings);

            HistoryManager.AddCheckpoints(containers, "Audited", where, userId);
        }

        /// <summary>
        ///     Get the type description from the tagident.
        /// </summary>
        /// <param name="tagIdent"></param>
        /// <returns></returns>
        public string GetTypeDesc(int tagIdent)
        {
            var type = ContainerIdentTypes.Types
                .Where(t => t.Ident == (tagIdent >> 16))
                .SingleOrDefault();

            return type.Description;
        }

        /// <summary>
        ///     Get the recorded container in a storage location.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="storageIndex"></param>
        /// <returns></returns>
        public Container GetContainerByStorageIndex(int parentId, int storageIndex)
        {
            if (Repository.Get<Container>(parentId) == null)
                throw new ArgumentException("parentId does not exist in database: " + parentId);

            return Repository.GetContainerByStorageIndex(parentId, storageIndex);
        }

        /// <summary>
        ///     Use the tagidentto get the containerIdent record.
        /// </summary>
        /// <param name="tagIdent"></param>
        /// <returns></returns>
        public ContainerIdent GetContainerIdentByTagIdent(int tagIdent)
        {
            return Repository.GetIdent(tagIdent);
        }

        /// <summary>
        ///     Get Container Ident from its primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContainerIdent GetContainerIdent(int id)
        {
            return Repository.Get<ContainerIdent>(id);
        }

        public IEnumerable<ContainerIdent> GetContainerIdents(IEnumerable<int> ContainerIdentIds)
        {
            return Repository.GetContainerIdents(ContainerIdentIds);
        }

        /// <summary>
        ///     Update the database that the container has been stored.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="parentDescription"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        public void UpdateContainersBeingStored(List<Container> containers, int userId, string location)
        {
            // Stage
            Repository.UpdateEntities(containers);

            // Audit
            containers.ForEach(c =>
                HistoryManager.AddCheckpoint(
                    c,
                    $"{c.Description} stored in " +
                    Repository.Get<Container>(c.ParentContainerStorageId.GetValueOrDefault()).Description + 
                    (c.StorageIndex != 0 ? " Position: " + c.StorageIndex : ""),
                    location,
                    userId));
        }

        /// <summary>
        ///     Update the database that the containers have been withdrawn.
        /// </summary>
        /// <param name="movedContainers"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        public void UpdateContainersBeingWithdrawn(List<Container> movedContainers, int userId, string location)
        {
            // Check container if container are on the pick list
            var pickListItemIds = Repository
                                    .AreItemsOnPickList(movedContainers
                                                           .Select(c => c.Id));
                        
            // Remove
            if (pickListItemIds != null || pickListItemIds.Count() > 0)
                pickListItemIds
                    .ToList()
                    .ForEach(id => Repository.DeletePicklistEntry(id));

            // Stage
            Repository.UpdateEntities(movedContainers);

            // Audit
            HistoryManager.AddCheckpoints(
                            movedContainers,
                            "Sample removed from storage",
                            location,
                            userId);
        }

        /// <summary>
        ///     Update the database that the containers has been shipped.
        /// </summary>
        /// <param name="containers"></param>

        public void UpdateContainersBeingShipped(List<Container> containers)
        {
            // Stage
            Repository.UpdateEntities(containers);
            //containers.ForEach(c => Repository.Update(c));
        }

        /// <summary>
        ///     Create audit record.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        /// <param name="audit"></param>
        public void AuditContainers(List<Container> containers, int userId, string location, string audit)
        {
            containers.ForEach(c =>
                HistoryManager.AddCheckpoint(
                    c,
                    $"{c.Description} " + audit,
                    location,
                    userId));
        }

        /// <summary>
        ///     Loop through parents of container to determine if the site parent matches site.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public bool IsContainerOnSite(Container container, Container site)
        {
            // Get initial parent
            var parentContainer = Repository.Get<Container>(container.ParentContainerStorageId.Value);

            // Loop through parents
            for (int i = 0; i < 10; i++) // Hardcoded 10 as limit- hack?
            {
                if (parentContainer == null)
                    return false;

                //If parent container is a location
                if ((GetContainerIdent(parentContainer.ContainerIdentId).TagIdent >> 16) == GeneralTypes.LOCATION_TYPE_ID)
                {
                    // If parent container is site
                    if (parentContainer.Uid == site.Uid)
                        return true;
                }
                else
                {
                    if (parentContainer.ParentContainerStorageId == null)
                        break;

                    // Go up one level
                    parentContainer = Repository.Get<Container>(parentContainer.ParentContainerStorageId.Value);
                }
            }
            return false;
        }

        public List<ContainerResponseBody> GetDisposedItems()
        {
            Log.Debug("Invoked");

            return Repository.GetDisposedItems();
        }
               
        public void AddDisposedItem(string uid, int userId, string location)
        {
            Log.Debug("Invoked");

            var container = Repository.GetContainer(uid);

            if (container == null)
                throw new ArgumentException($"{uid} does not exist in db");

            if (container.Flags == 1)
                return;

            container.SetAsDisposed();

            Repository.Update(container);

            HistoryManager.AddCheckpoint(
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
                    container.InceptDate.Value,
                    userId,
                    DateTime.Now,
                    "Item marked as disposed",
                    location));
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Adds the contents of a container to the database. 
        /// </summary>
        /// <param name="parentResponseBody"></param>
        /// <param name="parentId"></param>
        private void AddContainerContents(ContainerResponseBody parentResponseBody, int parentId, int userId)
        {
            // Get the ident of the subtype
            var ident = Repository.GetIdent(parentResponseBody.ContainsIdent);

            // Type of container
            var type = ContainerIdentTypes.Types
                .Where(g => (ident.TagIdent >> 16) == g.Ident)
                .SingleOrDefault();

            // Stage all contents to database
            for (var i = 0; i < parentResponseBody.ContainsQtty; i++)
            {
                var container = new Container(
                        0,
                        GenerateUid(),
                        type.Description + " " + (i + 1),
                        ident.Id,
                        parentId,
                        0,
                        0,
                        0,
                        0,
                        DateTime.Now);

                // Stage
                Repository.Add(container);

                // Audit
                Audit(userId, Repository.Get<Container>(parentId), container, "Added container to database");
            }
        }

        /// <summary>
        ///     Create a random uid string.
        /// </summary>
        /// <returns>The generated uid</returns>
        private string GenerateUid()
        {
            Log.Debug("Invoked");

            string result;

            const string chars = "ABCDEFabcdef0123456789";
            object monitor = new object();
            lock (monitor)
            {
                result = "e0ff" + new string(Enumerable.Repeat(chars, 12).Select(s => s[Random.Next(s.Length)])
                             .ToArray());
            }

            return result;
        }

        /// <summary>
        ///     Add a container to the database - does not include storage index.
        /// </summary>
        /// <param name="containerResponseBody"></param>
        /// <param name="parentContainerId"></param>
        /// <returns></returns>
        private int AddContainer(ContainerResponseBody containerResponseBody, int userId, string parentContainerUid)
        {
            // Get parent record
            var parentContainer = Repository.GetContainer(parentContainerUid);

            if (parentContainer == null)
                throw new Exception("Container not found");

            // Get the container Ident
            var ident = Repository.GetIdent(containerResponseBody.TagIdent);

            if (ident == null)
                throw new Exception("Ident not found");

            // Create the entity
            Container container = new Container(0,
                containerResponseBody.Uid,
                containerResponseBody.Description,
                ident.Id,
                parentContainer.Id,
                0,
                0,
                0,
                0,
                DateTime.Now);  // Review

            // Stage
            Repository.Add(container);

            // Update history
            Audit(userId, parentContainer, container, "Sample added to database");

            return container.Id;
        }

        /// <summary>
        ///     Add container with parentId set to the container of the users site location.
        ///     Note, does not include storage index.
        /// </summary>
        /// <param name="containerResponseBody"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int AddContainerToUserLocation(ContainerResponseBody containerResponseBody, int userId, IRuleContainerLevelCalculator ruleContainerLevelCalculator)
        {
            // Get the container Ident either by tagident or its description
            var ident = containerResponseBody.Ident == null
                ? Repository.GetIdent(containerResponseBody.TagIdent)
                : Repository.GetIdent(containerResponseBody.Ident);

            if (ident == null)
                throw new Exception("Ident not found");

            // If container is not made to be stored in another container set it to the users location.
            Container parent = new Container(); int? parentId = null;
            if (GetContainerLevel(ident, ruleContainerLevelCalculator) == ContainerLevelTypes.ROOT_LEVEL_CONTAINER)
            {
                // Get the container record belonging to the site
                var location = Repository.GetUsersSiteContainer(userId);
                if (location == null)
                    throw new Exception("location not found");
                else
                    parent = location;

                parentId = parent.Id;
            }
            
            Container container = new Container(
                0,
                containerResponseBody.Uid,
                containerResponseBody.Description,
                ident.Id,
                parentId,
                0,
                0,
                0,
                0,
                DateTime.Now);  // Review

            // Stage
            Repository.Add(container);

            // Update history
            Audit(userId, parent, container, $"{containerResponseBody.Description} added to database");

            return container.Id;
        }

        /// <summary>
        ///     Update history.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="parent"></param>
        /// <param name="container"></param>
        private void Audit(int userId, Container parent, Container container, string reason)
        {
            HistoryManager.AddCheckpoint(new Point(
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
                reason,
                parent.Description));
        }

        /// <summary>
        ///     Get the level of a container model (ident).
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        private ContainerLevelTypes GetContainerLevel(ContainerIdent ident, IRuleContainerLevelCalculator ruleContainerLevelCalculator)
        {
            // Get the type of container
            var type = ContainerIdentTypes.Subtypes
                .Where(s => s.TagIdent == ident.TagIdent).First();

            // Get the level of container
            return ruleContainerLevelCalculator.GetLevel((Int32)type.TagIdent >> 16);
        }
        
        #endregion
    }
}
