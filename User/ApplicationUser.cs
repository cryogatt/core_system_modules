using System.Collections.Generic;

namespace Infrastructure.Users
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {
            Roles = new List<string>();
        }

        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}