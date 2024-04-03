using Distribution.DTOs;
using Distribution.Services;
using Infrastructure.Container.Entities;
using Infrastructure.Distribution.Entities;
using Infrastructure.Distribution.Services;
using Infrastructure.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DistributionService
{
    public class DistributionManager : IDistributionManager
    {
        #region Constructors

        public DistributionManager(IDistributionRepository repository)
        {
            this.Repository = repository;
        }
        
        #endregion

        #region Private Properties

        /// <summary>
        ///     Data access to user repository.
        /// </summary>
        private readonly IDistributionRepository Repository;

        #endregion

        #region Public Methods

        /// <summary>
        ///     Get order based on their type - default is all orders.
        /// </summary>
        /// <param name="ordersType"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public OrdersResponse GetOrders(OrdersType ordersType = OrdersType.NOTSET, int siteId = 0)
        {
            List<OrdersResponseBody> responseBodies = null;
            switch (ordersType)
            {
                case (OrdersType.INBOUND):
                    responseBodies = Repository.GetIncomingOrders(siteId);
                    break;
                case (OrdersType.OUTBOUND):
                    responseBodies = Repository.GetOutgoingOrders(siteId);
                    break;
                default:
                    responseBodies = Repository.GetAllOrders();
                    break;
            }

            if (responseBodies == null)
                return null;

            return new OrdersResponse(
                responseBodies.Count(),
                responseBodies);
        }

        /// <summary>
        ///     Get specific order record.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public OrdersResponseBody GetOrder(int uid)
        {
            return Repository.GetOrder(uid);
        }

        /// <summary>
        ///     Create new order.
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        public int AddOrder(OrdersResponseBody newOrder, IUserManager userManager)
        {
            if (newOrder == null)
                throw new NullReferenceException();

            // Ensure a consignment number has been provided
            if (newOrder.ConsignmentNo == null)
                throw new Exception($"{newOrder.ConsignmentNo} not specified!");

            //  Get sender of the order
            var sender = userManager.GetSite(newOrder.Sender);
            if (sender == null)
                throw new Exception($"{newOrder.Sender} not recognised!");

            // Get recipient of shipment
            var recipient = userManager.GetSite(newOrder.Recipient);
            if (recipient == null)
                throw new Exception($"{newOrder.Recipient} not recognised!");

            // Create shipment record
            var shipment = new Shipment(
                0,
                newOrder.ConsignmentNo,
                sender.Id,
                recipient.Id,
                DateTime.Now,
                null,
                null,
                newOrder.Notes,
                newOrder.SampleQty,
                1); // TODO Remove hard coded status id.

            Repository.Add(shipment);

            return shipment.Id;
        }

        /// <summary>
        ///     Update an order record.
        /// </summary>
        /// <param name="orderEditted"></param>
        public void UpdateOrder(OrdersResponseBody ordersResponseBody, IUserManager userManager)
        {
            if (ordersResponseBody == null)
                throw new NullReferenceException();

            // Ensure a consignment number has been provided
            if (ordersResponseBody.ConsignmentNo == null)
                throw new Exception($"{ordersResponseBody.ConsignmentNo} not specified!");

            //  Get sender of the order
            var sender = userManager.GetSite(ordersResponseBody.Sender);
            if (sender == null)
                throw new Exception($"{ordersResponseBody.Sender} not recognised!");

            // Get recipient of shipment
            var recipient = userManager.GetSite(ordersResponseBody.Recipient);
            if (recipient == null)
                throw new Exception($"{ordersResponseBody.Recipient} not recognised!");

            var shipment = Repository.Get<Shipment>(ordersResponseBody.Uid);
            if (shipment == null)
                throw new Exception("Could not find shipment!");

            // Get the status of the order
            Status status = Repository.GetStatus(ordersResponseBody.Status);
            if (status == null)
                throw new Exception("Status not found: " + ordersResponseBody.Status);

            // Update entity
            var changed = shipment.Update(ordersResponseBody, sender.Id, recipient.Id, status.Id);

            // Update repository
            if (changed)
                Repository.Update(shipment);            
        }

        /// <summary>
        ///     Update an orders status. 
        /// </summary>
        /// <param name="statusId"></param>
        public void UpdateOrderStatus(Shipment shipment, int statusId)
        {
            var status = Repository.Get<Status>(statusId);

            var changed = shipment.UpdateStatus(status.Id);

            if (changed)
                Repository.Update(shipment);
        }

        /// <summary>
        ///     Get shipment from its primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Shipment GetShipment(int id)
        {
            return Repository.Get<Shipment>(id);
        }

        /// <summary>
        ///     Add samples to a shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="containers"></param>
        public void AddContents(int shipmentId, List<Container> containers)
        {
            //  Create content records & filter for only records not already part of a shipment
            var contents = containers
                .Where(c => !Repository.ContentExists(c.Id, shipmentId))
                .Select(container =>
                    new Contents(
                        0,
                        shipmentId,
                        container.Id))
                        .ToList();

            // Stage all
            contents.ForEach(c => Repository.Add(c));

            var shipment = Repository.Get<Shipment>(shipmentId);
            
            // Update the number of samples belonging to the shipment
            Repository.UpdateShipmentSampleQty(shipmentId);
        }

        /// <summary>
        ///     Delete contents from shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="containerIds"></param>
        public void DeleteContents(int shipmentId, List<int> containerIds)
        {
            // Delete the contents
            Repository.DeleteContents(shipmentId, containerIds);

            // Update the shipment record
            Repository.UpdateShipmentSampleQty(shipmentId);
        }

        /// <summary>
        ///     Find the courier by its container.
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public Courier GetCourier(int containerId)
        {
            return Repository.GetCourier(containerId);
        }

        /// <summary>
        ///     Add the courier record to database.
        /// </summary>
        /// <param name="courier"></param>
        /// <returns></returns>
        public int AddCourier(Courier courier)
        {
            Repository.Add(courier);

            return courier.Id;
        }

        /// <summary>
        ///     Delete the courier record.
        /// </summary>
        /// <param name="courier"></param>
        public void DeleteCourier(Courier courier)
        {
            Repository.RemoveCourier(courier);
        }

        /// <summary>
        ///     Get the contents of a shipment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Contents> GetShipmentContents(int id)
        {
            return Repository.GetShipmentContents(id);
        }

        /// <summary>
        ///     Get all couriers.
        /// </summary>
        /// <returns></returns>
        public CourierResponse GetAllCouriers()
        {
            var couriers = Repository.GetAllCouriers();

            return new CourierResponse(
                couriers.Count(),
                couriers);
        }

        /// <summary>
        ///     Get all couriers for shipper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourierResponse GetShipperCouriers(int id)
        {
            var couriers = Repository.GetShipperCouriers(id);

            return new CourierResponse(
                couriers.Count(),
                couriers);
        }

        #endregion
    }
}
