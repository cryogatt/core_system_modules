using System.Collections.Generic;

namespace Infrastructure.Users.DTOs
{
    public class UserResponse
    {
        public UserResponse(int totalRecords, List<UserResponseBody> users)
        {
            this.TotalRecords = totalRecords;
            this.Users = users;
        }

        /// <summary>
        ///     Number of records.
        /// </summary>
        public int TotalRecords { get; set; }

        public List<UserResponseBody> Users { get; set; }
    }
}
