using CommonEF;
using CommonEF.Services;
using Cryogatt.RFID.Trace;
using Distribution.DTOs;
using Infrastructure.Distribution.Entities;
using Infrastructure.Distribution.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DistributionDAL
{
    public class DistributionRepository : Repository, IDistributionRepository
    {
        #region Constructors

        public DistributionRepository(IContextFactory contextFactory) : base(contextFactory)
        { }

        #endregion

        /// <summary>
        ///     Get all order records from database.
        /// </summary>
        /// <returns></returns>
        public List<OrdersResponseBody> GetAllOrders()
        {
            Log.Debug("Invoked");

            List<OrdersResponseBody> resp = null;

            try
            {
                using (var context = ContextFactory.Create())
                {
                    resp = context.Shipments
                           .Select(shipment => new OrdersResponseBody
                           {
                                Uid = shipment.Id,
                                ConsignmentNo = shipment.ConsignmentNo,
                                Sender = context.Sites
                                    .Where(x => x.Id == shipment.SenderSiteId)
                                    .Select(s => s.Name).FirstOrDefault(),
                                Recipient = context.Sites
                                    .Where(x => x.Id == shipment.RecipientId)
                                    .Select(s => s.Name).FirstOrDefault(),
                                OrderedDate = shipment.OrderedDate,
                                DispatchedDate = shipment.DispatchedDate.Value,
                                ArrivedDate = shipment.ArrivedDate.Value,
                                SampleQty = shipment.SampleQty,
                                Status = shipment.Status.Name
                            }
                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return resp;
        }

        /// <summary>
        ///     Get all orders sent from a given site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<OrdersResponseBody> GetOutgoingOrders(int siteId)
        {
            Log.Debug("Invoked");

            List<OrdersResponseBody> resp = null;

            try
            {
                using (var context = ContextFactory.Create())
                {
                    resp = context.Shipments
                        .Where(shipment => shipment.SenderSiteId == siteId)
                           .Select(shipment => new OrdersResponseBody
                           {
                               Uid = shipment.Id,
                               ConsignmentNo = shipment.ConsignmentNo,
                               Sender = context.Sites
                                    .Where(x => x.Id == shipment.SenderSiteId)
                                    .Select(s => s.Name).FirstOrDefault(),
                               Recipient = context.Sites
                                    .Where(x => x.Id == shipment.RecipientId)
                                    .Select(s => s.Name).FirstOrDefault(),
                               OrderedDate = shipment.OrderedDate,
                               DispatchedDate = shipment.DispatchedDate.Value,
                               ArrivedDate = shipment.ArrivedDate.Value,
                               SampleQty = shipment.SampleQty,
                               Status = shipment.Status.Name
                           }
                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return resp;
        }

        /// <summary>
        ///     Get all orders sent to a given site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public List<OrdersResponseBody> GetIncomingOrders(int siteId)
        {
            Log.Debug("Invoked");

            List<OrdersResponseBody> resp = null;

            try
            {
                using (var context = ContextFactory.Create())
                {
                    resp = context.Shipments
                        .Where(shipment => shipment.RecipientId == siteId)
                           .Select(shipment => new OrdersResponseBody
                           {
                               Uid = shipment.Id,
                               ConsignmentNo = shipment.ConsignmentNo,
                               Sender = context.Sites
                                    .Where(x => x.Id == shipment.SenderSiteId)
                                    .Select(s => s.Name).FirstOrDefault(),
                               Recipient = context.Sites
                                    .Where(x => x.Id == shipment.RecipientId)
                                    .Select(s => s.Name).FirstOrDefault(),
                               OrderedDate = shipment.OrderedDate,
                               DispatchedDate = shipment.DispatchedDate.Value,
                               ArrivedDate = shipment.ArrivedDate.Value,
                               SampleQty = shipment.SampleQty,
                               Status = shipment.Status.Name
                           }
                      ).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return resp;
        }

        /// <summary>
        ///     Get specific order record.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public OrdersResponseBody GetOrder(int uid)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Shipments
                        .Where(shipment => shipment.Id == uid)
                        .Select(shipment => new OrdersResponseBody
                        {
                            Uid = shipment.Id,
                            ConsignmentNo = shipment.ConsignmentNo,
                            Sender = context.Sites
                                    .Where(x => x.Id == shipment.SenderSiteId)
                                    .Select(s => s.Name).FirstOrDefault(),
                            Recipient = context.Sites
                                    .Where(x => x.Id == shipment.RecipientId)
                                    .Select(s => s.Name).FirstOrDefault(),
                            OrderedDate = shipment.OrderedDate,
                            DispatchedDate = shipment.DispatchedDate.Value,
                            ArrivedDate = shipment.ArrivedDate.Value,
                            SampleQty = shipment.SampleQty,
                            Status = shipment.Status.Name
                        }
                    ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get status based on name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Status GetStatus(string name)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Statuses
                        .Where(s => s.Name == name)
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Does the content record already exist?
        /// </summary>
        /// <param name="containerId"></param>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public bool ContentExists(int containerId, int shipmentId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Contents
                    .Any(c => c.ContainerId == containerId &&
                    c.ShipmentId == shipmentId);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Update the number of samples belonging to the shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        public void UpdateShipmentSampleQty(int shipmentId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    var shipment = context.Shipments
                        .Where(s => s.Id == shipmentId)
                        .Single();

                    var sampleQty = context.Contents
                        .Where(c => c.ShipmentId == shipmentId)
                        .Count();

                    if (shipment.SampleQty != sampleQty)
                    {
                        shipment.UpdateSampleQty(sampleQty);

                        context.Update(shipment);

                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }
        /// <summary>
        ///     Delete contents from shipment.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="containerIds"></param>
        public void DeleteContents(int shipmentId, List<int> containerIds)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    var contents = context.Contents
                        .Where(c => c.ShipmentId == shipmentId &&
                        containerIds.Any(id => c.ContainerId == id))
                        .ToList();

                    contents.ForEach(c => context.Contents.Attach(c));

                    contents.ForEach(c => context.Contents.Remove(c));
                    context.SaveChanges();
                }                    
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Find the courier by its container.
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public Courier GetCourier(int containerId)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Couriers
                    .Where(c => c.ContainerId == containerId)
                    .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get the contents of a shipment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Contents> GetShipmentContents(int id)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Contents
                    .Where(c => c.ShipmentId == id)
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get all courier response bodies.
        /// </summary>
        /// <returns></returns>
        public List<CourierResponseBody> GetAllCouriers()
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Couriers
                        .Select(courier =>
                        new CourierResponseBody()
                        {
                            Id = courier.Id,
                            ShipperName = courier.Container.Description,
                            ShipperUid = courier.Container.Uid,
                            ShipmentConsignmentNo = courier.Shipment.ConsignmentNo,
                            ShipmentId = courier.ShipmentId
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get all couriers that the shipper is associated with.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CourierResponseBody> GetShipperCouriers(int id)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Couriers
                        .Where(courier => courier.ContainerId == id)
                        .Select(courier =>
                        new CourierResponseBody()
                        {
                            Id = courier.Id,
                            ShipperName = courier.Container.Description,
                            ShipperUid = courier.Container.Uid,
                            ShipmentConsignmentNo = courier.Shipment.ConsignmentNo,
                            ShipmentId = courier.ShipmentId
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        public void RemoveCourier(Courier courier)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    context.Couriers.Attach(courier);

                    context.Couriers.Remove(courier);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }
    }
}
