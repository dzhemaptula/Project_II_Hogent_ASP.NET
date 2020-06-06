using Project_ITLab.Data.IServices;
using Project_ITLab.Models.Domain;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Project_ITLab.Models.Exceptions;

namespace Project_ITLab.Data.ServiceInstances {
    public class UserService : IUserService {

        private readonly Context context;
        private readonly DbSet<User> Users;
        private static User CurrentUser { get; set; }//Remove when auth is fixed

        //temporary
        //nee toch nie, tis gucci for now
        public UserService(Context Context) {
            context = Context;
            Users = context.Users;
            //CurrentUser = GetByUsername("tijlzwartjes"); //remove vanaf dat alles weer werkt
        }

        public void LogIn(string username) {//#bruv check slides voor authentication, dees is nie veilig en gaat ook nie werken wanneer da het voor X aantal mensen draait

            if (string.IsNullOrEmpty(username))
                throw new NotPermittedException("Please provide a username");

            //indien username van bepaald formaat is(x aantal letters/cijfers bvb) toevoegen.

            CurrentUser = GetByUsername(username) ?? throw new NotPermittedException("Wrong login");
            //if (CurrentUser == null) { throw new NotPermittedException("Wrong login"); }
        }


        public IEnumerable<User> GetAll() {
            //return new ReadOnlyCollection<User>(new List<User>(Users));
            return Users.AsNoTracking().ToList().AsReadOnly();
        }

        public User GetByUsername(string username) {
            return Users.AsNoTracking().FirstOrDefault(x => x.Username.Equals(username.ToLower()));
        }

        public User GetCurrentUser() {
            return CurrentUser;
        }

        public void LogOut() {
            CurrentUser = null;
        }

        public User GetByCardNumber(string cardnumber)
        {
            return Users.AsNoTracking().FirstOrDefault(x => x.CardNumber.Equals(cardnumber));
        }
    }
}
