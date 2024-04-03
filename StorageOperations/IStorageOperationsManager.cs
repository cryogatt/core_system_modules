using System.Collections.Generic;
using Infrastructure.RFID.DTOs;

namespace StorageOperations
{
    public interface IStorageOperationsManager
    {
        /// <summary>
        ///     Process & persist the containers movements.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="storageItems"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        void SetContainersMovement(StorageOperation operation, List<RFIDResponseBody> storageItems, int userId, string location);

        /// <summary>
        ///     Apply rules to the shipment and contents.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="shipmentId"></param>
        /// <param name="tagUids"></param>
        /// <param name="userId"></param>
        /// <param name="location"></param>
        void ProcessShipment(StorageOperation operation, int shipmentId, List<RFIDResponseBody> tagUids, int userId, string location);
    }
}