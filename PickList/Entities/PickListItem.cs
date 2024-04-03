using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.PickList.Entities
{
    public class PickListItem
    {
        public PickListItem()
        { }

        public PickListItem(int id, int pickListId, int containerId)
        {
            Id = id;
            PickListId = pickListId;
            ContainerId = containerId;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     List that the item belongs to.
        /// </summary>
        [ForeignKey("PickList"), Required]
        public int PickListId { get; private set; }

        /// <summary>
        ///     Reference to container record.
        /// </summary>
        [ForeignKey("Container"), Required]
        public int ContainerId { get; private set; }

        public virtual PickList PickList { get; set; }
        public virtual Container.Entities.Container Container { get; set; }

        /// <summary>
        ///     Update which pick list the item belongs to.
        /// </summary>
        /// <param name="pickListId"></param>
        public void Update(int pickListId)
        {
            this.PickListId = pickListId;
        }
    }
}
