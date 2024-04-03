using System;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using System.Collections.Generic;
using Infrastructure.RFID.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class RFIDController : ApiController
    {
        public RFIDController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// POST: api/RFID 
        /// </summary>
        /// <returns>List of RFID records</returns>
        [ResponseType(typeof(List<RFIDResponse>))]
        public IHttpActionResult POSTRFIDResponse(List<string> uidList)
        {
            Log.Debug("Invoked");

            if (uidList.Count == 0)
            {
                return BadRequest("No uids passed");
            }

            try
            {
                // Return response
                return Ok(UnitOfWork.MaterialManager.GetRFIDResponseBodies(uidList));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}