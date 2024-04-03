using Infrastructure.Container.Services;
using Infrastructure.History.Services;
using Infrastructure.Material.DTOs;
using Infrastructure.Material.Entities;
using Infrastructure.RFID.DTOs;
using System;
using System.Collections.Generic;

namespace Infrastructure.Material.Services
{
    public interface IMaterialManager
    {
        /// <summary>
        ///     Get the response containing all batches.
        /// </summary>
        /// <returns></returns>
        BatchInfoResponse GetAllBatches();

        /// <summary>
        ///     Get a single batch by primary key.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        BatchInfoResponseBody GetBatchInfo(int uid);

        /// <summary>
        ///     Get the aliquots belonging to batch.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AliquotResponse GetBatchAliquots(int id, string batchType);

        /// <summary>
        ///     Get a single batch by unique name.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        BatchInfoResponseBody GetBatchInfo(string name);

        /// <summary>
        ///     Update batch record.
        /// </summary>
        /// <param name="batchesResponseBody"></param>
        void UpdateBatch(BatchInfoResponseBody batchesResponseBody);

        /// <summary>
        ///     Get the material fields (data schema) - 
        ///     TODO Update here for use of multiple material data sets. 
        /// </summary>
        /// <returns></returns>
        MaterialInfoResponse GetMaterialFields();

        /// <summary>
        ///     Get all samples belonging to a site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        AliquotResponse GetAllSiteSamples(int siteId);

        /// <summary>
        ///     Get all samples belonging to a shipment.
        /// </summary>
        /// <param name="senderSiteId"></param>
        /// <returns></returns>
        AliquotResponse GetAllShipmentSamples(int shipmentId);
        
        /// <summary>
        ///     Get the aliquots on picklist.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AliquotResponse GetAliquotsOnUserPickList(int userId);

        /// <summary>
        ///     Get all aliquot records stored within a range of dates.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        AliquotResponse GetAliquotsByDate(DateTime startDate, DateTime endDate);

        /// <summary>
        ///     Get the primary container response from the tag uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        PrimaryContainersResponseBody GetPrimaryContainer(string uid);

        /// <summary>
        ///     Get all primary containers stored in a given container.
        /// </summary>
        /// <param name="parentUid"></param>
        /// <returns></returns>
        PrimaryContainersResponse GetStoredPrimaryContainers(string parentUid);

        /// <summary>
        ///      Add all containers of the same batch to pick list to a users pick list.
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="userId"></param>
        /// <param name="containerManager"></param>
        void AddContainersBelongingToSameBatchToPickList(List<string> uids, int userId, IContainerManager containerManager);

        /// <summary>
        ///      Add all containers belonging to the given list of batches.
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="userId"></param>
        /// <param name="containerManager"></param>
        void AddAllBatchContainersToPickList(List<int> batchIds, int userId, IContainerManager containerManager);

        /// <summary>
        ///     Add batch to database.
        /// </summary>
        /// <param name="batchesResponseBody"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        int AddBatch(BatchInfoResponseBody batchesResponseBody, int groupId);

        /// <summary>
        ///     Add batch to database.
        /// </summary>
        /// <param name="newBatch"></param>
        /// <returns></returns>
        int AddBatch(BatchInfo newBatch);

        /// <summary>
        ///     Get the rfid response for the given uids.
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        RFIDResponse GetRFIDResponseBodies(List<string> uids);

        /// <summary>
        ///     Add a new sample to the database.
        /// </summary>
        /// <param name="newSample"></param>
        /// <param name="containerIdentId"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        /// <param name="containerManager"></param>
        /// <param name="historyManager"></param>
        int AddAliquot(
            PrimaryContainersResponseBody newSample,
            int containerIdentId, 
            int userId,
            string location,
            IContainerManager containerManager,
            IHistoryManager historyManager,
            string batchType);

        /// <summary>
        ///     Get a field/header record.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        AttributeField GetAttributeField(string field);

        /// <summary>
        ///     Add a value to the database
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int AddAttributeValue(AttributeValue value);
    }
}
