using System.Collections.Generic;
using Distribution.DTOs;
using Infrastructure.Container.Entities;
using Infrastructure.Distribution.Entities;
using Infrastructure.Users.Services;

namespace Distribution.Services
{
    public interface IDistributionManager
    {
        /// <summary>
        ///     Get order based on their type - default is all orders.
        /// </summary>
        /// <param name="ordersType"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        OrdersResponse GetOrders(OrdersType ordersType = OrdersType.NOTSET, int siteId = 0);

        /// <summary>
        ///     Get specific order record by primary key.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        OrdersResponseBody GetOrder(int uid);

        /// <summary>
        ///     Create new order.
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        int AddOrder(OrdersResponseBody newOrder, IUserManager userManager);

        /// <summary>
        ///     Update an order record.
        /// </summary>
        /// <param name="orderEditted"></param>
        void UpdateOrder(OrdersResponseBody ordersResponseBody, IUserManager userManager);

        /// <summary>
        ///     Update an orders status. 
        /// </summary>
        /// <param name="status"></param>
        void UpdateOrderStatus(Shipment shipment, int status);
        
        /// <summary>
        ///     Get shipment from its primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Shipment GetShipment(int id);

        /// <summary>
        ///     Add samples to a shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="containers"></param>
        void AddContents(int shipmentId, List<Container> containers);

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
        ///     Add the courier record to database.
        /// </summary>
        /// <param name="courier"></param>
        /// <returns></returns>
        int AddCourier(Courier courier);

        /// <summary>
        ///     Get all couriers.
        /// </summary>
        /// <returns></returns>
        CourierResponse GetAllCouriers();

        /// <summary>
        ///     Delete the courier record.
        /// </summary>
        /// <param name="courier"></param>
        void DeleteCourier(Courier courier);

        /// <summary>
        ///     Get the contents of a shipment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Contents> GetShipmentContents(int id);

        /// <summary>
        ///     Get all couriers for shipper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CourierResponse GetShipperCouriers(int id);
    }
}
