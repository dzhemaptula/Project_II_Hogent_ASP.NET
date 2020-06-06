using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.Domain {
    /// <summary>
    /// Class used as a bridge between User and Session to store Users registered for a Session. Attribute 'HasAttended' is used to store whether or not they have attended the Session.
    /// </summary>
    public class RegisteredUser {
        public int SessionId { get; set; }
        public Session Session { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public bool HasAttended { get; set; }

        public RegisteredUser(Session session, User user, bool hasAttended = false) {
            Session = session;
            SessionId = session.SessionId;
            User = user;
            UserId = user.UserId;
            HasAttended = hasAttended;
        }

        public RegisteredUser() {
        }
    }
}
