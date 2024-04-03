using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Distribution.Entities
{
    public class Courier
    {
        public Courier()
        { }

        public Courier(int id, int containerId, int shipmentId)
        {
            Id = id;
            ContainerId = containerId;
            ShipmentId = shipmentId;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Reference to the the container record.
        /// </summary>
        [ForeignKey("Container")]
        public int ContainerId { get; set; }

        /// <summary>
        ///     Reference to the the shipment record.
        /// </summary>
        [ForeignKey("Shipment")]
        public int ShipmentId { get; set; }

        public virtual Container.Entities.Container Container { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}
