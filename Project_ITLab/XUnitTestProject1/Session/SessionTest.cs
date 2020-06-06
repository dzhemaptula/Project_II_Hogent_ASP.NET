//using Project_ITLab.Models.Domain;
//using Project_ITLab.Models.Exceptions;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace Project_ITLab_Test {
//    public class SessionTest {

//        private readonly User user = new User("test", "test");
//        private readonly DateTime start = DateTime.Now.AddDays(1);
//        private readonly DateTime end = DateTime.Now.AddDays(2);

//        [Fact(DisplayName = "Test proper initialization empty constructor; success")]
//        public void TestSessionEmptyConstructorMakesValidObject() {
//            var session = new Session();

//            Assert.NotNull(session);
//        }

//        [Fact(DisplayName = "Test constructor with valid parameters. Check initialization and proper setting; success")]
//        public void TestSessionParameterConstructorMakesValidObject() {

//            var session = new Session(1, user, start, end, "test", "testroom", 69);

//            Assert.NotNull(session);
//            Assert.Equal(1, session.SessionId);
//            Assert.Equal(0, session.Helpers.Count);
//            session.AddHelper(new User("test", "Test"));
//            Assert.Equal(1, session.Helpers.Count);
//            Assert.Equal(start, session.StartTime);
//            Assert.Equal(end, session.EndTime);
//            Assert.Equal("test", session.Name);
//            Assert.Equal("testroom", session.Room);
//            Assert.Equal(69, session.MaxAttendees);
//        }

//        [Fact(DisplayName = "Test constructor setting of default values; success")]
//        public void TestSessionValidParameterConstructionDefaultAttributes() {
//            var session = new Session(1, user, start, end, "test", "testroom", 69);

//            Assert.NotNull(session);
//            Assert.Equal(SessionStatus.Joinable, session.Status);
//            Assert.Equal(0, session.Attendees?.Count);
//            Assert.Equal(0, session.SessionUsers?.Count);
//        }

//        [Fact(DisplayName = "Test Add Leader Valid User Status; success")]
//        public void TestAddLeaderValidUserStatusReturnsTrue() {
//            var session = new Session(user, start, end, "test", "testroom", 69);
//            var tempuser = user;
//            tempuser.Status = UserStatus.Active;

//            Assert.True(session.AddHelper(user));
//        }

//        [Fact(DisplayName = "Test Add Leader Invalid User Status; fail")]
//        public void TestAddLeaderInvalidUserStatusReturnsFalse() {
//            var session = new Session(user, start, end, "test", "testroom", 69);
//            var tempuser = user;
//            tempuser.Status = UserStatus.Blocked;

//            Assert.False(session.AddHelper(user));
//        }

//        [Fact(DisplayName = "Test Register User Invalid User Status; fail")]
//        public void TestRegisterUserAlreadyRegisteredThrowsError() {
//            var session = new Session(1, user, start, end, "test", "testroom", 69);
//            session.RegisterUser(user);

//            Assert.Throws<NotPermittedException>(() => session.RegisterUser(user));
//        }

//        [Fact(DisplayName = "Test Register User Invalid User Status; fail")]
//        public void TestRegisterUserInvalidSessionStatusThrowsError() {
//            var session = new Session(1, user, start, end, "test", "testroom", 69);
//            session.Status = SessionStatus.Finished;

//            Assert.Throws<NotPermittedException>(() => session.RegisterUser(user));
//        }

//        [Fact(DisplayName = "Test Register User Session full; fail")]
//        public void TestRegisterUserFullSessionThrowsError() {
//            var session = new Session(1, user, start, end, "test", "testroom", 0);

//            Assert.Throws<NotPermittedException>(() => session.RegisterUser(user));
//        }

//        //#FIX Needs to be fixed, test delayed action
//        [Fact(DisplayName = "Test Open Automatically Start; success", Skip = "Tests needs fixing")]
//        public void TestOpenSessionAutomaticallyStartsSuccess() {
//            var session = new Session(1, user, DateTime.Now.AddSeconds(5), end, "test", "testroom", 0);
//            session.Start();
//            var first = session.Status;
//            Task.Run(async () => {
//                await Task.Delay(6000);
//                Assert.Equal(SessionStatus.Attendable, first);
//                Assert.Equal(SessionStatus.Running, session.Status);
//            });
//        }

//        [Fact(DisplayName = "Test Open Session Sets Status to Attendable")]
//        public void TestOpenSessionSetStatusToAttendableWorks() {

//            var session = new Session(1, user, DateTime.Now.AddSeconds(5), end, "test", "testroom", 0);
//            session.Status = SessionStatus.Joinable;//Technically not needed, just to be sure

//            session.Open();
//            Assert.NotNull(session.Timer);
//            Assert.Equal(SessionStatus.Attendable, session.Status);
//        }

//        [Fact(DisplayName = "Test Unregister User Valid User; success")]
//        public void TestUnregisterUserValidSuccess() {
//            var session = new Session(1, user, DateTime.Now.AddSeconds(5), end, "test", "testroom", 0);
//            session.SessionUsers.Add(user);
//            session.Status = SessionStatus.Joinable;

//            try {
//                session.UnregisterUser(user);
//            } catch (Exception e) {
//                Assert.True(false, e.Message);
//            }
//        }

//        [Fact(DisplayName = "Test Unregister User Invalid Status; fail")]
//        public void TestUnregisterUserInvalidStatusException() {
//            var session = new Session(1, user, DateTime.Now.AddSeconds(5), end, "test", "testroom", 0);
//            session.SessionUsers.Add(user);
//            session.Status = SessionStatus.Running;

//            Assert.Throws<NotPermittedException>(() => session.UnregisterUser(user));
//        }

//        [Fact(DisplayName = "Test UnregisterUser User not registered; fail")]
//        public void TestUnregisterUserNotRegisteredException() {
//            var session = new Session(1, user, DateTime.Now.AddSeconds(5), end, "test", "testroom", 0);

//            session.Status = SessionStatus.Joinable;

//            Assert.Throws<NotPermittedException>(() => session.UnregisterUser(user));
//        }

//        [Fact(DisplayName = "Test Attend Valid User")]
//        public void TestAttendsessionValidUserWorks()
//        {
//            var session= new Session(1, user, DateTime.Now.AddSeconds(5), end, "test", "testroom", 10){ Status = SessionStatus.Attendable};

//            var testuser=new User(){ Username = "nicklersberghe", Status = UserStatus.Active};

//            session.SessionUsers.Add(testuser);

//            session.Attend(testuser);

//            Assert.True(session.Attendees.Contains(testuser));


//        }

//    }
//}
