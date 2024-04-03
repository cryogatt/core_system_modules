using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Distribution.Entities
{
    /// <summary>
    ///     The status of an order/shipment.
    /// </summary>
    public class Status
    {
        public Status()
        { }

        public Status(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     The actual value.
        /// </summary>
        [Required]
        public string Name { get; private set; }

        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}