using Infrastructure.Distribution.Entities;
using System;

namespace Distribution.DTOs
{
    public class OrdersResponseBody
    {
        public OrdersResponseBody() { }

        /// <summary>
        ///     Primary key.
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        ///     Name of order.
        /// </summary>
        public string ConsignmentNo { get; set; }

        /// <summary>
        ///     Name of site sending the order.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        ///     Name of site recieving the order.
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        ///     Date the order was placed.
        /// </summary>
        public DateTime? OrderedDate { get; set; }

        /// <summary>
        ///     Date the order was dispatched.
        /// </summary>
        public DateTime? DispatchedDate { get; set; }

        /// <summary>
        ///     Date the order arrived at recipeint site.
        /// </summary>
        public DateTime? ArrivedDate { get; set; }

        /// <summary>
        ///     Number of samples in the order.
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        ///     Description of the orders progress, in-transit, being prepared, etc. 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///     Free text.
        /// </summary>
        public string Notes { get; set; }
    }
}
