using System.Collections.Generic;
using System.Linq;
using Common;
using Distribution.DTOs;
using Infrastructure.Distribution.Entities;

namespace Infrastructure.Distribution.Services
{
    public interface IDistributionRepository : IRepository
    {
        /// <summary>
        ///     Get all order records from database.
        /// </summary>
        /// <returns></returns>
        List<OrdersResponseBody> GetAllOrders();

        /// <summary>
        ///     Get all orders sent from a given site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        List<OrdersResponseBody> GetOutgoingOrders(int siteId);

        /// <summary>
        ///     Get all orders sent to a given site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        List<OrdersResponseBody> GetIncomingOrders(int siteId);

        /// <summary>
        ///     Get specific order record.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        OrdersResponseBody GetOrder(int uid);

        /// <summary>
        ///     Get status based on name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Status GetStatus(string name);

        /// <summary>
        ///     Does the content record already exist?
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        bool ContentExists(int containerId, int shipmentId);

        /// <summary>
        ///     Update the number of samples belonging to the shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        void UpdateShipmentSampleQty(int shipmentId);

        /// <summary>
        ///     Delete contents from shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="containerIds"></param>
        void DeleteContents(int shipmentId, List<int> containerIds);

        /// <summary>
        ///     Find the courier by its container.
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        Courier GetCourier(int containerId);

        /// <summary>
        ///     Get the contents of a shipment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Contents> GetShipmentContents(int id);

        /// <summary>
        ///     Get all courier response bodies.
        /// </summary>
        /// <returns></returns>
        List<CourierResponseBody> GetAllCouriers();

        /// <summary>
        ///     Get all couriers that the shipper is associated with.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<CourierResponseBody> GetShipperCouriers(int id);

        void RemoveCourier(Courier courier);
    }
}
