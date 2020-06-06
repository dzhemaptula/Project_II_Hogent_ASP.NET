using Project_ITLab.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Project_ITLab.Models.Domain.DBClasses;

namespace Project_ITLab.Models.Domain
{
    public class Session
    {

        private string _name;
        private string _description;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Sessienaam is verplicht.");
                if (value.Length > 50)
                    throw new ArgumentException("Sessienaam mag maximaal 50 karakters lang zijn.");
                _name = value;
            }
        }
        public int SessionId { get; set; }
        public SessionStatus Status { get; set; }
        public string Room { get; set; }
        public string Speaker { get; set; }
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Beschrijving is verplicht.");

                _description = value;
            }
        }

        public IList<SessionLeader> Leaders { get; set; }
        public ICollection<RegisteredUser> SessionUsers { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }


        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int MaxAttendees { get; set; }

        public IObservable<long> Timer { get; private set; }

        public Session()
        {
            Leaders = new List<SessionLeader>();
            SessionUsers = new List<RegisteredUser>();
            Feedbacks = new List<Feedback>();
        }//Empty ctor for EF and test purposes

        public Session(int sessionId)
        {
            SessionId = sessionId;
            Leaders = new List<SessionLeader>();
            SessionUsers = new List<RegisteredUser>();
            Feedbacks = new List<Feedback>();
        }

        public Session(User leader, DateTime startTime, DateTime endTime, string name, string room, int maxAttendees = 100)
        {//#REMOVE no need for default max attendees, remove for production

            Status = SessionStatus.Joinable;

            Leaders = new List<SessionLeader> { new SessionLeader(this, leader, true) };
            SessionUsers = new List<RegisteredUser>();
            Feedbacks = new List<Feedback>();

            StartTime = startTime;
            EndTime = endTime;
            Name = name;
            Room = room;
            MaxAttendees = maxAttendees;

            Description = "In deze sessie wordt getoond hoe je een deftige ASP.NET MVC applicatie maakt. " +
                          "Hierbij wordt rekening gehouden met de wil van de klant en algemene Best Practice-principes.\n" +
                          "Dit is uiteindelijk ook maar demo-beschrijving. Geen van deze sessies bestaat en een beschrijving zou uiteindelijk geen betekenis hebben." +
                          "Het maakt wel niet zo veel uit, dit is ook maar geschreven om een voorbeeld te hebben om de looks & feels uit te kunnen testen.\n" +
                          "Persoonlijk lijkt het geslaagd. De look & feel van Chamilo werd overgenomen, terwijl de UX een stevige boost heeft gekregen.";
        }
        /// <summary>
        /// Registers a User. Checks if the User isn't registered already, the Status is Joinable and if the max amount hasn't been reached yet.
        /// </summary>
        /// <param name="user">User to be added</param>
        public void RegisterUser(User user)
        {

            if (user.Status != UserStatus.Active)//#Help Checken of status of niet?
                throw new NotPermittedException("Je staat niet op Actief. Vraag een hoofdverantwoordelijke om dit te veranderen.");

            if (SessionUsers.Any(s => s.User.Username == user.Username))
                throw new NotPermittedException("Je bent al ingeschreven voor deze sessie!");

            if (Status != SessionStatus.Joinable)
                throw new NotPermittedException("Je kan je niet meer inschrijven voor deze sessie!");

            if (SessionUsers.Count >= MaxAttendees)
                throw new NotPermittedException("Deze sessie zit helaas al vol.");


            SessionUsers.Add(new RegisteredUser(this, user));
        }
        /// <summary>
        /// Unregisters a User. Checks if the User is registered and the Status is Joinable or Attendable.
        /// </summary>
        /// <param name="user">User to be removed</param>
        public void UnregisterUser(User user)
        {

            var registered = SessionUsers.SingleOrDefault(s => s.User.Username == user.Username);

            if (registered == null)
                throw new NotPermittedException("Je bent niet ingeschreven voor deze sessie!");

            if (Status != SessionStatus.Joinable && Status != SessionStatus.Attendable)
                throw new NotPermittedException("Deze sessie is al aan de gang. Je kan je niet meer uitschrijven.");

            //if (DateTime.Now > DeadlineUnregister)
            //{
            //    throw new NotPermittedException("You cannot unregister after the deadline.");
            //}

            SessionUsers.Remove(registered);
        }


        public bool AddHelper(User helper)
        {
            if (helper.Status != UserStatus.Active)
                return false;

            Leaders.Add(new SessionLeader(this, helper));
            return true;
        }


        public void Attend(User person)
        {
            Attend(person, false);
        }

        public void Attend(User person, bool overridden)
        {
            if (Status != SessionStatus.Attendable && (Status != SessionStatus.Running && overridden))
            {
                if ((Status != SessionStatus.Running && overridden))//Extra check just to be able to have a better exception message
                    throw new NotPermittedException("Je bent te laat. Vraag de verantwoordelijke om handmatig je aanwezigheid op te nemen.");
                throw new NotPermittedException("Je kan je nu nog niet op aanwezig zetten.");
            }

            if (!SessionUsers.Any(s => s.User.Equals(person)))
                throw new NotPermittedException("Je bent niet ingeschreven voor deze sessie.");

            //#TODO testen op status persoon?
            SessionUsers.SingleOrDefault(s => s.User.Equals(person)).HasAttended = true;

        }

        public void AddFeedback(User user, string message)
        {

            var registered = SessionUsers.SingleOrDefault(s => s.User.Equals(user));

            if (Status != SessionStatus.Finished)
                throw new NotPermittedException("Feedback mag enkel worden toegevoegd als de sessie afgelopen is.");

            if (registered == null)
                throw new NotPermittedException("Je was niet ingeschreven voor deze sessie.");

            if (!registered.HasAttended)
                throw new NotPermittedException("Je was niet aanwezig.");




            Feedbacks.Add(new Feedback(this, user, message));
        }

        public void Start()
        {
            Status = SessionStatus.Running;

        }

        public void Open()
        {
            Status = SessionStatus.Attendable;
            Timer = Observable.Timer(StartTime);
            Timer.Subscribe(s => Start());
        }

        public void Close()
        {
            Status = SessionStatus.Finished;

            var notpresent = SessionUsers.Where(s => !s.HasAttended);

            foreach (var user in notpresent)
                user.User.IncrementTimesAbsent();
        }

        /// <summary>
        /// Checks if provided User is the Admin or a Helper for this Session.
        /// </summary>
        /// <param name="user">User to check</param>
        /// <returns>True if User is Admin or Helper</returns>
        public bool HasLeaderOrHelper(User user)
        {
            return Leaders
                .Any(s => s
                    .User
                    .Equals(user));
        }
        public bool IsAttendable()
        {
            return Status == SessionStatus.Attendable;
        }

        public bool IsJoinable()
        {
            return Status == SessionStatus.Joinable;
        }

        public bool IsRunning()
        {
            return Status == SessionStatus.Running;
        }

        public bool IsFinished()
        {
            return Status == SessionStatus.Finished;
        }

        public bool HasRegisteredUser(User user)
        {
            return SessionUsers.Any(item => item.User.Equals(user));
        }

        /// <summary>
        /// Returns a human-readable list of Session leaders, session owner first, then ordered by name. Separates each name by a comma and whitespace.
        /// </summary>
        /// <returns>List of Sessionleader names</returns>
        public string GetLeadersToString()
        {
            var s = "";
            var isNotFirst = false;
            var leaders = Leaders.OrderByDescending(s => s.IsSessionLeader).ThenBy(s => s.User.Username);
            foreach (var leader in leaders)
            {
                if (isNotFirst)
                    s = s + ", ";//Add comma and whitespace before each leader if its not the first
                isNotFirst = true;
                s = s + leader.User.GetFullName();
            }
            return s;
        }
    }
    public enum SessionStatus
    {
        Joinable,
        Attendable,
        Running,
        Finished
    }
}
