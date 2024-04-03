using Cryogatt.RFID.Trace;
using Infrastructure.Users.Entites;
using CommonEF;
using Infrastructure.Users.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Infrastructure.Users.DTOs;
using CommonEF.Services;

namespace UsersDAL
{
    /// <summary>
    ///     Service to user data.
    /// </summary>
    public class UserRepository : 
        Repository,
        IUserRepository
    {
        #region Constructors

        public UserRepository(IContextFactory contextFactory) : base(contextFactory)
        { }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Get a user by unique username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>null if not found</returns>
        public User GetActiveUser(string username)
        {
            Log.Debug("Invoked");

            try
            { 
                using (var context = ContextFactory.Create())
                {
                    return context.Users
                        .Where(u => u.Username.ToUpper().Equals(username.Trim().ToUpper())
                        && u.DeletedAt == null)
                        .Include(u => u.Role)
                        .Include(u => u.Groups)
                        .Include(u => u.Sites)
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get the list of active users.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllActiveUsers()
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Users
                    .Where(u => u.DeletedAt == null)
                    .Include(u => u.Role)
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get all user records.
        /// </summary>
        /// <returns></returns>
        public List<UserResponseBody> GetAllActiveUsersResponseBodies()
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Users
                    .Where(user => user.DeletedAt == null)
                    .Select(user =>
                    new UserResponseBody
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        LastLogin = user.LastLogin,
                        AddedDate = user.CreatedAt,
                        UpdatedDate = user.UpdatedAt,
                        DeletedAt = user.DeletedAt,
                        IsAdmin = user.Role.Name == "Admin",
                        Groups = user.Groups.Select(g => g.Name).ToList(),
                        Sites = user.Sites.Select(s => s.Name).ToList()
                    }).ToList(); 
                }      
            }          
            catch (Exception ex)
            {          
                Log.Error(ex.ToString()); throw;
            }          
        }              

        /// <summary>
        ///     Includes the navigation properties.
        /// </summary>
        /// <returns></returns>
        public User GetActiveUserIncludeRelationShips(int id)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Users
                        .Where(u =>
                        u.DeletedAt == null &&
                        u.Id == id)
                        .Include(u => u.Groups)
                        .Include(u => u.Sites)
                        .Include(u => u.Role)
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Return a user with all navigation properties.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserResponseBody GetActiveUserResponseBody(int id)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Users
                    .Where(user => user.DeletedAt == null)
                    .Where(user => user.Id == id)
                    .Select(user =>
                    new UserResponseBody
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        LastLogin = user.LastLogin,
                        AddedDate = user.CreatedAt,
                        UpdatedDate = user.UpdatedAt,
                        DeletedAt = user.DeletedAt,
                        IsAdmin = user.Role.Name == "Admin",
                        Groups = user.Groups.Select(g => g.Name).ToList(),
                        Sites = user.Sites.Select(s => s.Name).ToList()
                    }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Return a user response body from unique username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserResponseBody GetActiveUserResponseBody(string username)
        {
            Log.Debug("Invoked");

            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Users
                    .Where(user => user.DeletedAt == null)
                    .Where(user => user.Username == username)
                    .Select(user =>
                    new UserResponseBody
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        LastLogin = user.LastLogin,
                        AddedDate = user.CreatedAt,
                        UpdatedDate = user.UpdatedAt,
                        DeletedAt = user.DeletedAt,
                        IsAdmin = user.Role.Name == "Admin",
                        Groups = user.Groups.Select(g => g.Name).ToList(),
                        Sites = user.Sites.Select(s => s.Name).ToList()
                    }).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get group by unique name parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Group GetGroup(string name)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Groups
                    .Where(g => g.Name == name)
                    .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get group records from database from their given names.
        /// </summary>
        /// <param name="groupNames"></param>
        /// <returns></returns>
        public List<Group> GetGroups(List<string> groupNames)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return groupNames
                        .Select(name => context.Groups
                        .Where(g => g.Name == name)
                        .SingleOrDefault())
                        .AsQueryable()
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get site records from database from their given names.
        /// </summary>
        /// <param name="siteNames"></param>
        /// <returns></returns>
        public List<Site> GetSites(List<string> siteNames)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return siteNames
                        .Select(name => context.Sites
                        .Where(g => g.Name == name)
                        .SingleOrDefault())
                        .AsQueryable()
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        /// <summary>
        ///     Get site by name.
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public Site GetSite(string siteName)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    return context.Sites
                        .Where(s => s.Name == siteName)
                        .SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        public void AddUser(User user)
        {
            try
            {
                using (var context = ContextFactory.Create())
                {
                    context.Sites.Attach(user.Sites.FirstOrDefault());

                    context.Groups.Attach(user.Groups.FirstOrDefault());

                    context.Users.Add(user);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString()); throw;
            }
        }

        #endregion
    }
}
