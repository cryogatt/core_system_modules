using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Distribution.Entities
{
    /// <summary>
    ///     Contents of shipment.
    /// </summary>
    public class Contents
    {
        public Contents()
        {

        }

        public Contents(int id, int shipmentId, int containerId)
        {
            Id = id;
            ShipmentId = shipmentId;
            ContainerId = containerId;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     Associated shipment.
        /// </summary>
        [ForeignKey("Shipment"), Required]
        public int ShipmentId { get; private set; }

        /// <summary>
        ///     Reference to the container record.
        /// </summary>
        [ForeignKey("Container"), Required]
        public int ContainerId { get; private set; }

        public virtual Shipment Shipment { get; private set; }

        public virtual Container.Entities.Container Container { get; private set; }
    }
}