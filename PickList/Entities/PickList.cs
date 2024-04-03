using Infrastructure.Users.Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.PickList.Entities
{
    public class PickList
    {
        public PickList()
        { }

        public PickList(int id, string name, int userId)
        {
            Id = id;
            Name = name;
            UserId = userId;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Name of list.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     User in which the list belongs to.
        /// </summary>
        [ForeignKey("User"), Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<PickListItem> PickListItems { get; set; }
    }
}