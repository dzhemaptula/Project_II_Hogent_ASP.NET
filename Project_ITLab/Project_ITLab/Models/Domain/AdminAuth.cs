using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_ITLab.Models.Enums;

namespace Project_ITLab.Models.Domain
{
    public class AdminAuth
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public UserRole UserRole { get; set; }

        public AdminAuth()
        {
        }

        public AdminAuth(string username, string hashedPassword, UserRole role)
        {
            this.Username = username;
            this.HashedPassword = hashedPassword;
            this.UserRole = role;
        }

    }
}
