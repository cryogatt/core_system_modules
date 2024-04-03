using Common;
using Infrastructure.Material.DTOs;
using Infrastructure.Material.Entities;
using Infrastructure.RFID.DTOs;
using System;
using System.Collections.Generic;

namespace Infrastructure.Material.Services
{
    public interface IMaterialRepository : IRepository
    {
        /// <summary>
        ///     Get all batch records from database.
        /// </summary>
        /// <returns></returns>
        List<BatchInfoResponseBody> GetAllBatches();

        /// <summary>
        ///     Get an aliquot from its containerUid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Aliquot GetAliquot(string containerUid);

        /// <summary>
        ///     Get all aliquots belonging to batch.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<AliquotResponseBody> GetBatchInfoAliquots(int batchInfoId, string batchType);

        List<Aliquot> GetBatchAliquots(int batchId);

        /// <summary>
        ///     Get aliquots on pick list.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<AliquotResponseBody> GetAliquotsOnUserPickList(int userId);

        /// <summary>
        ///     Get a single batch by primary key.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        BatchInfoResponseBody GetBatchInfo(int uid);

        /// <summary>
        ///     Get a single batch by unique name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        BatchInfoResponseBody GetBatchInfo(string name);
        Batch GetBatch(int id, string batchType);

        IEnumerable<BatchType> GetBatchTypes();

        /// <summary>
        ///     Get a field record from its name.
        /// </summary>
        /// <param name="attributeFieldName"></param>
        /// <returns></returns>
        AttributeField GetField(string attributeFieldName);

        /// <summary>
        ///     Get value belonging to a field in a batch.
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        AttributeValue GetAttributeValue(int batchId, int fieldId);

        /// <summary>
        ///     Get the material fields - TODO update here for use of multiple data.
        /// </summary>
        /// <returns></returns>
        List<MaterialInfoResponseBody> GetMaterialInfoFields();

        /// <summary>
        ///     Get primary sample record.
        /// </summary>
        /// <param name="uid">The uid of the sample record</param>
        /// <returns></returns>
        PrimaryContainersResponseBody GetPrimaryContainer(string uid);

        /// <summary>
        ///     Get all primary containers stored in a parent conatiner.
        /// </summary>
        /// <param name="parentUid"></param>
        /// <returns></returns>
        List<PrimaryContainersResponseBody> GetStoredPrimaryContainers(string parentUid);

        /// <summary>
        ///     Get the rfid response for the given uids.
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        List<RFIDResponseBody> GetRFIDResponseBodies(List<string> uids);

        /// <summary>
        ///     Get all aliquot records stored within a range of dates.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<AliquotResponseBody> GetAliquotsByDate(DateTime startDate, DateTime endDate);

        /// <summary>
        ///     Get a field/header record.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        AttributeField GetAttributeField(string field);

        /// <summary>
        ///     Get all samples belonging to a site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        List<AliquotResponseBody> GetAllSiteSamples(int siteId);

        /// <summary>
        ///     Get all samples belonging to a shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        List<AliquotResponseBody> GetAllShipmentSamples(int shipmentId);
    }
}
