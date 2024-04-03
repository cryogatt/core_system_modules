using Common;
using Infrastructure.Users.DTOs;
using Infrastructure.Users.Entites;
using System.Collections.Generic;

namespace Infrastructure.Users.Services
{
    public interface IUserRepository : IRepository
    {
        /// <summary>
        ///     Get all user records.
        /// </summary>
        /// <returns></returns>
        List<User> GetAllActiveUsers();

        /// <summary>
        ///     Get all user records.
        /// </summary>
        /// <returns></returns>
        List<UserResponseBody> GetAllActiveUsersResponseBodies();

        /// <summary>
        ///     Return a user from their username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        User GetActiveUser(string username);

        /// <summary>
        ///     Return a user with all navigation properties.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetActiveUserIncludeRelationShips(int id);

        /// <summary>
        ///     Return a user response body from primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserResponseBody GetActiveUserResponseBody(int id);

        /// <summary>
        ///     Return a user response body from unique username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserResponseBody GetActiveUserResponseBody(string username);
        
        /// <summary>
        ///     Get Group by its given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Group GetGroup(string name);

        /// <summary>
        ///     Get all sites by name.
        /// </summary>
        /// <param name="sites"></param>
        /// <returns></returns>
        List<Site> GetSites(List<string> siteNames);

        /// <summary>
        ///     Get groups by name.
        /// </summary>
        /// <param name="groupNames"></param>
        /// <returns></returns>
        List<Group> GetGroups(List<string> groupNames);

        /// <summary>
        ///     Get site by name.
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        Site GetSite(string siteName);

        void AddUser(User user);
    }
}