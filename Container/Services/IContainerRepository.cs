using System.Collections.Generic;
using Common;
using Infrastructure.Container.DTOs;
using Infrastructure.Container.Entities;

namespace Infrastructure.Container.Services
{
    public interface IContainerRepository : IRepository
    {
        /// <summary>
        ///     Get container by uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Entities.Container GetContainer(string uid);

        /// <summary>
        ///     Get container by uid including relationships
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Entities.Container GetContainerInlcudeRelationships(string uid);

        #region HACK - TODO Refactor for use of users beloning to mulpitle sites

        /// <summary>
        ///     Get the Site as a container belonging to the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Entities.Container GetUsersSiteContainer(int userId);

        /// <summary>
        ///     Get the Site as a container belonging to the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Entities.Container GetUsersSiteContainer(string username);

        /// <summary>
        ///     Get the Ident record from the ident name.
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        ContainerIdent GetIdent(string ident);

        /// <summary>
        ///     Get the container ident from its tagIdent.
        /// </summary>
        /// <param name="tagIdent"></param>
        /// <returns></returns>
        ContainerIdent GetIdent(int tagIdent);

        /// <summary>
        ///     Retreive the users personal pick list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetUserPickListId(int userId);

        /// <summary>
        ///     Add item to pick list.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        void AddContainerToPicklist(int id, int listId);

        /// <summary>
        ///     Get the recorded container in a storage location.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="storageIndex"></param>
        /// <returns></returns>
        Entities.Container GetContainerByStorageIndex(int parentId, int storageIndex);

        /// <summary>
        ///     Is the container on the pick list?
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        bool IsItemOnPickList(int containerId);

        /// <summary>
        ///     Get list of containers by their uid.
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        IEnumerable<Entities.Container> GetContainers(IEnumerable<string> uids);

        /// <summary>
        ///     Are the containers on pick list?
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        IEnumerable<int> AreItemsOnPickList(IEnumerable<int> containerIds);

        /// <summary>
        ///     Remove the pick list item.
        /// </summary>
        /// <param name="containerId"></param>
        void DeletePicklistEntry(int containerId);

        /// <summary>
        ///     Get a containers childrens by its primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Entities.Container> GetStorageChildren(int id);

        /// <summary>
        ///     Get a containers childrens by its uid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ContainerResponseBody> GetStorageChildrenResponseBodies(string uid);

        /// <summary>
        ///     Get a container response body by its primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ContainerResponseBody GetContainerReponseBody(int id);

        /// <summary>
        ///     Get a container response body by its uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        ContainerResponseBody GetContainerReponseBody(string uid);

        /// <summary>
        ///     
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ContainerResponseBody> GetDisposedItems();

        IEnumerable<ContainerIdent> GetContainerIdents(IEnumerable<int> ids);
        
        #endregion
    }
}
