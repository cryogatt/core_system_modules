using CommonEF;
using Infrastructure.History.Services;
using Infrastructure.History.Entities;
using System.Linq;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using CommonEF.Services;
using Cryogatt.RFID.Trace;
using History.Entities;

namespace HistoryDAL
{
    public class HistoryRepository : Repository, IHistoryRepository
    {
        #region Constructors

        public HistoryRepository(IContextFactory contextFactory) : base(contextFactory)
        { }

        #endregion

        /// <summary>
        ///     Inlcudes User navigation property.
        /// </summary>
        /// <param name="containerUid"></param>
        /// <returns></returns>
        public List<Point> GetContainersHistory(string containerUid)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Points
                    .Where(p => p.Uid == containerUid)
                    .Include(p => p.User)
                    .ToList();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }

        /// <summary>
        ///     Has the container been stored yet?
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public bool IsInceptDateSet(int containerId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Points
                    .Any(p => p.ContainerId == containerId
                    && p.InceptDate != null);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }

        public ContainerStatus GetContainerStatus(string uid)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.ContainerStatuses
                        .Where(s => s.ContainerUid == uid)
                        .SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }

        public void SetContainerStatus(ContainerStatus containerStatus)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    var existingRecord = context.ContainerStatuses
                        .Where(s => s.ContainerUid == containerStatus.ContainerUid)
                        .SingleOrDefault();

                    if (existingRecord == null)
                    {
                        context.Add(containerStatus);
                    }
                    else
                    {
                        existingRecord.Status = containerStatus.Status;
                        context.Update(existingRecord);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message); throw;
            }
        }
    }
}
