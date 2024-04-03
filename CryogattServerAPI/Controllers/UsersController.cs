using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using CryogattServerAPI.Trace;
using Infrastructure.Users.DTOs;

namespace CryogattServerAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : ApiController
    {
        public UsersController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        ///     GET: api/v1/Users
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetUsers()
        {
            Log.Debug("Invoked");

            try
            {
                // Return response
                return Ok(UnitOfWork.UserManager.GetAllActiveUsers());
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        ///     GET: api/v1/Users/id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [ResponseType(typeof(UserResponseBody))]
        public IHttpActionResult GetUsers(int uid)
        {
            Log.Debug("Invoked");

            try
            {
                // Query the database for specified user
                var user = UnitOfWork.UserManager.GetSingleActiveUser(uid);

                if (user == null)
                    return NotFound();

                // Return response
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        ///     PUT: api/Users/id (edited user).
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsers(int uid, UserResponseBody user)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }
            if (uid != user.Id)
            {
                Log.Error("Edited user ID does not match the ID in the URI.");
                return BadRequest("Edited user ID does not match the ID in the URI.");
            }

            try
            {
                // Verify user exists
                if (UnitOfWork.UserManager.GetSingleActiveUser(uid) == null)
                    return BadRequest("Failed to find user.");

                // Perform update
                UnitOfWork.UserManager.UpdateUserRecord(user);

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return InternalServerError();
            }
        }

        /// <summary>
        ///     DELETE api/Users
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteUser(List<int> users)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValid)
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            try
            {
                // Delete every user in list
                users.ForEach(id => UnitOfWork.UserManager.DeleteUserRecord(id));

                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest("Failed to remove user(s) from the database.");
            }
        }

        /// <summary>
        ///     POST: api/Users(new user).
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PostUser(UserResponseBody user)
        {
            Log.Debug("Invoked");

            if (!ModelState.IsValidField("Username") &&
                !ModelState.IsValidField("Password") &&
                !ModelState.IsValidField("FirstName") &&
                !ModelState.IsValidField("LastName"))
            {
                Log.Error(ModelState.ToString());
                return BadRequest("Invalid model state.");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Cannot create user with empty password.");
            }

            try
            {
                // Add user to database
                int id = UnitOfWork.UserManager.AddUser(user);
                // Check key is valid
                if (id == 0)
                    throw new Exception("An error occured adding user");
                // Get newly entered entity
                var urb = UnitOfWork.UserManager.GetSingleActiveUser(id);
                // Return the created user
                return CreatedAtRoute("DefaultApi", new { uid = urb.Id }, urb);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest("Failed to add user to the database.");
            }
        }
    }
}
