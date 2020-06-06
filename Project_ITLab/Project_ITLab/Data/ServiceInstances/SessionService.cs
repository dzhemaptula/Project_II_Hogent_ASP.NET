using Project_ITLab.Data.IServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Project_ITLab.Models.Exceptions;
using System.Diagnostics;
using Project_ITLab.Data;
using Project_ITLab.Data.ServiceInstances;

namespace Project_ITLab.Models.Domain {
    public class SessionService : ISessionService {
        private readonly Context context;
        private readonly DbSet<Session> Sessions;

        public SessionService(Context Context) {
            context = Context;
            Sessions = context.Sessions;
        }


        public IEnumerable<Session> GetByTime(DateTime startTime, DateTime endTime) {
            return Sessions
                .Include(s => s.Feedbacks).ThenInclude(s => s.User)
                .Include(s => s.Leaders).ThenInclude(s => s.User)
                .Include(s => s.SessionUsers).ThenInclude(s => s.User)//Include users
                .AsNoTracking()//Essentially unmodifiable DB-wise
                .Where(x => x.StartTime >= startTime && x.EndTime <= endTime)
                .ToList().AsReadOnly();
        }


        public IEnumerable<Session> GetAll() {
            //return new ReadOnlyCollection<Session>(new List<Session>(Sessions)).OrderBy(s => s.StartTime).ToList();
            return Sessions
                .Include(s => s.Feedbacks).ThenInclude(s => s.User)
                .Include(s => s.Leaders).ThenInclude(s => s.User)
                .Include(s => s.SessionUsers).ThenInclude(s => s.User)//Include users
                .AsNoTracking()//Essentially unmodifiable DB-wise
                .OrderBy(s => s.StartTime)//Order by StartTime first
                .ThenBy(s => s.Name)//Then alphabetically, should multiple sessions start at the same time
                .ToList().AsReadOnly();
        }

        public Session GetById(int id) {
            return Sessions
                .Include(s => s.Feedbacks).ThenInclude(s => s.User)
                .Include(s => s.Leaders).ThenInclude(s => s.User)
                .Include(s => s.SessionUsers).ThenInclude(s => s.User)//Include users
                .AsNoTracking()//Essentially unmodifiable DB-wise
                .FirstOrDefault(x => x.SessionId.Equals(id));
        }


        public void AttendSession(User user, int sessionId, bool overridden = false) {
            var session = Sessions.Include(s => s.SessionUsers).ThenInclude(s => s.User).FirstOrDefault(s => s.SessionId == sessionId) ??
                          throw new ArgumentException("Sessie werd niet gevonden.");
            session.Attend(user, overridden);

            Sessions.Update(session);

            context.SaveChanges();
        }

        public void RegisterForSession(User user, int sessionId) {
            var session = Sessions.Include(s => s.SessionUsers).ThenInclude(s => s.User).FirstOrDefault(s => s.SessionId == sessionId) ??
                         throw new ArgumentException("Sessie werd niet gevonden.");
            session.RegisterUser(user);

            Sessions.Update(session);

            context.SaveChanges();
        }

        public void UnregisterForSession(User user, int sessionId) {
            var session = Sessions.Include(s => s.SessionUsers).ThenInclude(s => s.User).FirstOrDefault(s => s.SessionId == sessionId) ??
                          throw new ArgumentException("Sessie werd niet gevonden.");
            session.UnregisterUser(user);

            Sessions.Update(session);

            context.SaveChanges();
        }


        public void OpenSession(int id) {
            var session = Sessions.FirstOrDefault(x => x.SessionId.Equals(id)) ?? throw new ArgumentException("That session could not be found!");

            if (session.EndTime < DateTime.Now)
                throw new NotPermittedException("Sessie is al gedaan.");

            if (session.IsAttendable())
                throw new NotPermittedException("Deze sessie is al open.");

            if (!session.IsJoinable())
                throw new NotPermittedException("Sessie kan niet opengezet worden.");

            if (Sessions.Any(s => s.Status == SessionStatus.Attendable))
                throw new NotPermittedException("Er is al een andere sessie bezig.");


            if ((session.StartTime - DateTime.Now).TotalMinutes > 60)
                throw new NotPermittedException("Sessie kan niet langer dan een uur voor de start opengezet worden.");


            session.Open();

            Sessions.Update(session);

            context.SaveChanges();
        }

        public void AddFeedback(int sessionId, User user, string text) {
            //var session = Sessions.SingleOrDefault(s => s.SessionId == sessionId)
            //              ?? throw new ArgumentException("Sessie werd niet gevonden.");
            var session = Sessions
                .Include(s => s.SessionUsers)//Include users
                .FirstOrDefault(x => x.SessionId.Equals(sessionId));

            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Feedback mag niet leeg zijn.");

            session.AddFeedback(user, text);

            Sessions.Update(session);

            context.SaveChanges();
        }

        public void CloseSession(int id) {
            var session = Sessions.FirstOrDefault(x => x.SessionId.Equals(id)) ?? throw new ArgumentException("That session could not be found!");

            if (DateTime.Now < session.StartTime)
                throw new NotPermittedException("Sessie kan niet voor de startuur gesloten worden.");

            if (session.IsFinished())
                throw new NotPermittedException("Sessie is al gedaan.");

            session.Close();

            Sessions.Update(session);

            context.SaveChanges();

        }

        public void StartSession(int id) {
            var session = Sessions.FirstOrDefault(x => x.SessionId.Equals(id)) ?? throw new ArgumentException("That session could not be found!");

            if (session.IsFinished())
                throw new NotPermittedException("Sessie is gesloten.");

            if (session.IsRunning())
                throw new NotPermittedException("Sessie is al gestart.");

            session.Start();

            Sessions.Update(session);

            context.SaveChanges();
        }

        public void DeactivateFeedback(int sessionId, int feedbackId) {

            var session = Sessions.FirstOrDefault(x => x.SessionId.Equals(sessionId)) ?? throw new ArgumentException("That session could not be found!");

            var feedback = session.Feedbacks.FirstOrDefault(s => s.FeedbackId == feedbackId);



        }
    }
}