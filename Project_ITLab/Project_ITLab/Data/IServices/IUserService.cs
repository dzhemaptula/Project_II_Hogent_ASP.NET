using Project_ITLab.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Data.IServices
{
    public interface IUserService
    {
        /// <summary>
        /// Returns all users in the system
        /// </summary>
        /// <returns>Collection of users</returns>
        IEnumerable<User> GetAll();

        /// <summary>
        /// Returns user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        User GetByUsername(string username);

        User GetByCardNumber(string cardnumber);
        void LogIn(string username);

        User GetCurrentUser();
        void LogOut();
    }
}
