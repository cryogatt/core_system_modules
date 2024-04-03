using Infrastructure.Container.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Users.Entites
{
    public class Site
    {
        public Site()
        {

        }

        public Site(int id, string address, string name, int containerId)
        {
            Id = id;
            Address = address;
            Name = name;
            ContainerId = containerId;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Business address.
        /// </summary>
        [StringLength(450)]
        public string Address { get; set; }

        /// <summary>
        ///     Given name of site not company.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Associated container record.
        /// </summary>
      //  [ForeignKey("Container")]
        public int ContainerId { get; set; }

        public virtual Container.Entities.Container Container { get; set; }

        /// <summary>
        ///     All users belonging to site.
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

    }
}
