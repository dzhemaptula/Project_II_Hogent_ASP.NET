using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.Domain.DBClasses {
    /// <summary>
    /// Class used as a bridge between User and Session to store Users that are the leader or helper for a Session. Attribute 'IsSessionLeader' is used to store whether or not they are the primary leader of the Session.
    /// </summary>
    public class SessionLeader {
        public Session Session { get; set; }
        public int SessionId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public bool IsSessionLeader { get; set; }

        public SessionLeader(Session session, User user, bool isLeader = false) {
            Session = session;
            SessionId = session.SessionId;
            User = user;
            UserId = user.UserId;
            IsSessionLeader = isLeader;
        }

        public SessionLeader() {
        }
    }
}
