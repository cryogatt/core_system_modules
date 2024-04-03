using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Container.Entities
{
    public class ContainerIdent
    {
        /// <summary>
        ///     Empty Constructor required by entity framework. 
        /// </summary>
        public ContainerIdent()
        { }

        public ContainerIdent(int id, string description, int tagIdent, int containerTypeId, 
            ContainerType containerType = null, ICollection<Container> container = null)
        {
            Id = id;
            Description = description;
            TagIdent = tagIdent;
            ContainerTypeId = containerTypeId;
            ContainerType = containerType;
            Container = container;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     General name.
        /// </summary>
        [Required(ErrorMessage = "Container Ident Description Required!")]
        public string Description { get; private set; }

        /// <summary>
        ///     Code written to block 0.
        /// </summary>
        [Required(ErrorMessage = "Ident Written to the Tag required!")]
        public int TagIdent { get; private set; }

        /// <summary>
        ///     Type of container.
        /// </summary>
        [ForeignKey("ContainerType"), Required(ErrorMessage = "Container Type Required!")]
        public int ContainerTypeId { get; private set; }

        public virtual ContainerType ContainerType { get; private set; }
        public virtual ICollection<Container> Container { get; private set; }
    }
}