using System;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using System.Collections.Generic;
using Distribution.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        public OrdersController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        // GET: api/v1/ListOfOrders?status={Direction of shipment}
        [ResponseType(typeof(List<OrdersResponseBody>))]
        public IHttpActionResult GetListOfOrders()
        {
            Log.Debug("Invoked");

            string status = "";
            foreach (var param in Request.GetQueryNameValuePairs())
                if (param.Key.ToUpper().Trim() == "STATUS")
                    status = param.Value;

            // Determine the type of orders required
            OrdersType ordersType = OrdersType.NOTSET;
            switch (status)
            {
                case "INBOUND":
                    ordersType = OrdersType.INBOUND;
                    break;
                case "OUTBOUND":
                    ordersType = OrdersType.OUTBOUND;
                    break;
            }
            try
            {
                // Get the current user id
                var user = UnitOfWork.UserManager.GetUser(RequestContext.Principal.Identity.Name);
                // Get users site - TODO Ammend for users of multiple sites
                var site = user.Sites.First();
                // return response
                return Ok(UnitOfWork.DistributionManager.GetOrders(ordersType, site.Id));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // GET: api/v1/ListOfOrders/uid
        [ResponseType(typeof(OrdersResponseBody))]
        public IHttpActionResult GetOrder(int uid)
        {
            Log.Debug("Invoked");

            try
            {
                OrdersResponseBody result = UnitOfWork.DistributionManager.GetOrder(uid);

                // Return error if not found
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        // PUT: api/v1/Orders/uid (edited order record)
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int uid, OrdersResponseBody orderEditted)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }
            if (uid != orderEditted.Uid)
            {
                Log.Error("Edited order ID does not match the ID in the URI.");
                return BadRequest("Edited order ID does not match the ID in the URI.");
            }

            try
            {
                OrdersResponseBody orderSaved = UnitOfWork.DistributionManager.GetOrder(uid);

                if (orderSaved == null)
                    return BadRequest($"Order with id {uid} does not exist!");

                UnitOfWork.DistributionManager.UpdateOrder(orderEditted, UnitOfWork.UserManager);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
            return Ok();
        }

        // POST: api/v1/Orders (new Order record)
        [ResponseType(typeof(void))]
        public IHttpActionResult PostOrder(OrdersResponseBody newOrder)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            try
            {
                int shipmentId = UnitOfWork.DistributionManager.AddOrder(newOrder, UnitOfWork.UserManager);

                if (shipmentId == 0)
                {
                    Log.Error("Failed to create Order entry into database.");
                    return BadRequest("Failed to create Order entry into database.");
                }

                // Return the Order record
                OrdersResponseBody result = UnitOfWork.DistributionManager.GetOrder(shipmentId);
                return CreatedAtRoute("DefaultApi", new { uid = result.Uid }, result);                
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }
    }
}