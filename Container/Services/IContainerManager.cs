using ContainerLevels;
using Infrastructure.Container.DTOs;
using Infrastructure.Container.Entities;
using System.Collections.Generic;

namespace Infrastructure.Container.Services
{
    public interface IContainerManager
    {
        /// <summary>
        ///     Add a new record.
        /// </summary>
        /// <param name="containerResponseBody"></param>
        /// <param name="parentContainerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int AddContainers(ContainerResponseBody containerResponseBody, int userId, string parentContainerUid = null,
                          IRuleContainerLevelCalculator ruleContainerLevelCalculator = null);

        IEnumerable<ContainerIdent> GetContainerIdents(IEnumerable<int> ids);

        /// <summary>
        ///     Update record.
        /// </summary>
        /// <param name="containerResponseBody"></param>
        /// <returns></returns>
        void UpdateContainer(ContainerResponseBody containerResponseBody, int userId);

        /// <summary>
        ///     Mark an item as disposed. 
        /// </summary>
        /// <returns></returns>
        void AddDisposedItem(string uid, int userId, string location);
        
        /// <summary>
        ///     Get container by uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Entities.Container GetContainer(string uid);

        /// <summary>
        ///     Get list of containers by their uid.
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        IEnumerable<Entities.Container> GetContainers(IEnumerable<string> uids);

        /// <summary>
        ///     Get container response body by uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        ContainerResponseBody GetContainerResponseBody(string uid);

        /// <summary>
        ///     Get the type description from the tag ident.
        /// </summary>
        /// <param name="tagIdent"></param>
        /// <returns></returns>
        string GetTypeDesc(int tagIdent);

        /// <summary>
        ///     Get the contents from a container.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        ContainerResponse GetContainerContentsResponse(string uid = null);

        /// <summary>
        ///     Get the recorded container in a storage position.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="storageIndex"></param>
        /// <returns></returns>
        Entities.Container GetContainerByStorageIndex(int parentId, int storageIndex);

        /// <summary>
        ///     Get the contents from a container.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        IEnumerable<Entities.Container> GetContainerContents(string uid);

        /// <summary>
        ///     Get all parents of a container.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        ContainerResponse GetContainersParents(string uid);

        /// <summary>
        ///     Get the level of the given containers contents.
        ///     Assumes contents are all of same level.
        /// </summary>
        /// <param name="parentUid"></param>
        /// <param name="ruleContainerLevelCalculator"></param>
        /// <returns></returns>
        ContainerLevelTypes GetContainerLevel(string parentUid, IRuleContainerLevelCalculator ruleContainerLevelCalculator);

        /// <summary>
        ///     Update the database that the containers has been stored.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="parentDescription"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        void UpdateContainersBeingStored(List<Entities.Container> containers, int userId, string location);

        /// <summary>
        ///     Update the database that the containers have been withdrawn.
        /// </summary>
        /// <param name="movedContainers"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        void UpdateContainersBeingWithdrawn(List<Entities.Container> movedContainers, int userId, string location);

        /// <summary>
        ///     Find the container level for the given tag ident.
        /// </summary>
        /// <param name="tagIdent"></param>
        /// <param name="ruleContainerLevelCalculator"></param>
        /// <returns></returns>
        ContainerLevelTypes GetContainerLevel(int tagIdent, IRuleContainerLevelCalculator ruleContainerLevelCalculator);

        /// <summary>
        ///     Delete list of uids from the pick list.
        /// </summary>
        /// <param name="uids"></param>
        void DeletePicklistEntries(List<string> uids);

        /// <summary>
        ///     Mark a list of containers as audited.
        /// </summary>
        /// <param name="uids"></param>
        void AuditContainers(List<Entities.Container> containers, int userId, string where);

        /// <summary>
        ///     Create audit record with audit message.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        /// <param name="audit"></param>
        void AuditContainers(List<Entities.Container> containers, int userId, string location, string audit);

        /// <summary>
        ///     Update the database that the containers has been shipped.
        /// </summary>
        /// <param name="containers"></param>
        void UpdateContainersBeingShipped(List<Entities.Container> container);

        #region HACK - TODO Refactor for use of users beloning to mulpitle sites

        /// <summary>
        ///     Get the container record of a users site.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Entities.Container GetUsersSiteContainer(string username);

        /// <summary>
        ///     Get the container of first site site which the user belongs to. 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        ContainerResponse GetUsersSiteContents(string username);

        /// <summary>
        ///     Add containers to a users pick list.
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="userId"></param>
        void AddContainersToPickList(List<string> uids, int userId);

        /// <summary>
        ///     Add containers to a users pick list.
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="userId"></param>
        void AddContainersToPickList(List<int> containerIds, int userId);

        /// <summary>
        ///     Get Container Ident from its tagident.
        /// </summary>
        /// <param name="tagIdent"></param>
        /// <returns></returns>
        ContainerIdent GetContainerIdentByTagIdent(int tagIdent);

        /// <summary>
        ///     Get Container Ident from its primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ContainerIdent GetContainerIdent(int id);

        List<ContainerResponseBody> GetDisposedItems();

        #endregion
    }
}
