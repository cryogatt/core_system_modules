using Infrastructure.Distribution.Entities;
using Infrastructure.Users.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Distribution.Entities
{
    public class Recipient
    {
        /// <summary>
        ///     Foreign key to the shipment record.
        /// </summary>
        [Key]
        [ForeignKey("Shipment")]
        public int ShipmentId { get; set; }

        /// <summary>
        ///     Foreign key to the site record.
        /// </summary>
        [Required]
        public int SiteId { get; set; }

        public virtual Shipment Shipment { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }
    }
}
