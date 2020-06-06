using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.Domain
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }

        public Session Session { get; set; }
        public int SessionId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }


        public Announcement(Session session, User user, string message)
        {
            Session = session;
            SessionId = session.SessionId;
            User = user;
            UserId = user.UserId;
            Message = message;
            Date = DateTime.Now;
        }
        public Announcement() { }
    }
}
