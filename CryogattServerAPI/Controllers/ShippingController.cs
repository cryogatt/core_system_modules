using CryogattServerAPI.Trace;
using Distribution.DTOs;
using Infrastructure.Distribution.Entities;
using Infrastructure.RFID.DTOs;
using StorageOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class ShippingController : ApiController
    {
        public ShippingController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;
        
        [ResponseType(typeof(void))]
        public IHttpActionResult PutShippingOperations(int uid, List<RFIDResponseBody> tagUids)
        {
            Log.Debug("Invoked");

            if (tagUids == null)
                return BadRequest("No items passed");

            if (!ModelState.IsValidField("Uid"))
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            // Determine direction
            StorageOperation operation = StorageOperation.NOTSET;

            foreach (var param in Request.GetQueryNameValuePairs())
            {
                if (param.Key.ToUpper().Trim() == "SEND")
                {
                    operation = StorageOperation.SEND;
                }
                if (param.Key.ToUpper().Trim() == "RECEIVE")
                {
                    operation = StorageOperation.RECEIVE;
                }
            }

            if (operation == StorageOperation.NOTSET)
            {
                Log.Error("Direction not specified");
                return BadRequest("Direction not specified");
            }

            try
            {
                // Get the current user id
                var user = UnitOfWork.UserManager.GetSingleActiveUser(RequestContext.Principal.Identity.Name);

                // Get group location
                var location = UnitOfWork.ContainerManager.GetUsersSiteContainer(user.Username); // TODO Amend for user of multiple sites

                UnitOfWork.StorageOperationsManager.ProcessShipment(operation, uid, tagUids, user.Id, location.Description);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }

            return Ok();
        }

        [ResponseType(typeof(List<Courier>))]
        public IHttpActionResult GetAllCouriers()
        {
            Log.Debug("Invoked");

            try
            {
                var result = UnitOfWork.DistributionManager.GetAllCouriers();

                // Return the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
        /// <summary>
        /// Get the courier record for the shipper uid
        /// </summary>
        /// <param name="uid">The shipper uid</param>
        /// <returns>CourierResponseBody</returns>
        [ResponseType(typeof(CourierResponseBody))]
        public IHttpActionResult GetCourier(string uid)
        {
            Log.Debug("Invoked");

            try
            {
                // Get the container record
                var shipper = UnitOfWork.ContainerManager.GetContainer(uid);
                
                if(shipper == null)
                   return BadRequest("Item not in database!");

                // Return the result
                return Ok(UnitOfWork.DistributionManager.GetShipperCouriers(shipper.Id));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}