using System;
using System.Collections.Generic;
using System.Linq;
using CryptSharp;
using Infrastructure.Users.Entites;
using Newtonsoft.Json;

namespace Infrastructure.Users.DTOs
{
    public class UserResponseBody
    {
        /// <summary>
        ///     Default constructor required for serialisation.
        /// </summary>
        public UserResponseBody() { }

        /// <summary>
        ///     Primary key.
        /// </summary>
        [JsonProperty]
        public int Id { get; set; }

        /// <summary>
        ///     Credentials required for login.
        /// </summary>
        [JsonProperty]
        public string Username { get; set; }

        /// <summary>
        ///     Given name.
        /// </summary>
        [JsonProperty]
        public string FirstName { get; set; }

        /// <summary>
        ///     Family name.
        /// </summary>
        [JsonProperty]
        public string LastName { get; set; }

        /// <summary>
        ///     Contact info.
        /// </summary>
        [JsonProperty]
        public string Email { get; set; }

        /// <summary>
        ///     Date of last activity.
        /// </summary>
        [JsonProperty]
        public DateTime? LastLogin { get; set; }
        
        /// <summary>
        ///     Date user was added. 
        /// </summary>
        [JsonProperty]
        public DateTime? AddedDate { get; set; }

        /// <summary>
        ///     Date the user was last updated.
        /// </summary>
        [JsonProperty]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        ///     Date the user was (soft) deleted from system.
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        ///     Credentials required for login.
        /// </summary>
        [JsonProperty]
        public string Password { get; set; }

        /// <summary>
        ///     Has admin privilages?
        /// </summary>
        [JsonProperty]
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        ///     Names of sites the user belonsg to.
        /// </summary>
        [JsonProperty]
        public List<string> Sites { get; set; }

        /// <summary>
        ///     Names of groups the user belongs to.
        /// </summary>
        [JsonProperty]
        public List<string> Groups { get; set; }
    }

    /// <summary>
    ///     Helper methods.
    /// </summary>
    public static class UserExtensionMethods
    {
        /// <summary>
        ///     Convert the user response body to a user record.
        /// </summary>
        /// <param name="userResponseBody"></param>
        /// <returns></returns>
        public static User ToUser(this UserResponseBody userResponseBody)
            => new User(
                    userResponseBody.Id,
                    userResponseBody.FirstName,
                    userResponseBody.LastName,
                    userResponseBody.Username,
                    userResponseBody.Email,
                    userResponseBody.Password
                    );        
    }
}
