using CommonEF;
using CommonEF.Services;
using Cryogatt.RFID.Trace;
using Infrastructure.Container.DTOs;
using Infrastructure.Container.Entities;
using Infrastructure.Container.Services;
using Infrastructure.PickList.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ContainerDAL
{
    public class ContainerRepository : Repository, IContainerRepository
    {
        #region Constructors

        public ContainerRepository(IContextFactory contextFactory) : base(contextFactory)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Get container by uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Container GetContainer(string uid)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Containers
                        .Where(c => c.Uid == uid)
                        .SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);  throw;
            }
        }

        /// <summary>
        ///     Get container by uid including relationships.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Container GetContainerInlcudeRelationships(string uid)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Containers
                    .Where(c => c.Uid == uid)
                    .Include(c => c.StorageChildren)
                    .Include(c => c.ContainerIdent)
                    .SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }

        /// <summary>
        ///     Get the ident record by name.
        /// </summary>
        /// <param name="identName"></param>
        /// <returns></returns>
        public ContainerIdent GetIdent(string identName)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.ContainerIdents
                    .Where(c => c.Description == identName)
                    .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }

        /// <summary>
        ///     Get the ident record by name.
        /// </summary>
        /// <param name="tagIdent"></param>
        /// <returns></returns>
        public ContainerIdent GetIdent(int tagIdent)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.ContainerIdents
                    .Where(c => c.TagIdent == tagIdent)
                    .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }

        /// <summary>
        ///     Get the site as a container belonging to the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Container GetUsersSiteContainer(int userId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Users
                        .Where(c => c.Id == userId)
                        .SingleOrDefault()
                        .Sites.First()
                        .Container;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }

        /// <summary>
        ///     Get the site as a container belonging to the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Container GetUsersSiteContainer(string username)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Users
                        .Where(c => c.Username == username)
                        .SingleOrDefault()
                        .Sites.First()
                        .Container;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }

        /// <summary>
        ///     Retreive the users personal pick list.
        ///     Creates one if does not exist.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserPickListId(int userId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    var pl = context.PickLists
                    .Where(p => p.UserId == userId)
                    .SingleOrDefault();

                    // if not in db - TODO Remove this logic from repository layer.
                    if (pl == null)
                    {
                        // Create new list
                        var userPickList = new PickList(
                            0,
                            context.Users.Where(u => u.Id == userId).First().Username + " Pick List",
                            userId);
                        
                        Add(userPickList);
                    }
                    return pl.Id;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        /// Add a container id to the picklist unless already there or already booked out
        /// </summary>
        /// <param name="id">The container id</param>
        /// <param name="listId"></param>
        public void AddContainerToPicklist(int id, int listId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {

                    var item = context.PickListItems
                        .Where(p => p.ContainerId == id &&
                        p.PickListId == listId)
                        .FirstOrDefault();

                    // Already exist in the list?
                    if (item == null)
                    {
                        var newItem = new PickListItem(0, listId, id);
                        Add(newItem);
                    }
                    else
                    {
                        if (item.PickListId != listId)
                        {
                            item.Update(listId);
                            Update(item);
                        }
                    }
                }            
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get the recorded container in a storage location.
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="storageIndex"></param>
        /// <returns></returns>
        public Container GetContainerByStorageIndex(int parentId, int storageIndex)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Containers
                    .Where(c =>
                    c.ParentContainerStorageId == parentId &&
                    c.StorageIndex == storageIndex)
                    .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Does the container exist on pick list?
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public bool IsItemOnPickList(int containerId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.PickListItems
                    .Any(i => i.ContainerId == containerId);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Are the containers on pick list?
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public IEnumerable<int> AreItemsOnPickList(IEnumerable<int> containerIds)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.PickListItems
                            .Where(item => containerIds.Contains(item.ContainerId))
                            .Select(item => item.ContainerId)
                            .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Remove the container from the pick list.
        /// </summary>
        /// <param name="containerId"></param>
        public void DeletePicklistEntry(int containerId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = new CommonEF.Cryogatt())
                {

                    var entries = context.PickListItems
                        .Where(p => p.ContainerId == containerId)
                        .AsEnumerable();

                    context.Set<PickListItem>().RemoveRange(entries);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get a containers childrens by its primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Container> GetStorageChildren(int id)
        {
            try
            {
                using (var context = new CommonEF.Cryogatt())
                {
                    return context.Containers
                        .Find(id)
                        .StorageChildren
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get a containers childrens by its uid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ContainerResponseBody> GetStorageChildrenResponseBodies(string uid)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Containers
                        .Where(c => c.Uid == uid)
                        .SingleOrDefault()
                        .StorageChildren
                        .Select(c =>
                        new ContainerResponseBody
                        {
                            Uid = c.Uid,
                            Description = c.Description,
                            Ident = c.ContainerIdent.Description,
                            TagIdent = c.ContainerIdent.TagIdent,
                            InceptDate = c.InceptDate,
                            ContainsQtty = c.StorageChildren.Count,
                            ContainsIdent = c.StorageChildren.FirstOrDefault() != null
                            ? c.StorageChildren.FirstOrDefault().Description
                            : null
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get a container response body by its primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContainerResponseBody GetContainerReponseBody(int id)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Containers
                        .Where(c => c.Id == id)                        
                        .Select(c =>
                        new ContainerResponseBody
                        {
                            Uid = c.Uid,
                            Description = c.Description,
                            Ident = c.ContainerIdent.Description,
                            TagIdent = c.ContainerIdent.TagIdent,
                            InceptDate = c.InceptDate,
                            ContainsQtty = c.StorageChildren.Count,
                            ContainsIdent = c.StorageChildren.FirstOrDefault().Description
                        })
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get a container response body by its uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ContainerResponseBody GetContainerReponseBody(string uid)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Containers
                        .Where(c => c.Uid == uid)
                        .Select(c =>
                        new ContainerResponseBody
                        {
                            Uid = c.Uid,
                            Description = c.Description,
                            Ident = c.ContainerIdent.Description,
                            TagIdent = c.ContainerIdent.TagIdent,
                            InceptDate = c.InceptDate,
                            ContainsQtty = c.StorageChildren.Count,
                            ContainsIdent = c.StorageChildren.FirstOrDefault().Description
                        })
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        public List<ContainerResponseBody> GetDisposedItems()
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Containers
                        .Where(c => c.Flags == 1)
                        .Select(c =>
                        new ContainerResponseBody
                        {
                            Uid = c.Uid,
                            Description = c.Description,
                            Ident = c.ContainerIdent.Description,
                            TagIdent = c.ContainerIdent.TagIdent,
                            InceptDate = c.InceptDate,
                            ContainsQtty = c.StorageChildren.Count,
                            ContainsIdent = c.StorageChildren.FirstOrDefault().Description
                        })
                        .ToList();                        
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        public IEnumerable<Container> GetContainers(IEnumerable<string> uids)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Containers
                                    .Where(c => uids.Contains(c.Uid))
                                    .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        public IEnumerable<ContainerIdent> GetContainerIdents(IEnumerable<int> ids)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.ContainerIdents
                                    .Where(c => ids.Contains(c.Id))
                                    .ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                // Rethrow
                throw;
            }
        }



            #endregion
        }
}
