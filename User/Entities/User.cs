using CryptSharp;
using Infrastructure.Users.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Users.Entites
{
    public class User
    {
        #region Constructors 

        /// <summary>
        ///     Default Constructor required for entity framework to create the object
        ///     when reading out of the database.
        /// </summary>
        public User()
        { }

        /// <summary>
        ///     Constructor for creating a new User.
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public User(int id, string firstname, string lastname, string username, string email, string password,
            int roleId = 0, List<Site> sites = null, List<Group> groups = null, DateTime? deletedAt = null, 
            DateTime? lastLogin = null, DateTime? updatedAt = null, Role role = null)
        {
            this.Id = id;
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Username = username;
            this.Email = email;
            this.Password = Crypter.Blowfish.Crypt(password);
            this.RoleId = roleId;
            this.Sites = sites;
            this.Groups = groups;
            this.CreatedAt = DateTime.Now;
            this.DeletedAt = deletedAt;
            this.LastLogin = lastLogin;
            this.UpdatedAt = updatedAt;
            this.Role = role;
        }
        #endregion

        #region Public Properties

        /// <summary>
        ///     Primary Key.
        /// </summary>
        [Key]
        public int Id { get; private set; }
        
        /// <summary>
        ///     Given name. 
        /// </summary>
        [Required, StringLength(50)]
        public string FirstName { get; private set; }

        /// <summary>
        ///     Family name.
        /// </summary>
        [Required, StringLength(50)]
        public string LastName { get; private set; }

        /// <summary>
        ///     Username required for logging in.
        /// </summary>
        [Required, StringLength(20)]
        public string Username { get; private set; }

        /// <summary>
        ///     Optional email.
        /// </summary>
        [StringLength(450)]
        public string Email { get; private set; }

        /// <summary>
        ///     Password required for logging in.
        /// </summary>
        [Required, StringLength(450, MinimumLength = 5)]
        public string Password { get; private set; }

        /// <summary>
        ///     The date of when the user was last logged in.
        /// </summary>
        public DateTime? LastLogin { get; private set; }

        /// <summary>
        ///     N/A?
        /// </summary>
        public string RememberToken { get; private set; }

        /// <summary>
        ///     Date the user was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        ///     The date the user was created.
        /// </summary>
        public DateTime? CreatedAt { get; private set; }

        /// <summary>
        ///     The date the user was deleted.
        /// </summary>
        public DateTime? DeletedAt { get; private set; }

        /// <summary>
        ///     The role assigned to user.
        /// </summary>
        public int RoleId { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        ///     The role assigned to user.
        /// </summary>
        public virtual Role Role { get; set; }

        ///// <summary>
        /////     The sites in which the user belongs to. 
        ///// </summary>
        [ForeignKey("Site")]
        public virtual ICollection<Site> Sites { get; set; }

        /// <summary>
        ///     The groups in which the user belongs to.
        /// </summary>
        [ForeignKey("Group")]
        public virtual ICollection<Group> Groups { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Update user. Notes, does not include groups & sites relationship.
        /// </summary>
        /// <param name="userResponseBody"></param>
        public void Update(UserResponseBody userResponseBody)
        {
            this.FirstName = userResponseBody.FirstName;
            this.LastName = userResponseBody.LastName;
            this.Username = userResponseBody.Username;
            this.Email = userResponseBody.Email;
            this.RoleId = userResponseBody.IsAdmin ? 1 : 2; // TODO Remove hardcoded rule.
            this.CreatedAt = userResponseBody.AddedDate;
            this.DeletedAt = userResponseBody.DeletedAt;
            this.LastLogin = userResponseBody.LastLogin;
            this.UpdatedAt = DateTime.Now;

            // Only update password if it contains a valid value:
            if (!string.IsNullOrEmpty(userResponseBody.Password))
                this.Password = Crypter.Blowfish.Crypt(userResponseBody.Password);
        } 

        #endregion
    }
}
