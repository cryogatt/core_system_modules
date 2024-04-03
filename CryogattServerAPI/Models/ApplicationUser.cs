using System.Collections.Generic;

namespace CryogattServerAPI.Models
{
    public class ApplicationUser
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }

        public ApplicationUser()
        {
            Roles = new List<string>();
        }
    }
}