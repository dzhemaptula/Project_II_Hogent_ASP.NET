using Project_ITLab.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.ViewModels {
    public class SessionsCollectionUserViewModel {
        public IEnumerable<Session> Sessions { get; }
        public User? User { get; }
        public int Status { get; set; }
        //Status 0 = all sessions
        //Status 1 = Registered sessions
        //Status 2 = non-Registered sessions
        //Status 3 = Owned sessions
        //Status 4 = upcomming sessions

        public List<IGrouping<DateTime, Session>> GetAll()
        {
            return GroupSessions(Sessions);
        }
        public List<IGrouping<DateTime, Session>> GetRegistered() {
            IEnumerable<Session> ungroupedSessions = User == null ? Sessions : Sessions.Where(s => s.SessionUsers.Any(s => s.User.Equals(User))).ToList().AsReadOnly();

            return GroupSessions(ungroupedSessions);
        }
        public List<IGrouping<DateTime, Session>> GetNotRegistered() {
            IEnumerable<Session> ungroupedSessions = User == null ? Sessions : Sessions.Where(s => !s.SessionUsers.Any(s => s.User.Equals(User))).ToList().AsReadOnly();

            return GroupSessions(ungroupedSessions);
        }

        public List<IGrouping<DateTime, Session>> GetOwned()
        {
            IEnumerable<Session> ungroupedSessions = User == null ? Sessions : Sessions.Where(s => s.HasLeaderOrHelper(User)).ToList().AsReadOnly();

            return GroupSessions(ungroupedSessions);
        }

        public List<IGrouping<DateTime, Session>> GetUpcomming()
        {
            IEnumerable<Session> ungroupedSessions = Sessions.Where(s => s.StartTime >DateTime.Now).ToList().AsReadOnly();

            return GroupSessions(ungroupedSessions);
        }
        public SessionsCollectionUserViewModel(IEnumerable<Session> sessions, User user, int id = 0) {
            Sessions = sessions;
            User = user;
            Status = id;
        }

        public List<IGrouping<DateTime,Session>> GroupSessions(IEnumerable<Session> ungrouped)
        {
            List<IGrouping<DateTime, Session>> res = ungrouped.GroupBy(g => g.StartTime.Date).ToList();

            return res;
        }
    }
}
