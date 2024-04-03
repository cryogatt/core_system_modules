using Infrastructure.Users.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Material.Entities
{
    public class BatchInfo
    {
        public BatchInfo()
        {

        }

        public BatchInfo(int id, string name, string notes, int groupId, int cryoSeedQty, int testedSeedQty, int sDSeedQty, DateTime date)
        {
            Id = id;
            Name = name;
            Notes = notes;
            GroupId = groupId;
            CryoSeedQty = cryoSeedQty;
            TestedSeedQty = testedSeedQty;
            SDSeedQty = sDSeedQty;
            Date = date;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50), Index(IsUnique = true)]
        public string Name { get; set; }

        /// <summary>
        ///     Free text.
        /// </summary>
        [StringLength(450)]
        public string Notes { get; private set; }

        /// <summary>
        ///     Group/team/project in which the batch belongs to.
        /// </summary>
        [Required]
        [ForeignKey("Group")]
        public int GroupId { get; private set; }

        public int CryoSeedQty { get; private set; }
        public int TestedSeedQty { get; private set; }
        public int SDSeedQty { get; private set; }

        /// <summary>
        ///     Creation date.
        /// </summary>
        [Required(ErrorMessage = "Date Required!")]
        public DateTime Date { get; private set; }
        
        public virtual Group Group { get; set; }
        public virtual ICollection<Aliquot> Aliquot { get; private set; }
        public virtual ICollection<AttributeValue> AttributeValue { get; private set; }

        /// <summary>
        ///     Update the notes property.
        /// </summary>
        /// <param name="notes"></param>
        public void Update(string notes)
        {
            this.Notes = notes;
        }
    }
}