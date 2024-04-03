using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using Infrastructure.Material.DTOs;
using Infrastructure.Material.Exceptions;

namespace CryogattServerAPI.Controllers
{

    [Authorize]
    public class MaterialsController : ApiController
    {
        public MaterialsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        // GET: api/v1/Materials
        [ResponseType(typeof(BatchInfoResponse))]
        public IHttpActionResult GetMaterials()
        {
            Log.Debug("Invoked");

            try
            {
                // Return response
                return Ok(UnitOfWork.MaterialManager.GetAllBatches());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // GET: api/v1/materials/uid
        [ResponseType(typeof(BatchInfoResponseBody))]
        public IHttpActionResult GetMaterials(int uid)
        {
            Log.Debug("Invoked");

            try
            {
                // Get Response body for batch
                var result = UnitOfWork.MaterialManager.GetBatchInfo(uid);

                // Return error if batch not found
                if (result == null)
                   return NotFound();                

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // PUT: api/v1/Materials (edited Material record)
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMaterials(int uid, BatchInfoResponseBody edittedMaterial)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            try
            {
                // Prefrom update
                UnitOfWork.MaterialManager.UpdateBatch(edittedMaterial);
                
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST: api/v1/Materials (new material record)
        [ResponseType(typeof(BatchInfoResponseBody))]
        public IHttpActionResult PostMaterial(BatchInfoResponseBody newMaterial)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }
            try
            { 
                // Get the current user 
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);
            
                // Get user site record 
                var group = UnitOfWork.UserManager.GetGroup(user.Groups.FirstOrDefault()); // TODO - Fix for user of multiple groups

                if (group == null)
                    throw new Exception("Error user does not belong to a group!");

                // Add record to database
                var id = UnitOfWork.MaterialManager.AddBatch(newMaterial, group.Id);

                return Ok(UnitOfWork.MaterialManager.GetBatchInfo(id));
            }
            catch (DuplicateBatchException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}
