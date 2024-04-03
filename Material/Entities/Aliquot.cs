using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Material.Entities
{
    public class Aliquot
    {
        public Aliquot()
        { }

        public Aliquot(int id, int batchId, string serial, int containerId)
        {
            Id = id;
            BatchId = batchId;
            Serial = serial;
            ContainerId = containerId;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     The batch in which the sample belongs to.
        /// </summary>
        [Required, ForeignKey("Batch")]
        public int BatchId { get; private set; }

        /// <summary>
        ///     Serial number applied to sample.
        /// </summary>
        [StringLength(50)]
        public string Serial { get; private set; }

        /// <summary>
        ///     Reference to its container record.
        /// </summary>
        [ForeignKey("Container"), Required]
        public int ContainerId { get; private set; }

        public virtual Batch Batch { get; private set; }
        public virtual Container.Entities.Container Container { get; private set; }
    }
}
