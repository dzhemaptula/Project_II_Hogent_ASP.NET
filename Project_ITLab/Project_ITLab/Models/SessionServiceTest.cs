//using Project_ITLab.Models.Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Project_ITLab.Models.Exceptions;
//using Xunit;

//namespace Project_ITLab_Test {
//    public class SessionServiceTest {


//        private ICollection<Session> testSessions = new List<Session>();
//        private ICollection<Session> testSessionsOpen = new List<Session>();
//        private ICollection<Session> mockSessions = new List<Session>();

//        private readonly User user = new User { Status = UserStatus.Active };


//        public SessionServiceTest() {
//            SetupSessions();
//        }

//        private void SetupSessions() {

//            #region Register attendance
//            //Sessions used to test AttendSession
//            testSessions.Add(new Session(0) { Status = SessionStatus.Attendable });//Valid
//            testSessions.Add(new Session(1) { Status = SessionStatus.Finished });//Invalid
//            testSessions.Add(new Session(2) { Status = SessionStatus.Joinable });//Invalid
//            testSessions.Add(new Session(3) { Status = SessionStatus.Running });//Valid with override
//            #endregion

//            #region Open session
//            //Sessions used to test Open Session, contains test cases
//            testSessionsOpen.Add(new Session(4) { Status = SessionStatus.Joinable, StartTime = DateTime.Now.AddMinutes(30), EndTime = DateTime.Now.AddHours(1) });//Valid
//            testSessionsOpen.Add(new Session(5) { Status = SessionStatus.Attendable, StartTime = DateTime.Now.AddMinutes(30), EndTime = DateTime.Now.AddHours(1) });//Invalid, Status
//            testSessionsOpen.Add(new Session(6) { Status = SessionStatus.Running, StartTime = DateTime.Now.AddMinutes(30), EndTime = DateTime.Now.AddHours(1) });//Invalid, Status
//            testSessionsOpen.Add(new Session(7) { Status = SessionStatus.Finished, StartTime = DateTime.Now.AddMinutes(30), EndTime = DateTime.Now.AddHours(1) });//Invalid, Status
//            testSessionsOpen.Add(new Session(8) { Status = SessionStatus.Joinable, StartTime = DateTime.Now.AddHours(2), EndTime = DateTime.Now.AddHours(3) });//Invalid, Starttime
//            testSessionsOpen.Add(new Session(9) { Status = SessionStatus.Joinable, StartTime = DateTime.Now.AddHours(-2), EndTime = DateTime.Now.AddHours(-1) });//Invalid, Endtime
//            #endregion

//            #region Valid sessions
//            mockSessions.Add(new Session(10) { Status = SessionStatus.Joinable, StartTime = DateTime.Now.AddMinutes(30), EndTime = DateTime.Now.AddHours(1) });//Valid
//            mockSessions.Add(new Session(11) { Status = SessionStatus.Joinable, StartTime = DateTime.Now.AddMinutes(30), EndTime = DateTime.Now.AddHours(1) });//Valid
//            mockSessions.Add(new Session(12) { Status = SessionStatus.Joinable, StartTime = DateTime.Now.AddMinutes(30), EndTime = DateTime.Now.AddHours(1) });//Valid
//            mockSessions.Add(new Session(13) { Status = SessionStatus.Joinable, StartTime = DateTime.Now.AddMinutes(30), EndTime = DateTime.Now.AddHours(1) });//Valid
//            #endregion
//        }


//        [Fact(DisplayName = "Test Attend Session User Registered Valid Session")]
//        public void TestAttendSessionValidUserValidSessionWorks() {

//            var service = new SessionService();
//            service.Sessions = new List<Session>(testSessions);

//            var session = service.Sessions.FirstOrDefault(s => s.SessionId == 0);
//            session?.SessionUsers.Add(new RegisteredUser(session, user));

//            service.AttendSession(user, 0);



//            Assert.NotNull(session);
//            //Assert.True(session.SessionUsers.Any(s=> s.User.Equals(s)));
//            Assert.Contains(session.SessionUsers, s => s.User.Equals(user));
//        }


//        [Fact(DisplayName = "Test Attend Session User Not Registered Valid Session Throws Exception")]
//        public void TestAttendSessionUserNotRegisteredValidSessionThrowsException() {

//            var service = new SessionService();
//            service.Sessions = new List<Session>(testSessions);

//            Assert.Throws<NotPermittedException>(() => service.AttendSession(user, 2));
//        }


//        [Theory(DisplayName = "Test Attend Session Invalid User Valid Session Throws Exception")]
//        [InlineData(1)]
//        [InlineData(2)]
//        [InlineData(3)]
//        public void TestAttendSessionUserRegisteredInvalidSessionNoAdminOverrideThrowsException(int id) {

//            var service = new SessionService();
//            service.Sessions = new List<Session>(testSessions);

//            var session = service.Sessions.FirstOrDefault(s => s.SessionId == id);

//            session?.SessionUsers.Add(new RegisteredUser(session, user));

//            Assert.Throws<NotPermittedException>(() => service.AttendSession(user, id, false));
//        }

//        //No session open
//        //Not open yet
//        //Minder dan een uur op voorhand
//        //#HELP openzetten na aanvang?
//        //Niet meer openzetten na einduur
//        [Theory(DisplayName = "Test Open Session Invalid Session Throws Exception.")]
//        [InlineData(5)]
//        [InlineData(6)]
//        [InlineData(7)]
//        [InlineData(8)]
//        [InlineData(9)]
//        public void TestOpenSessionInvalidSessionThrowsNotPermittedException(int id) {
//            var service = new SessionService { Sessions = new List<Session>(mockSessions) };

//            var session = testSessionsOpen.First(s => s.SessionId == id);
//            service.Sessions.Add(session);

//            Assert.Throws<NotPermittedException>(() => service.OpenSession(session.SessionId));
//        }


//        [Fact(DisplayName = "Test Open Session Valid Session No Sessions Open Yet Works fine")]
//        public void TestOpenSessionValidSessionNoSessionOpenYetWorks() {
//            var service = new SessionService { Sessions = testSessionsOpen };

//            var session = testSessionsOpen.First(s => s.SessionId == 4);

//            service.OpenSession(4);

//            Assert.Equal(SessionStatus.Attendable, session.Status);
//        }


//        [Fact(DisplayName = "Test Open Session Valid Session Session Already Open Throws Exception")]
//        public void TestOpenSessionValidSessionSessionAlreadyOpenThrowsException() {

//            var service = new SessionService { Sessions = mockSessions };

//            service.Sessions.Add(
//                new Session(5)
//                {
//                    Status = SessionStatus.Attendable,
//                    StartTime = DateTime.Now.AddMinutes(30),
//                    EndTime = DateTime.Now.AddHours(1)
//                }
//                );

//            Assert.Throws<NotPermittedException>(() => service.OpenSession(10));

//        }

//    }
//}
