using CLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    // This class acts as my database storing static values
    public class DbService
    {
        private List<UserDTO> Users { get; set; }

        public DbService()
        {
            Users = new List<UserDTO>();
            // Adding user objects to list
            Users.Add(new UserDTO { Username = "Mads", Password = "Admin", UID = "Mads123456"});
            Users.Add(new UserDTO { Username = "Peter", Password = "Admin", UID = "Peter123456"});
            Users.Add(new UserDTO { Username = "Daniel", Password = "Admin", UID = "Daniel123456"});
        }

        public async Task<UserDTO> GetOneUser(string username)
        {
            return Users.FirstOrDefault(u => u.Username == username);
        } 
    }
}
