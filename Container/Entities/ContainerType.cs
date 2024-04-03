using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Container.Entities
{
    public class ContainerType
    {
        /// <summary>
        ///     Empty Constructor required by entity framework. 
        /// </summary>
        public ContainerType()
        { }

        public ContainerType(int id, string description, ICollection<ContainerIdent> containerIdent = null)
        {
            Id = id;
            Description = description;

            if (containerIdent != null)
                ContainerIdent = containerIdent;
        }

        public ContainerType(string description)
        {
            Description = description;
        }
        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     Name/Information.
        /// </summary>
        [Required(ErrorMessage = "Description Required")]
        public string Description { get; set; }

        public virtual ICollection<ContainerIdent> ContainerIdent { get; private set; }
    }
}