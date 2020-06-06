using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_ITLab.Models.Domain;
using Project_ITLab.Models.Exceptions;

namespace Project_ITLab.Data.IServices {
    public interface ISessionService {
        /// <summary>
        /// Returns all sessions.
        /// </summary>
        /// <returns>All sessions</returns>
        IEnumerable<Session> GetAll();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IEnumerable<Session> GetByTime(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Returns Session with the given id, null if none found.
        /// </summary>
        /// <param name="id">Id of the Session</param>
        /// <returns>Session, null if not found</returns>
        Session GetById(int id);

        /// <summary>
        /// Sets User as Attending the given Session.
        /// </summary>
        /// <param name="user">User that wants to attend</param>
        /// <param name="sessionId">Id of the Session</param>
        /// <param name="overridden">When the administrator manually adds a User. Default false when omitted</param>
        void AttendSession(User user, int sessionId, bool overridden = false);

        /// <summary>
        /// Sets a User as Registered for the given Session.
        /// </summary>
        /// <param name="user">User to register</param>
        /// <param name="sessionId">Id of the session</param>
        void RegisterForSession(User user, int sessionId);


        /// <summary>
        /// Sets a User as not Registered for the given Session.
        /// </summary>
        /// <param name="user">User to register</param>
        /// <param name="sessionId">Id of the session</param>
        void UnregisterForSession(User user, int sessionId);

        /// <summary>
        /// Opens a Session and sets the Status to Attendable. Allows Users to register as Attending.
        /// </summary>
        /// <param name="id">Id of the Session</param>
        void OpenSession(int id);

        /// <summary>
        /// Adds Feedback to the given session.
        /// </summary>
        /// <param name="sessionId">ID of the session</param>
        /// <param name="user">User giving feedback</param>
        /// <param name="text">Text of the feedback</param>
        void AddFeedback(int sessionId, User user, string text);

        /// <summary>
        /// Sets the given Feedback is inactive
        /// </summary>
        /// <param name="sessionId">ID of the session</param>
        /// <param name="feedbackId">ID of the feedback</param>
        void DeactivateFeedback(int sessionId, int feedbackId);

        void CloseSession(int id);

        void StartSession(int id);
    }
}
