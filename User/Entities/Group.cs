using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Users.Entites
{
    public class Group
    {
        #region Constructors
        
        /// <summary>
        ///     Default Constructor required for entity framework to create the object
        ///     when reading out of the database.
        /// </summary>
        public Group()
        { }

        /// <summary>
        ///      Constructor for creating a new Group.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="date"></param>
        public Group(string name, string description, DateTime date)
        {
            this.Name = name;
            this.Description = description;
            this.Date = date;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     Given name for the group.
        /// </summary>    
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Group Name Required!"), StringLength(50)]
        public string Name { get; private set; }

        /// <summary>
        ///     General description of group.
        /// </summary>
        [StringLength(250)]
        public string Description { get; private set; }

        /// <summary>
        ///     Date created.
        /// </summary>
        [Required(ErrorMessage = "Date Required!")]
        public DateTime Date { get; private set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        ///     Users belonging to group.
        /// </summary>
        public virtual ICollection<User> Users { get; private set; }

        #endregion
    }
}
