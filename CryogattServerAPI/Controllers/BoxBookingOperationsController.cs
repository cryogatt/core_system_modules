using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;
using Infrastructure.Container.Entities;
using Infrastructure.RFID.DTOs;
using StorageOperations;
using Infrastructure.Material.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class BoxBookingOperationsController : ApiController
    {
        public BoxBookingOperationsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        [ResponseType(typeof(List<Container>))]
        public IHttpActionResult PutBookingOperations(List<RFIDResponseBody> storageItems)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValidField("Uid"))
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            // Determine Storage, Withdrawal, or Movement
            bool store = false, withdraw = false, movement = false;

            foreach (var param in Request.GetQueryNameValuePairs())
            {
                if (param.Key.ToUpper().Trim() == "STORE")
                {
                    store = true;
                }
                if (param.Key.ToUpper().Trim() == "WITHDRAW")
                {
                    withdraw = true;
                }
                if (param.Key.ToUpper().Trim() == "MOVEMENT")
                {
                    movement = true;
                }
            }

            try
            {
                if (storageItems.Count == 0)
                    return BadRequest("No uids passed");

                // Get the current user id
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                // Get group location
                Container location = UnitOfWork.ContainerManager.GetUsersSiteContainer(user.Username); // TODO Amend for user of multiple sites

                var operation = store ? StorageOperation.STORE
                    : movement ? StorageOperation.MOVEMENT
                    : StorageOperation.WITHDRAW;

                UnitOfWork.StorageOperationsManager.SetContainersMovement(operation, storageItems, user.Id, location.Description);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        ///  Post a new primary container record
        /// </summary>
        /// <param name="uid">batch type</param>
        /// <param name="newSample"></param>
        /// The id of batch
        /// <returns></returns>
        [ResponseType(typeof(List<BatchInfoResponseBody>))]
        public IHttpActionResult PostPrimaryContainer(string uid, PrimaryContainersResponseBody newSample)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            if (newSample == null)
            {
                Log.Error("No container passed");
                return BadRequest("No containers passed");
            }
            // Check all parameters are valid
            if (!newSample.IsValid())
            {
                if (newSample.Description.IsNullOrWhiteSpace())
                    return BadRequest("Please enter a valid label description");

                return BadRequest("Incomplete model passed to server");
            }

            try
            {
                // if all uids are not already in the container database
                if (UnitOfWork.ContainerManager.GetContainer(newSample.Uid) != null)
                {
                    Log.Error("Container already in database!");
                    return BadRequest("Container already in database!");
                }

                var ident = UnitOfWork.ContainerManager.GetContainerIdentByTagIdent(newSample.TagIdent);

                if (ident == null)
                {
                    Log.Error("Container Ident in database!");
                    return BadRequest("Container Ident in database!");
                }

                if (UnitOfWork.MaterialManager.GetBatchInfo(newSample.BatchId) == null)
                {
                    Log.Error("Batch does not exist! batch_id passed: " + newSample.BatchId);
                    return BadRequest("Batch does not exist!");
                }

                // Get the current user id
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                // Get group location
                var location = UnitOfWork.ContainerManager.GetUsersSiteContainer(user.Username); // TODO Amend for user of multiple sitesntainer location = ContainerManagerService.ContainerManagerSingleton.GetContainer(u.Sites.First().ContainerId); // TODO Amend for user of multiple sites

                // Add to database
                var id = UnitOfWork.MaterialManager.AddAliquot(
                    newSample,
                    ident.Id,
                    user.Id,
                    location.Description,
                    UnitOfWork.ContainerManager,
                    UnitOfWork.HistoryManager,
                    uid);

                if (id == 0)
                    throw new Exception("Could not add sample to database");

                return Ok(UnitOfWork.MaterialManager.GetBatchInfo(newSample.BatchId));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}
