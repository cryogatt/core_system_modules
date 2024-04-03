using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using Infrastructure.Material.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class AliquotsController : ApiController
    {
        public AliquotsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        // GET: api/v1/Aliquot/uid (get aliquots of a given batch where uid is the primary key)
        [ResponseType(typeof(AliquotResponse))]
        public IHttpActionResult GetAliquots(int uid)
        {
            Log.Debug("Invoked");

            if (uid == 0)
            {
                Log.Error("No given batch Id");
                return BadRequest("No given batch Id");
            }

            try
            {
                var batchType = Request.GetQueryNameValuePairs()
                    .Where(param => param.Key.ToUpper().Trim() == "BATCH_TYPE")
                    .Select(param => param.Value)
                    .Single();

                // Get response
                var resp = UnitOfWork.MaterialManager.GetBatchAliquots(uid, batchType);

                if (resp == null)
                    return BadRequest("Batch has no contents Id: " + uid);

                // Return the result
                return Ok(resp);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.ToString());
                return BadRequest("No given batch type");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError(ex);
            }
        }
    }
}
