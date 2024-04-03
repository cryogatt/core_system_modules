using CommonEF;
using CommonEF.Services;
using Cryogatt.RFID.Trace;
using Infrastructure.Material.DTOs;
using Infrastructure.Material.Entities;
using Infrastructure.Material.Services;
using Infrastructure.RFID.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaterialDAL
{
    public class MaterialRepository : Repository, IMaterialRepository
    {
        #region Constructors

        public MaterialRepository(IContextFactory contextFactory) : base(contextFactory)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Get all batches.
        /// </summary>
        /// <returns></returns>
        public List<BatchInfoResponseBody> GetAllBatches()
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {

                    return (from b in context.BatchInfos
                            select new BatchInfoResponseBody
                            {
                                Uid = b.Id,
                                Name = b.Name,
                                Date = b.Date,
                                Material = (from f in context.AttributeFields
                                            select new MaterialInfoResponseBody
                                            {
                                                AttributeFieldName = f.Name,
                                                AttributeValueName = (from val in context.AttributeValues
                                                                      where val.AttributeFieldId.Equals(f.Id) && val.BatchInfoId.Equals(b.Id)
                                                                      select val.Value).FirstOrDefault()
                                            }),
                                Notes = b.Notes,
                                CryoSeedQty = b.CryoSeedQty,
                                SDSeedQty = b.SDSeedQty,
                                TestedSeedQty = b.TestedSeedQty
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get single batch.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public BatchInfoResponseBody GetBatchInfo(int uid)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return (from b in context.BatchInfos
                            where b.Id == uid
                            select new BatchInfoResponseBody
                            {
                                Uid = b.Id,
                                Name = b.Name,
                                Date = b.Date,
                                Material = (from f in context.AttributeFields
                                            select new MaterialInfoResponseBody
                                            {
                                                AttributeFieldName = f.Name,
                                                AttributeValueName = (from val in context.AttributeValues
                                                                      where val.AttributeFieldId.Equals(f.Id) && val.BatchInfoId.Equals(b.Id)
                                                                      select val.Value).FirstOrDefault()
                                            }),
                                Notes = b.Notes,
                                CryoSeedQty = b.CryoSeedQty,
                                SDSeedQty = b.SDSeedQty,
                                TestedSeedQty = b.TestedSeedQty
                            }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        public Batch GetBatch(int id, string batchType)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Batches
                        .Where(batch => batch.BatchInfoId == id)
                        .Where(batch => batch.BatchType.Name == batchType)
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get a batch by unique name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BatchInfoResponseBody GetBatchInfo(string name)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return (from b in context.BatchInfos
                            where b.Name == name
                            select new BatchInfoResponseBody
                            {
                                Uid = b.Id,
                                Name = b.Name,
                                Date = b.Date,
                                Material = (from f in context.AttributeFields
                                            select new MaterialInfoResponseBody
                                            {
                                                AttributeFieldName = f.Name,
                                                AttributeValueName = (from val in context.AttributeValues
                                                                      where val.AttributeFieldId.Equals(f.Id) && val.BatchInfoId.Equals(b.Id)
                                                                      select val.Value).FirstOrDefault()
                                            }),
                                Notes = b.Notes,
                                CryoSeedQty = b.CryoSeedQty,
                                SDSeedQty = b.SDSeedQty,
                                TestedSeedQty = b.TestedSeedQty
                            }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get a field record from its name.
        /// </summary>
        /// <param name="attributeFieldName"></param>
        /// <returns></returns>
        public AttributeField GetField(string attributeFieldName)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.AttributeFields
                        .Where(f => f.Name == attributeFieldName)
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get value belonging to a field in a batch.
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public AttributeValue GetAttributeValue(int batchId, int fieldId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.AttributeValues
                    .Where(v =>
                     v.BatchInfoId == batchId &&
                     v.AttributeFieldId == fieldId)
                    .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get the fields for material 
        ///     - TODO update for use of material of different types/data 
        /// </summary>
        /// <returns></returns>
        public List<MaterialInfoResponseBody> GetMaterialInfoFields()
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.AttributeFields
                    .Select(f =>
                    new MaterialInfoResponseBody
                    {
                        AttributeFieldName = f.Name
                    })
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get an aliquot from its containerUid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Aliquot IMaterialRepository.GetAliquot(string containerUid)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Aliquots
                    .Where(a => a.Container.Uid == containerUid)
                    .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get all aliquots belongibng to batch.
        /// </summary>
        /// <param name="batchInfoId"></param>
        /// <returns></returns>
        public List<AliquotResponseBody> GetBatchInfoAliquots(int batchInfoId, string batchType)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    var resp = (from a in context.Aliquots
                                where a.Batch.BatchInfoId == batchInfoId
                                where a.Batch.BatchType.Name == batchType
                                select new AliquotResponseBody
                                {
                                    Uid = a.Container.Uid,

                                    BatchName = a.Batch.BatchInfo.Name,

                                    Position = a.Container.StorageIndex,

                                    PrimaryDescription = a.Container.Description,

                                    ParentDescription = (from par in context.Containers
                                                         where par.Id == (a.Container.ParentContainerStorageId)
                                                         select par.Description).FirstOrDefault(),

                                    GrandParentDescription = (from par in context.Containers
                                                              where par.Id == (a.Container.ParentContainerStorageId)
                                                              from gpar in context.Containers
                                                              where gpar.Id == (par.ParentContainerStorageId)
                                                              select gpar.Description).FirstOrDefault(),

                                    GreatGrandParentDescription = (from par in context.Containers
                                                                   where par.Id == (a.Container.ParentContainerStorageId)
                                                                   from gpar in context.Containers
                                                                   where gpar.Id == (par.ParentContainerStorageId)
                                                                   from ggpar in context.Containers
                                                                   where ggpar.Id == (gpar.ParentContainerStorageId)
                                                                   select ggpar.Description).FirstOrDefault(),

                                    Site = (from par in context.Containers
                                            where par.Id == (a.Container.ParentContainerStorageId)
                                            from gpar in context.Containers
                                            where gpar.Id == (par.ParentContainerStorageId)
                                            from ggpar in context.Containers
                                            where ggpar.Id == (gpar.ParentContainerStorageId)
                                            from site in context.Containers
                                            where site.Id == (ggpar.ParentContainerStorageId)
                                            select site.Description).FirstOrDefault(),

                                    Status = context.ContainerStatuses
                                            .Where(s => s.ContainerUid == a.Container.Uid)
                                            .FirstOrDefault() != null ?
                                            context.ContainerStatuses.Where(s => s.ContainerUid == a.Container.Uid).FirstOrDefault().Status
                                            : "NOT SET"
                                }).ToList();

                    // Work around for outer apply not supported by MySQL
                    foreach (AliquotResponseBody aliq in resp)
                    {
                        aliq.Material = (from f in context.AttributeFields
                                         select new MaterialInfoResponseBody
                                         {
                                             AttributeFieldName = f.Name,
                                             AttributeValueName = (from val in context.AttributeValues
                                                                   from a in context.Aliquots
                                                                   where a.Container.Uid == aliq.Uid
                                                                   where val.AttributeFieldId.Equals(f.Id) && val.BatchInfoId.Equals(a.Batch.BatchInfoId)
                                                                   select val.Value).FirstOrDefault()
                                         }).ToList();
                    }
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        public List<Aliquot> GetBatchAliquots(int batchId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Aliquots
                        .Where(aliquot => aliquot.BatchId == batchId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get aliquots on pick list.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<AliquotResponseBody> GetAliquotsOnUserPickList(int userId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    var resp = (from p in context.PickLists
                                where p.UserId == userId
                                from pli in context.PickListItems
                                where pli.PickListId == p.Id
                                from cont in context.Containers
                                where cont.Id == pli.ContainerId
                                from a in context.Aliquots
                                where a.ContainerId == (cont.Id)
                                select new AliquotResponseBody
                                {
                                    Uid = a.Container.Uid,

                                    BatchName = a.Batch.BatchInfo.Name,

                                    Position = a.Container.StorageIndex,

                                    PrimaryDescription = a.Container.Description,

                                    ParentDescription = (from par in context.Containers
                                                         where par.Id == (a.Container.ParentContainerStorageId)
                                                         select par.Description).FirstOrDefault(),

                                    GrandParentDescription = (from par in context.Containers
                                                              where par.Id == (a.Container.ParentContainerStorageId)
                                                              from gpar in context.Containers
                                                              where gpar.Id == (par.ParentContainerStorageId)
                                                              select gpar.Description).FirstOrDefault(),

                                    GreatGrandParentDescription = (from par in context.Containers
                                                                   where par.Id == (a.Container.ParentContainerStorageId)
                                                                   from gpar in context.Containers
                                                                   where gpar.Id == (par.ParentContainerStorageId)
                                                                   from ggpar in context.Containers
                                                                   where ggpar.Id == (gpar.ParentContainerStorageId)
                                                                   select ggpar.Description).FirstOrDefault(),

                                    Site = (from par in context.Containers
                                            where par.Id == (a.Container.ParentContainerStorageId)
                                            from gpar in context.Containers
                                            where gpar.Id == (par.ParentContainerStorageId)
                                            from ggpar in context.Containers
                                            where ggpar.Id == (gpar.ParentContainerStorageId)
                                            from site in context.Containers
                                            where site.Id == (ggpar.ParentContainerStorageId)
                                            select site.Description).FirstOrDefault(),

                                    Status = context.ContainerStatuses
                                            .Where(s => s.ContainerUid == a.Container.Uid)
                                            .FirstOrDefault() != null ?
                                            context.ContainerStatuses.Where(s => s.ContainerUid == a.Container.Uid).FirstOrDefault().Status
                                            : "NOT SET"
                                }).ToList();

                    // Work around for OUTER APPLY not supported by MySQL
                    foreach (AliquotResponseBody aliq in resp)
                    {
                        aliq.Material = (from f in context.AttributeFields
                                         select new MaterialInfoResponseBody
                                         {
                                             AttributeFieldName = f.Name,
                                             AttributeValueName = (from val in context.AttributeValues
                                                                   from a in context.Aliquots
                                                                   where a.Container.Uid == aliq.Uid
                                                                   where val.AttributeFieldId.Equals(f.Id) && val.BatchInfoId.Equals(a.Batch.BatchInfoId)
                                                                   select val.Value).FirstOrDefault()
                                         }).ToList();
                    }

                    return resp;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get all aliquot records stored within a range of dates.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<AliquotResponseBody> GetAliquotsByDate(DateTime startDate, DateTime endDate)
        {
            Log.Debug("Invoked");

            List<AliquotResponseBody> resp;

            try
            {
                using (var db = ContextFactory.Create())
                {
                    resp = (from a in db.Aliquots
                            where a.Batch.BatchInfo.Date >= startDate.Date && a.Batch.BatchInfo.Date <= endDate
                            select new AliquotResponseBody
                            {
                                Uid = a.Container.Uid,

                                BatchName = a.Batch.BatchInfo.Name,

                                Position = a.Container.StorageIndex,

                                PrimaryDescription = a.Container.Description,

                                ParentDescription = (from par in db.Containers
                                                     where par.Id == (a.Container.ParentContainerStorageId)
                                                     select par.Description).FirstOrDefault(),

                                GrandParentDescription = (from par in db.Containers
                                                          where par.Id == (a.Container.ParentContainerStorageId)
                                                          from gpar in db.Containers
                                                          where gpar.Id == (par.ParentContainerStorageId)
                                                          select gpar.Description).FirstOrDefault(),

                                GreatGrandParentDescription = (from par in db.Containers
                                                               where par.Id == (a.Container.ParentContainerStorageId)
                                                               from gpar in db.Containers
                                                               where gpar.Id == (par.ParentContainerStorageId)
                                                               from ggpar in db.Containers
                                                               where ggpar.Id == (gpar.ParentContainerStorageId)
                                                               select ggpar.Description).FirstOrDefault(),

                                Site = (from par in db.Containers
                                        where par.Id == (a.Container.ParentContainerStorageId)
                                        from gpar in db.Containers
                                        where gpar.Id == (par.ParentContainerStorageId)
                                        from ggpar in db.Containers
                                        where ggpar.Id == (gpar.ParentContainerStorageId)
                                        from site in db.Containers
                                        where site.Id == (ggpar.ParentContainerStorageId)
                                        select site.Description).FirstOrDefault(),

                                Status = db.ContainerStatuses
                                            .Where(s => s.ContainerUid == a.Container.Uid)
                                            .FirstOrDefault() != null ?
                                            db.ContainerStatuses.Where(s => s.ContainerUid == a.Container.Uid).FirstOrDefault().Status
                                            : "NOT SET"
                            }).ToList();

                    foreach (AliquotResponseBody aliq in resp)
                    {
                        aliq.Material = (from a in db.Aliquots
                                         where a.Container.Uid == aliq.Uid
                                         from v in db.AttributeValues
                                         where v.BatchInfoId.Equals(a.Batch.BatchInfoId)
                                         from f in db.AttributeFields
                                         where f.Id.Equals(v.AttributeFieldId)
                                         select new MaterialInfoResponseBody
                                         {
                                             AttributeValueName = v.Value,
                                             AttributeFieldName = f.Name,
                                         }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
            return resp;
        }

        /// <summary>
        ///     Get all samples belonging to a site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        /// 

        /// <summary>
        ///     Get all aliquot records stored within a range of dates.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        ///  <param name="crop"></param>
        /// <returns></returns>
       
        public List<AliquotResponseBody> GetAllSiteSamples(int siteId)
        {
            Log.Debug("Invoked");

            List<AliquotResponseBody> resp;

            try
            {
                using (var db = ContextFactory.Create())
                {
                    resp = (// Site
                            from s in db.Sites
                            where s.Id == siteId
                            // Site Container
                            from c in db.Containers
                            where c.Id == s.ContainerId
                            //Dewar
                            from ggpar in db.Containers
                            where ggpar.ParentContainerStorageId == (c.Id)
                            // Rack
                            from gpar in db.Containers
                            where gpar.ParentContainerStorageId == (ggpar.Id)
                            // Box
                            from par in db.Containers
                            where par.ParentContainerStorageId == (gpar.Id)
                            // Primary Container
                            from pri in db.Containers
                            where pri.ParentContainerStorageId == (par.Id)
                            // Aliquot
                            from aliq in db.Aliquots
                            where aliq.ContainerId == (pri.Id)
                            select new AliquotResponseBody
                            {
                                BatchName = (from b in db.Batches where b.Id == (aliq.BatchId) select b.BatchInfo.Name).FirstOrDefault(),
                                Uid = pri.Uid,
                                GreatGrandParentDescription = ggpar.Description,
                                GrandParentDescription = gpar.Description,
                                ParentDescription = par.Description,
                                PrimaryDescription = pri.Description,
                                Site = c.Description,
                                Status = db.ContainerStatuses
                                            .Where(s => s.ContainerUid == pri.Uid)
                                            .FirstOrDefault() != null ?
                                            db.ContainerStatuses.Where(s => s.ContainerUid == pri.Uid).FirstOrDefault().Status
                                            : "NOT SET"
                            }).ToList();

                    // Work around for outer apply not supported by MySQL
                    foreach (AliquotResponseBody aliq in resp)
                    {
                        aliq.Material = (from f in db.AttributeFields
                                         select new MaterialInfoResponseBody
                                         {
                                             AttributeFieldName = f.Name,
                                             AttributeValueName = (from val in db.AttributeValues
                                                                   from a in db.Aliquots
                                                                   where a.Container.Uid == aliq.Uid
                                                                   where val.AttributeFieldId.Equals(f.Id) && val.BatchInfoId.Equals(a.Batch.BatchInfoId)
                                                                   select val.Value).FirstOrDefault()
                                         }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
            return resp;
        }

        /// <summary>
        ///     Get all samples belonging to a shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public List<AliquotResponseBody> GetAllShipmentSamples(int shipmentId)
        {
            Log.Debug("Invoked");

            List<AliquotResponseBody> resp;

            try
            {
                using (var db = ContextFactory.Create())
                {
                    resp = (// ShipmentConsignmentNo
                            from s in db.Shipments
                            where s.Id == (shipmentId)
                            // Contents
                            from content in db.Contents
                            where content.ShipmentId == (s.Id)
                            // Aliquots
                            from a in db.Aliquots
                            where a.ContainerId == (content.ContainerId)
                            select new AliquotResponseBody
                            {
                                Uid = a.Container.Uid,

                                BatchName = a.Batch.BatchInfo.Name,

                                PrimaryDescription = a.Container.Description,

                                ParentDescription = (from par in db.Containers
                                                     where par.Id == (a.Container.ParentContainerStorageId)
                                                     select par.Description).FirstOrDefault(),

                                GrandParentDescription = (from par in db.Containers
                                                          where par.Id == (a.Container.ParentContainerStorageId)
                                                          from gpar in db.Containers
                                                          where gpar.Id == (par.ParentContainerStorageId)
                                                          select gpar.Description).FirstOrDefault(),

                                GreatGrandParentDescription = (from par in db.Containers
                                                               where par.Id == (a.Container.ParentContainerStorageId)
                                                               from gpar in db.Containers
                                                               where gpar.Id == (par.ParentContainerStorageId)
                                                               from ggpar in db.Containers
                                                               where ggpar.Id == (gpar.ParentContainerStorageId)
                                                               select ggpar.Description).FirstOrDefault(),

                                Site = (from par in db.Containers
                                        where par.Id == (a.Container.ParentContainerStorageId)
                                        from gpar in db.Containers
                                        where gpar.Id == (par.ParentContainerStorageId)
                                        from ggpar in db.Containers
                                        where ggpar.Id == (gpar.ParentContainerStorageId)
                                        from site in db.Containers
                                        where site.Id == (ggpar.ParentContainerStorageId)
                                        select site.Description).FirstOrDefault(),

                                Status = db.ContainerStatuses
                                            .Where(s => s.ContainerUid == a.Container.Uid)
                                            .FirstOrDefault() != null ?
                                            db.ContainerStatuses.Where(s => s.ContainerUid == a.Container.Uid).FirstOrDefault().Status
                                            : "NOT SET"
                            }).ToList();

                    foreach (AliquotResponseBody aliq in resp)
                    {
                        aliq.Material = (from a in db.Aliquots
                                         where a.Container.Uid == aliq.Uid
                                         from v in db.AttributeValues
                                         where v.BatchInfoId.Equals(a.Batch.BatchInfoId)
                                         from f in db.AttributeFields
                                         where f.Id.Equals(v.AttributeFieldId)
                                         select new MaterialInfoResponseBody
                                         {
                                             AttributeValueName = v.Value,
                                             AttributeFieldName = f.Name,
                                         }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
            return resp;
        }

        /// <summary>
        ///     Get primary sample record.
        /// </summary>
        /// <param name="uid">The uid of the sample record</param>
        /// <returns></returns>
        public PrimaryContainersResponseBody GetPrimaryContainer(string uid)
        {
            Log.Debug("Invoked");

            PrimaryContainersResponseBody resp;

            try
            {
                using (var context = ContextFactory.Create())
                {
                    resp = (from cont in context.Containers
                            where cont.Uid == uid
                            from aliq in context.Aliquots
                            where aliq.ContainerId == cont.Id
                            select new PrimaryContainersResponseBody
                            {
                                Uid = cont.Uid,
                                Description = cont.Description,
                                Position = cont.StorageIndex,
                                InceptDate = cont.InceptDate.Value,
                                TagIdent = cont.ContainerIdent.TagIdent,
                                BatchId = (from a in context.Aliquots where a.ContainerId.Equals(cont.Id) select a.BatchId).FirstOrDefault(),
                                BatchName = (from a in context.Aliquots where a.ContainerId.Equals(cont.Id) select a.Batch.BatchInfo.Name).FirstOrDefault(),
                            }).FirstOrDefault();

                    // Work around for outer apply not supported by MySQL
                    resp.Material = (from f in context.AttributeFields
                                     select new MaterialInfoResponseBody
                                     {
                                         AttributeFieldName = f.Name,
                                         AttributeValueName = (from val in context.AttributeValues
                                                               from a in context.Aliquots
                                                               where a.Container.Uid.Equals(uid)
                                                               where val.AttributeFieldId.Equals(f.Id) && val.BatchInfoId.Equals(a.Batch.BatchInfoId)
                                                               select val.Value).FirstOrDefault()
                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
            return resp;
        }

        /// <summary>
        ///     Get the samples that belong to a particular parent
        /// </summary>
        /// <param name="parentUid">The Id of the parent that the samples belongs to</param>
        /// <returns></returns>
        public List<PrimaryContainersResponseBody> GetStoredPrimaryContainers(string parentUid)
        {
            Log.Debug("Invoked");

            List<PrimaryContainersResponseBody> resp;

            try
            {
                using (var context = ContextFactory.Create())
                {
                    resp = (from c in context.Containers
                            where c.Uid == parentUid
                            from cont in context.Containers
                            where cont.ParentContainerStorageId == c.Id
                            from aliq in context.Aliquots
                            where aliq.ContainerId == cont.Id
                            select new PrimaryContainersResponseBody
                            {
                                Uid = cont.Uid,
                                Description = cont.Description,
                                Position = cont.StorageIndex,
                                InceptDate = cont.InceptDate.Value,
                                TagIdent = cont.ContainerIdent.TagIdent,
                                BatchId = (from a in context.Aliquots where a.ContainerId.Equals(cont.Id) select a.BatchId).FirstOrDefault(),
                                BatchName = (from a in context.Aliquots where a.ContainerId.Equals(cont.Id) select a.Batch.BatchInfo.Name).FirstOrDefault(),
                            }).ToList();

                    // Work around for outer apply not supported by MySQL
                    foreach (PrimaryContainersResponseBody container in resp)
                    {
                        container.Material = (from f in context.AttributeFields
                                              select new MaterialInfoResponseBody
                                              {
                                                  AttributeFieldName = f.Name,
                                                  AttributeValueName = (from val in context.AttributeValues
                                                                        from a in context.Aliquots
                                                                        where a.Container.Uid.Equals(container.Uid)
                                                                        where val.AttributeFieldId.Equals(f.Id) && val.BatchInfoId.Equals(a.Batch.BatchInfoId)
                                                                        select val.Value).FirstOrDefault()
                                              }).ToList();
                    }
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get the rfid response for the given uids.
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        public List<RFIDResponseBody> GetRFIDResponseBodies(List<string> uids)
        {
            Log.Debug("Invoked");

            List<RFIDResponseBody> resp = new List<RFIDResponseBody>();

            try
            {
                using (var db = ContextFactory.Create())
                {
                    foreach (string uid in uids)
                    {
                        // Set the container properties
                        resp.Add((from cont in db.Containers
                                  where cont.Uid.Equals(uid)
                                  select new RFIDResponseBody
                                  {
                                      Uid = cont.Uid,
                                      BatchName = (from a in db.Aliquots
                                                   where a.ContainerId.Equals(cont.Id)
                                                   select a.Batch.BatchInfo.Name).FirstOrDefault(),
                                      Position = cont.StorageIndex,
                                      Description = cont.Description,
                                      ContainerType = (from c in db.ContainerIdents
                                                       where c.Id.Equals(cont.ContainerIdentId)
                                                       select c.ContainerType.Description).FirstOrDefault(),
                                  }).FirstOrDefault());
                        // Set the parent container properties
                        foreach (RFIDResponseBody container in resp)
                        {
                            if (container != null)
                            {
                                container.ParentUidDescription = (from c in db.Containers
                                                                  where c.Uid == container.Uid
                                                                  from par in db.Containers
                                                                  where par.Id == (c.ParentContainerStorageId)
                                                                  select par).ToDictionary(c => c.Uid, c => c.Description);
                            }
                        }
                    }
                }     
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
            return resp;
        }

        /// <summary>
        ///     Get a field/header record.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public AttributeField GetAttributeField(string field)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.AttributeFields
                    .Where(a => a.Name == field)
                    .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        public IEnumerable<BatchType> GetBatchTypes()
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.BatchTypes.ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }
        #endregion
    }
}
