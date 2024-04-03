using System;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using System.Collections.Generic;
using Infrastructure.Material.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class ContentsController : ApiController
    {
        public ContentsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        ///     GET: api/v1/Contents?STATUS?{SITE/SHIPMENT}/{uid}
        ///     Where uid is the primary key of the shipment.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [ResponseType(typeof(AliquotResponse))]
        public IHttpActionResult GetContents(int uid)
        {
            Log.Debug("Invoked");

            string status = null;

            foreach (var param in Request.GetQueryNameValuePairs())
            {
                if (param.Key.ToUpper().Trim() == "STATUS")
                    status = param.Value;
            }
            try
            {
                //Get shipment from primary key
                var shipment = UnitOfWork.DistributionManager.GetShipment(uid);

                AliquotResponse resp = null;

                if (status == "SITE")
                {
                    //Get all contents belonging to site
                    resp = UnitOfWork.MaterialManager.GetAllSiteSamples(shipment.SenderSiteId);
                }
                else if (status == "SHIPMENT")
                {
                    //Get all contents belonging to shipment
                    resp = UnitOfWork.MaterialManager.GetAllShipmentSamples(shipment.Id);
                }                              

                // Return the result
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // POST: api/v1/Contents/{uid} 
        [ResponseType(typeof(void))]
        public IHttpActionResult AddContents(int uid, List<string> containerUids)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            try
            {
                var containers = containerUids
                    .Select(containerUid => UnitOfWork.ContainerManager.GetContainer(containerUid))
                    .ToList();

                if (containers.Any(c => c == null))
                    return BadRequest("Container not recognised!");
                
                UnitOfWork.DistributionManager.AddContents(uid, containers);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // DELETE: api/Contents/{ShipmentId} (delete extries contents)
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteContents(int uid, List<string> contents)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            try
            {
                // Get container records
                var containers = contents
                    .Select(containerUid => UnitOfWork.ContainerManager.GetContainer(containerUid));

                if(containers.Any(c => c == null))
                    return BadRequest("Containers not in database!");
                
                // Delete records
                UnitOfWork.DistributionManager.DeleteContents(uid, containers.Select(c => c.Id).ToList());
                
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


