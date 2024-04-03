using Infrastructure.Users.DTOs;
using Infrastructure.Users.Entites;
using System.Collections.Generic;

namespace Infrastructure.Users.Services
{
    public interface IUserManager
    {
        /// <summary>
        ///     Validate the login credentials.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        ApplicationUser ValidateUser(string username, string password);

        /// <summary>
        ///     Get the response of all users who have not been deleted from the system.
        /// </summary>
        /// <returns></returns>
        UserResponse GetAllActiveUsers();

        /// <summary>
        ///     Get user record by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        User GetUser(string username);

        /// <summary>
        ///     Get the response body of a user by their primary key.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        UserResponseBody GetSingleActiveUser(int id);

        /// <summary>
        ///     Get the response body of a user by their username.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        UserResponseBody GetSingleActiveUser(string username);

        /// <summary>
        ///     Add new user to repository.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int AddUser(UserResponseBody user);

        /// <summary>
        ///     Update the user record in repository.
        /// </summary>
        /// <param name="user"></param>
        void UpdateUserRecord(UserResponseBody user);

        /// <summary>
        ///     Delete the user record in repository.
        /// </summary>
        /// <param name="user"></param>
        void DeleteUserRecord(int id);

        /// <summary>
        ///     Get group by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Group GetGroup(string name);

        /// <summary>
        ///     Get list of all sites.
        /// </summary>
        /// <returns></returns>
        List<Site> GetAllSites();

        /// <summary>
        ///     Get site by primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Site GetSite(int id);

        /// <summary>
        ///     Get site by name.
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        Site GetSite(string siteName);
    }
}
