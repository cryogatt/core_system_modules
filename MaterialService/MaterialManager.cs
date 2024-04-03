using Infrastructure.Container.Entities;
using Infrastructure.Container.Services;
using Infrastructure.History.Services;
using Infrastructure.Material.DTOs;
using Infrastructure.Material.Entities;
using Infrastructure.Material.Exceptions;
using Infrastructure.Material.Services;
using Infrastructure.RFID.DTOs;
using Infrastructure.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaterialService
{
    public class MaterialManager : IMaterialManager
    {
        #region Constructors

        public MaterialManager(IMaterialRepository repository, IUserManager userManager)
        {
            Repository = repository;
            UserManager = userManager;
        }

        #endregion

        /// <summary>
        ///     Access to data.
        /// </summary>
        private readonly IMaterialRepository Repository;

        /// <summary>
        ///     Access to user data.
        /// </summary>
        private readonly IUserManager UserManager;

        /// <summary>
        ///     Add batch record including its associated values.
        /// </summary>
        /// <param name="batchesResponseBody"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        public int AddBatch(BatchInfoResponseBody batchesResponseBody, int groupId)
        {
            if (GetBatchInfo(batchesResponseBody.Name) != null)
                throw new DuplicateBatchException("Name is not unique!");

            if (batchesResponseBody.Material.Count() == 0)
                throw new Exception("No fields/data provided for batch!");

            var batchTypes = Repository.GetBatchTypes();

            if (batchTypes == null)
                throw new Exception("Batch Type not found!");
                       
            var batchInfo = Repository.Add(
                new BatchInfo(
                    0, 
                    batchesResponseBody.Name, 
                    batchesResponseBody.Notes, 
                    groupId, 
                    batchesResponseBody.CryoSeedQty,
                    batchesResponseBody.TestedSeedQty,
                    batchesResponseBody.SDSeedQty,
                    DateTime.Now));

            // Stage batches
            var batches = batchTypes.Select(type => 
                Repository.Add(new Batch(
                    0,
                    type.Id,
                    batchInfo.Id)))
                .ToList();

            if (batches.Any(batch => batch.Id == 0))
               throw new Exception("Could not create batch:" + batchesResponseBody.Name);

            // Add the values of the batch
            AddBatchAttributeValues(batchesResponseBody, batchInfo.Id);

            return batchInfo.Id;
        }

        /// <summary>
        ///     Add batch to database.
        /// </summary>
        /// <param name="newBatch"></param>
        /// <returns></returns>
        public int AddBatch(BatchInfo newBatch)
        {
            // Stage 
            var batchInfo = Repository.Add(newBatch);

            var batchTypes = Repository.GetBatchTypes();

            // Stage batches
            var batches = batchTypes.Select(type =>
                Repository.Add(new Batch(
                    0,
                    type.Id,
                    batchInfo.Id)))
                .ToList();

            if (batches.Any(batch => batch.Id == 0))
                throw new Exception("Could not create batch:" + batchInfo.Name);

            return batchInfo.Id;
        }

        /// <summary>
        ///     Get the response for all batches.
        /// </summary>
        /// <returns></returns>
        public BatchInfoResponse GetAllBatches()
        {
            var batches = Repository.GetAllBatches();

            return new BatchInfoResponse(
                batches.Count,
                batches);
        }

        /// <summary>
        ///     Get a single batch by its primary key.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public BatchInfoResponseBody GetBatchInfo(int uid)
        {
            return Repository.GetBatchInfo(uid);
        }

        /// <summary>
        ///     Get batch by unique name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BatchInfoResponseBody GetBatchInfo(string name)
        {
            return Repository.GetBatchInfo(name);
        }
                
        /// <summary>
        ///     Updatebatch record.
        /// </summary>
        /// <param name="batchesResponseBody"></param>
        public void UpdateBatch(BatchInfoResponseBody batchesResponseBody)
        {
            var batch = Repository.Get<BatchInfo>(batchesResponseBody.Uid);

            if (batch == null)
                throw new Exception("Batch not in database:" + batchesResponseBody.Name);

            // Update the values belonging to the batch
            UpdateAttributeValues(batchesResponseBody, batch);

            // Have the notes been updated
            if (batch.Notes != batchesResponseBody.Notes)
            {
                batch.Update(batchesResponseBody.Notes);
                Repository.Update(batch);
            }
        }

        /// <summary>
        ///      Add all containers of the same batch to pick list to a users pick list.
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="userId"></param>
        /// <param name="containerManager"></param>
        void IMaterialManager.AddContainersBelongingToSameBatchToPickList(List<string> uids, int userId, IContainerManager containerManager)
        {
            var aliquots = uids
                .Select(uid => Repository.GetAliquot(uid));

            // Ensure all are in database
            if (aliquots.Any(a => a == null))
                throw new Exception("List of uids passed invalid");

            // Gather all batch ids
            var batchIds = aliquots
                .Select(a => a.BatchId);

            // Add all batch contents to pick list
            AddBatchesToPickList(userId, containerManager, batchIds);
        }

        /// <summary>
        ///      Add all containers belonging to the given list of batches.
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="userId"></param>
        /// <param name="containerManager"></param>
        void IMaterialManager.AddAllBatchContainersToPickList(List<int> batchIds, int userId, IContainerManager containerManager)
        {
            var batches = batchIds
                .Select(id => Repository.Get<Batch>(id));

            // Ensure all batches are in database
            if (batches.Any(b => b == null))
                throw new Exception("List of batch id passed invalid");
            
            // Add all batch contents to pick list
            AddBatchesToPickList(userId, containerManager, batchIds);
        }

        /// <summary>
        ///     Get the primary container response from the tag uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        PrimaryContainersResponseBody IMaterialManager.GetPrimaryContainer(string uid)
        {
            return Repository.GetPrimaryContainer(uid);
        }

        /// <summary>
        ///     Get all primary containers stored in a given parent.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public PrimaryContainersResponse GetStoredPrimaryContainers(string parentUid)
        {
            var primaryContainers = Repository.GetStoredPrimaryContainers(parentUid);

            if (primaryContainers == null)
                return null;

            return new PrimaryContainersResponse(
                primaryContainers.Count(),
                primaryContainers);
        }

        /// <summary>
        ///     Get the response required for processing the rfid tags.
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        public RFIDResponse GetRFIDResponseBodies(List<string> uids)
        {
            var responseBodies = Repository.GetRFIDResponseBodies(uids);

            return new RFIDResponse(
                responseBodies.Count(),
                responseBodies);
        }

        /// <summary>
        ///     Add a new sample to the database.
        /// </summary>
        /// <param name="newSample"></param>
        /// <param name="containerIdentId"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        /// <param name="containerManager"></param>
        /// <param name="historyManager"></param>
        public int AddAliquot(
            PrimaryContainersResponseBody newSample,
            int containerIdentId,
            int userId,
            string location,
            IContainerManager containerManager,
            IHistoryManager historyManager,
            string batchType)
        {
            var batchInfo = Repository.Get<BatchInfo>(newSample.BatchId);

            if (batchInfo == null)
                throw new ArgumentException("batchInfo id not in database: " + newSample.BatchId);

            var batch = Repository.GetBatch(batchInfo.Id, batchType);

            if (batch == null)
                throw new ArgumentException("batch id not in database");

            var container = CreateNewContainer(newSample, containerIdentId);
            
            // Stage container record
            Repository.Add(container);
            
            // Create aliquot record
            var aliquot = CreateNewAliquot(newSample, batch, container);

            // Stage
            Repository.Add(aliquot);

            // Audit
            historyManager.AddCheckpoint(
                container,
                "Sample added to database",
                location,
                userId);

            return aliquot.Id;
        }

        /// <summary>
        ///     Get all aliquot records stored within a range of dates.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public AliquotResponse GetAliquotsByDate(DateTime startDate, DateTime endDate)
        {
            var responseBodies = Repository.GetAliquotsByDate(startDate, endDate);

            if (responseBodies == null)
                return null;

            return new AliquotResponse(
                responseBodies.Count(),
                responseBodies);
        }



        /// <summary>
        ///     Get a field/header record.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        /// 
        public AttributeField GetAttributeField(string field)
        {
            return Repository.GetAttributeField(field);
        }

        /// <summary>
        ///     Add a value to the database
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int AddAttributeValue(AttributeValue value)
        {
            if (value == null)
                throw new NullReferenceException();

            // Stage
            Repository.Add(value);

            return value.Id;
        }

        /// <summary>
        ///     Get the material fields (data schema) - 
        ///     TODO Update here for use of multiple material data sets. 
        /// </summary>
        /// <returns></returns>
        public MaterialInfoResponse GetMaterialFields()
        {
            var data = Repository.GetMaterialInfoFields();

            if (data == null)
                throw new Exception("No available data");

            return new MaterialInfoResponse(
                data.Count(),
                data);
        }

        /// <summary>
        ///     Get the aliquots belonging to the batch.
        /// </summary>
        /// <param name="batchInfoId"></param>
        /// <returns></returns>
        public AliquotResponse GetBatchAliquots(int batchInfoId, string batchType)
        {
            var batch = Repository.Get<BatchInfo>(batchInfoId);

            // Ensure batch exists
            if (batch == null)
                throw new Exception("Batch Info does not exist: Id " + batchInfoId);

            // Get all aliquots
            var aliquots = Repository.GetBatchInfoAliquots(batchInfoId, batchType);

            if (aliquots == null)
                throw new Exception("Could not retrieve aliquots for batch " + batchInfoId);

            // Assume batch has no contents
            if (aliquots.Count() == 0)
                return null;

            return new AliquotResponse(
                aliquots.Count(),
                aliquots);
        }

        /// <summary>
        ///     Get all samples belonging to a site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public AliquotResponse GetAllSiteSamples(int siteId)
        {
            var responseBodies = Repository.GetAllSiteSamples(siteId);

            return new AliquotResponse(
                responseBodies.Count(),
                responseBodies);
        }

        /// <summary>
        ///     Get all samples belonging to a shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public AliquotResponse GetAllShipmentSamples(int shipmentId)
        {
            var responseBodies = Repository.GetAllShipmentSamples(shipmentId);

            return new AliquotResponse(
                responseBodies.Count(),
                responseBodies);
        }

        /// <summary>
        ///     Get aliquots on pick list.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AliquotResponse GetAliquotsOnUserPickList(int userId)
        {
            var aliquots = Repository.GetAliquotsOnUserPickList(userId);

            if (aliquots == null)
                throw new Exception("Could not retrieve aliquots on pick list");

            return new AliquotResponse(
                aliquots.Count(),
                aliquots);
        }
        
        #region Private Methods

        /// <summary>
        ///     Add contents of all batches to pick list.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="containerManager"></param>
        /// <param name="batchIds"></param>
        private void AddBatchesToPickList(int userId, IContainerManager containerManager, IEnumerable<int> batchIds)
        {
            // Gather all aliquots belonging to batch
            var batches = batchIds.Select(Repository.GetBatchAliquots);

            // Gather all aliquot uids
            var allAliqoutUids = batches
                .SelectMany(b => b)
                .Select(a => a.ContainerId)
                .ToList();

            // Pass to container manager
            containerManager.AddContainersToPickList(allAliqoutUids, userId);
        }

        /// <summary>
        ///     Add the attribute values of a batch
        /// </summary>
        /// <param name="batchesResponseBody"></param>
        /// <param name="batch"></param>
        private void AddBatchAttributeValues(BatchInfoResponseBody batchesResponseBody, int batchInfoId)
        {
            // Add the value for each name 
            foreach (var field in batchesResponseBody.Material)
            {
                var header = Repository.GetField(field.AttributeFieldName);

                if (header == null)
                    throw new Exception("header is not db: " + field.AttributeFieldName);

                var value = new AttributeValue(
                    0,
                    field.AttributeValueName,
                    header.Id,
                    batchInfoId);
                // Add value to database
                Repository.Add(value);
            }
        }

        /// <summary>
        ///     Update the values belonging to a a batch.
        /// </summary>
        /// <param name="batchesResponseBody"></param>
        /// <param name="batch"></param>
        private void UpdateAttributeValues(BatchInfoResponseBody batchesResponseBody, BatchInfo batch)
        {
            // Access each value for each field 
            foreach (var edit in batchesResponseBody.Material)
            {
                var header = Repository.GetField(edit.AttributeFieldName);

                if (header == null)
                    throw new Exception("header is not db: " + edit.AttributeFieldName);

                // Determine which value belong to the batch 
                var attributeValue = Repository.GetAttributeValue(batchesResponseBody.Uid, header.Id);

                if (attributeValue != null)
                {
                    // Stage changes if editted
                    if (attributeValue.Value != edit.AttributeValueName)
                    {
                        // Update to the editted value
                        attributeValue.Update(edit.AttributeValueName);
                        Repository.Update(attributeValue);
                    }
                }
                else
                {
                    // Add if does not exist
                    var value = new AttributeValue(
                        0,
                        edit.AttributeValueName,
                        header.Id,
                        batch.Id);
                    // Add value to database
                    Repository.Add(value);
                }
            }
        }

        private static Aliquot CreateNewAliquot(PrimaryContainersResponseBody newSample, Batch batch, Container container)
        {

            // Create aliquot
            return new Aliquot(
                0,
                batch.Id,
                newSample.Description,
                container.Id);
        }

        private static Container CreateNewContainer(PrimaryContainersResponseBody newSample, int containerIdentId)
        {
            // Create container record
            return new Container(
                0,
                newSample.Uid,
                newSample.Description,
                containerIdentId,
                null,
                0,
                null,
                0,
                0,
                DateTime.Now);
        }

        #endregion
    }
}
