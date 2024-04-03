using Distribution.DTOs;
using Distribution.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Distribution.Entities
{
    public class Shipment
    {
        public Shipment()
        {

        }

        public Shipment(int id, string consignmentNo, int senderSiteId, int recipientSiteId, DateTime orderedDate,
            DateTime? dispatchedDate, DateTime? arrivedDate, string notes, int sampleQty, int statusId)
        {
            Id = id;
            ConsignmentNo = consignmentNo;
            SenderSiteId = senderSiteId;
            RecipientId = recipientSiteId;
            OrderedDate = orderedDate;
            DispatchedDate = dispatchedDate;
            ArrivedDate = arrivedDate;
            Notes = notes;
            SampleQty = sampleQty;
            StatusId = statusId;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     Unique reference for the shipment.
        /// </summary>
        [Required, StringLength(450), Index(IsUnique = true)]
        public string ConsignmentNo { get; private set; }

        /// <summary>
        /// The site sending the shipment.
        /// </summary>
        public int SenderSiteId { get; private set; }

        /// <summary>
        ///     The site recieving the shipment.
        /// </summary>
        public int RecipientId { get; private set; }

        /// <summary>
        ///     The date the order was placed.
        /// </summary>
        [Required(ErrorMessage = "Date Required!")]
        public DateTime OrderedDate { get; private set; } = DateTime.Now;

        /// <summary>
        ///     The date the shipment was dispatched.
        /// </summary>
        public DateTime? DispatchedDate { get; private set; }

        /// <summary>
        ///     The date the shipment arrived.
        /// </summary>
        public DateTime? ArrivedDate { get; private set; }

        /// <summary>
        ///     Free text.
        /// </summary>
        public string Notes { get; private set; }

        /// <summary>
        ///     Number of samples in shipments.
        /// </summary>
        public int SampleQty { get; private set; } = 0;

        /// <summary>
        ///     The status of the shipment.
        /// </summary>
        public int StatusId { get; private set; } = 1;

        [ForeignKey("StatusId")]
        public virtual Status Status { get; private set; }

        public virtual ICollection<Contents> Contents { get; private set; }
               
        /// <summary>
        ///     Update the shipment record. 
        ///     Returns false if all given properties are the same.
        /// </summary>
        /// <param name="ordersResponseBody"></param>
        /// <param name="senderSiteId"></param>
        /// <param name="recipientSiteId"></param>
        /// <param name="statusId"></param>
        public bool Update(OrdersResponseBody ordersResponseBody, int senderSiteId, int recipientSiteId, int statusId)
        {
            bool changed = false;
            if (ConsignmentNo != ordersResponseBody.ConsignmentNo)
            {
                ConsignmentNo = ordersResponseBody.ConsignmentNo;
                changed = true;
            }
            if (Notes != ordersResponseBody.Notes)
            {
                Notes = ordersResponseBody.Notes;
                changed = true;
            }
            if (SampleQty != ordersResponseBody.SampleQty)
            {
                SampleQty = ordersResponseBody.SampleQty;
                changed = true;
            }
            if (DispatchedDate != ordersResponseBody.DispatchedDate)
            {
                DispatchedDate = ordersResponseBody.DispatchedDate;
                changed = true;
            }
            if (ArrivedDate != ordersResponseBody.ArrivedDate)
            {
                ArrivedDate = ordersResponseBody.ArrivedDate;
                changed = true;
            }
            if (SenderSiteId != senderSiteId)
            {
                SenderSiteId = senderSiteId;
                changed = true;
            }
            if (RecipientId != recipientSiteId)
            {
                RecipientId = recipientSiteId;
                changed = true;
            }
            if (StatusId != statusId)
            {
                StatusId = statusId;
                changed = true;
            }
            return changed;
        }
        
        public void UpdateSampleQty(int noOfSamples)
        {
            SampleQty = noOfSamples;
        }

        public bool UpdateStatus(int id)
        {
            if (StatusId == id)
                return false;

            if (id == 2)
                DispatchedDate = DateTime.Now;
            else if (id == 3)
                ArrivedDate = DateTime.Now;

            StatusId = id;

            return true;
        }
    }
}
