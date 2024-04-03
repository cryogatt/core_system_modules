using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Material.Entities
{
    public class AttributeField
    {
        public AttributeField()
        { }

        public AttributeField(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     Field name.
        /// </summary>
        [Required(ErrorMessage = "Attribute Name Required!"), StringLength(256)]
        public string Name { get; private set; }

        /// <summary>
        ///     General description of feild.
        /// </summary>
        public string Description { get; private set; }

        public virtual ICollection<AttributeValue> AttributeValue { get; private set; }
    }
}