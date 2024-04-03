using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Material.Entities
{
    public class Batch
    {
        public Batch()
        { }

        public Batch(int id, int batchTypeId, int batchInfoId)
        {
            Id = id;
            BatchTypeId = batchTypeId;
            BatchInfoId = batchInfoId;
        }

        /// <summary>
        ///     Pimary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }
               
        [Required]
        [ForeignKey("BatchType")]
        public int BatchTypeId { get; private set; }

        [Required]
        [ForeignKey("BatchInfo")]
        public int BatchInfoId { get; private set; }

        public virtual BatchType BatchType { get; set; }
        public virtual BatchInfo BatchInfo { get; set; }        
    }
}