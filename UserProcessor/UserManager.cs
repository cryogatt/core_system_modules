using Cryogatt.RFID.Trace;
using CryptSharp;
using Infrastructure.Users;
using Infrastructure.Users.DTOs;
using Infrastructure.Users.Entites;
using Infrastructure.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UserService
{
    public class UserManager : IUserManager
    {
        #region Constructors

        /// <summary>
        ///     Business logic for user operations.
        /// </summary>
        /// <param name="repository"></param>
        public UserManager(IUserRepository repository)
        {
            this.Repository = repository;
        }

        #endregion

        #region Private Properties

        /// <summary>
        ///     Data access to user repository.
        /// </summary>
        private readonly IUserRepository Repository;

        #endregion

        /// <summary>
        ///     Add new user to database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Primary Key</returns>
        public int AddUser(UserResponseBody user)
        {
            var sites = Repository.GetSites(user.Sites);

            // Throw exception if given sites are not in database.
            if (sites.Any(s => s == null))
                throw new ArgumentException("Given site not in db");

            var groups = Repository.GetGroups(user.Groups);

            // HACK
            groups.Add(Repository.GetGroup("Cryogatt Employees"));
            // ENDHACK
            
            // Throw exception if given sites are not in database.
            if (groups.Any(s => s == null))
                throw new ArgumentException("Given groups not in db");

            // TODO Remove hardcoded rule for role
            int roleId = user.IsAdmin ? 1 : 2;

            // Create new record
            var newUser = new User(
                        0,
                        user.FirstName,
                        user.LastName,
                        user.Username,
                        user.Email,
                        user.Password,
                        roleId,
                        sites.ToList(),
                        groups.ToList(),
                        null,
                        DateTime.Now,
                        DateTime.Now);
            
            Repository.AddUser(newUser);

            return newUser.Id;
        }

        /// <summary>
        ///     Remove user record from database.
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUserRecord(int id)
        {
            var user = GetSingleActiveUser(id);
            // Throw exception if not found
            if (user == null)
                throw new Exception("User not found id: " + id);

            // Set deleted property
            user.DeletedAt = DateTime.Now;
            // Update
            UpdateUserRecord(user);
        }

        /// <summary>
        ///     Get user record by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
        {
            return Repository.GetActiveUser(username);
        }

        /// <summary>
        ///     Get all users that have not been soft deleted.
        /// </summary>
        /// <returns></returns>
        public UserResponse GetAllActiveUsers()
        {
            var users = Repository.GetAllActiveUsersResponseBodies();

            return new UserResponse(users.Count(), users);
        }

        /// <summary>
        ///     Get a user that has not been deleted.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserResponseBody GetSingleActiveUser(int id)
        {
            // Get user
            var user = Repository.GetActiveUserResponseBody(id);
            // return null if not found
            if (user == null)
                return null;
            // Throw exception if 'soft' deleted
            if (user.DeletedAt != null)
                throw new Exception("User has been deleted!");

            return user;
        }

        /// <summary>
        ///     Get user by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserResponseBody GetSingleActiveUser(string username)
        {
            // Get user
            var user = Repository.GetActiveUserResponseBody(username);
            // return null if not found
            if (user == null)
                return null;
            // Throw exception if 'soft' deleted
            if (user.DeletedAt != null)
                throw new Exception("User has been deleted!");

            return user;
        }

        /// <summary>
        ///     TODO does not update sites or groups.
        /// </summary>
        /// <param name="userResponseBody"></param>
        public void UpdateUserRecord(UserResponseBody userResponseBody)
        {
            if (userResponseBody == null)
                throw new ArgumentException();

            var user = Repository.Get<User>(userResponseBody.Id);

            // Return null if not found
            if (user == null)
                throw new Exception("User not found");

            // Update the entity
            user.Update(userResponseBody);
            
            Repository.Update(user);
        }

        /// <summary>
        ///     Ensure users credentials are valid
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ApplicationUser ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(password))
                return null;

            // Find username in table
            User tuser = Repository.GetActiveUser(username);

            if (tuser == null)
                return null;

            if (UserPasswordIsValid(tuser, password))
            {
                var u = new ApplicationUser
                {
                    Username = username
                };

                if (tuser.Role.Name == "Admin")
                    u.Roles.Add("Admin");
                
                return u;
            }
            
            return null;
        }

        /// <summary>
        ///     Get group by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Group GetGroup(string name)
        {
            return Repository.GetGroup(name);
        }

        /// <summary>
        ///     Get list of all sites.
        /// </summary>
        /// <returns></returns>
        public List<Site> GetAllSites()
        {
            return Repository.GetAll<Site>()
                .ToList();
        }

        /// <summary>
        ///     Get site by primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Site GetSite(int id)
        {
            return Repository.Get<Site>(id);
        }

        /// <summary>
        ///     Get site by name.
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public Site GetSite(string siteName)
        {
            return Repository.GetSite(siteName);
        }

        #region Private Methods

        /// <summary>
        ///     Use the CryptSharp library to validate user password
        /// </summary>
        /// <param name="user">The user structure read from the database</param>
        /// <param name="password">Entered password</param>
        /// <returns>True if valid, false otherwise</returns>
        private bool UserPasswordIsValid(User user, string password)
        {
            try
            {
                return Crypter.CheckPassword(password, user.Password);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }

        /// <summary>
        ///     Get a list of group objects using their assoicated names.
        ///     Create new group if one does not exists.
        /// </summary>
        /// <param name="groupNames"></param>
        /// <returns></returns>
        private IEnumerable<Group> ToGroupList(string[] groupNames)
        {
            foreach (var group in groupNames)
            {
                // Get record from db
                var record = Repository.GetGroup(group);
                // Throw exception if not in data store
                if (record == null)
                    throw new Exception($"Group {group} does not exist");
                // Return list
                yield return record;
            }
        }

        #endregion
    }
}
