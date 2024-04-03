using System;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Description;
using System.Web.WebPages;
using CryogattServerAPI.Trace;
using Infrastructure.Material.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class PrimaryContainersController : ApiController
    {
        public PrimaryContainersController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        [ResponseType(typeof(PrimaryContainersResponse))]
        public IHttpActionResult GetPrimaryContainer()
        {
            string uid = "";

            foreach (var param in Request.GetQueryNameValuePairs())
            {
                if (param.Key.ToUpper().Trim() == "UIDS")
                {
                    uid = param.Value;
                }
            }

            if (uid.IsEmpty())
            {
                return BadRequest("No Uid passed!");
            }
            try
            {
                Log.Debug("Invoked");

                // Query the database for sample
                var resp = UnitOfWork.MaterialManager.GetPrimaryContainer(uid);

                // Return the result
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        /// GET: api/v1/PrimaryContainers/{uid} 
        /// Get all samples that belong to a given parent whoose uid is the argument
        [ResponseType(typeof(PrimaryContainersResponse))]
        public IHttpActionResult GetStoredPrimaryContainers(string uid)
        {
            Log.Debug("Invoked");

            try
            {
                // Check uid is valid
                if (UnitOfWork.ContainerManager.GetContainerResponseBody(uid) == null)
                    return BadRequest("Container not in database: " + uid);

                // Query the database for all samples in parent
                var resp = UnitOfWork.MaterialManager.GetStoredPrimaryContainers(uid);

                // Return the result
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}