using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using Infrastructure.Material.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class PicklistController : ApiController
    {
        private readonly IUnitOfWork UnitOfWork;

        public PicklistController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        // GET: api/v1/Picklist
        [ResponseType(typeof(AliquotResponse))]
        public IHttpActionResult GetPicklist()
        {
            Log.Debug("Invoked");

            try
            {
                // Get the current user id
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);
                
                var resp = UnitOfWork.MaterialManager.GetAliquotsOnUserPickList(user.Id);

                // Return response
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST: api/Picklist (new entry/entries into picklist)
        [ResponseType(typeof(void))]
        public  IHttpActionResult PostPicklist(List<string> picklistItems)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }
                        
            // Parameters are uids - add all of the samples belonging to this amples batch
            bool getRestofBatch = false;
            // Parameters are batch Ids - Add all of the samples beling to a batch Id.
            bool addBatches = false;

            // Parse parameters - Does the parameter contain the batch id or the uid of an aliquot belonging to batch?
            foreach (var param in Request.GetQueryNameValuePairs())
            {
                if (param.Key.ToUpper().Trim() == "UID")
                {
                    getRestofBatch = Convert.ToBoolean(param.Value);
                }
                if (param.Key.ToUpper().Trim() == "BATCH_ID")
                {
                    addBatches = Convert.ToBoolean(param.Value);
                }
            }

            try
            {
                // Get the current user 
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                if (!addBatches)
                {
                    if (getRestofBatch)
                        // Add all uids and all samples that belong to the smae batch to pick list
                        UnitOfWork.MaterialManager.AddContainersBelongingToSameBatchToPickList(picklistItems, user.Id, UnitOfWork.ContainerManager);

                    else
                        // Just add samples on list
                        UnitOfWork.ContainerManager.AddContainersToPickList(picklistItems, user.Id);
                }
                else
                {
                    // Cast all ids to ints - TODO thrown exception will be displayed as server side error but is actually client side.
                    var batchIds = picklistItems
                        .Select(id => Int32.Parse(id))
                        .ToList();

                    UnitOfWork.MaterialManager.AddAllBatchContainersToPickList(batchIds, user.Id, UnitOfWork.ContainerManager);
                }

                return Ok("Success!");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // DELETE: api/Picklist (delete extries from the picklist)
        [ResponseType(typeof(void))]
        public IHttpActionResult DeletePicklist(List<string> uids)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            try
            {
                UnitOfWork.ContainerManager.DeletePicklistEntries(uids);
                
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest("Failed to remove item(s) from the pick list.");
            }
        }
    }
}