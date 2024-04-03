using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Users.Entites
{
    public class Role
    {
        /// <summary>
        ///     Default Constructor required for entity framework to create the object
        ///     when reading out of the database.
        /// </summary>
        public Role()
        { }

        /// <summary>
        ///     Constructor for creating a new Role.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="createdAt"></param>
        public Role(string Name, string Description, DateTime createdAt)
        {
            this.Name = Name;
            this.Description = Description;
            this.CreatedAt = createdAt;
        }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        ///     Associated name of role.
        /// </summary>
        [Required, StringLength(20)]
        public string Name { get; private set; }

        /// <summary>
        ///     General description of role.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        ///     Date & Time created.
        /// </summary>
        [Required(ErrorMessage = "Date Required!")]
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
    }
}
