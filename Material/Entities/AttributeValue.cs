using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Material.Entities
{
    public class AttributeValue
    {
        public AttributeValue()
        { }

        public AttributeValue(int id, string value, int attributeFieldId, int batchInfoId)
        {
            Id = id;
            Value = value;
            AttributeFieldId = attributeFieldId;
            BatchInfoId = batchInfoId;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     User entered value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        ///     The field/header that the value belongs to.
        /// </summary>
        [Required, ForeignKey("AttributeField")]
        public int AttributeFieldId { get; private set; }

        /// <summary>
        ///     The batch in which the value belongs to.
        /// </summary>
        [Required, ForeignKey("BatchInfo")]
        public int BatchInfoId { get; private set; }

        public virtual AttributeField AttributeField { get; private set; }
        public virtual BatchInfo BatchInfo { get; private set; }

        /// <summary>
        ///     Update the value.
        /// </summary>
        /// <param name="value"></param>
        public void Update(string value)
        {
            this.Value = value;
        }
    }
}