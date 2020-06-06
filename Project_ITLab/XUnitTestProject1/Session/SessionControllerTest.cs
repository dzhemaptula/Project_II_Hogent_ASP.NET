using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Project_ITLab.Controllers;
using Project_ITLab.Data.IServices;
using Project_ITLab.Models.Domain;
using Project_ITLab.Models.ViewModels;
using Xunit;

namespace Project_ITLab_Test {
    public class SessionControllerTest {

        private Mock<ISessionService> sessionMock;
        private Mock<IUserService> userMock;

        private ICollection<Session> sessions;
        private User user;

        public SessionControllerTest() {
            SetupMockData();
            SetupMock();
        }



        //[Fact(DisplayName = "Test Index No User Logged in")]
        //public void TestIndexNoUserLoggedInRedirectsToAction() {
        //    SetupMockNoUser();
        //    var controller = new SessionController(sessionMock.Object, userMock.Object);
        //    var result = (RedirectToActionResult)controller.Index();

        //    Assert.Equal("LogIn", result.ActionName);
        //    Assert.Equal("User", result.ControllerName);
        //    userMock.Verify(s => s.GetCurrentUser());
        //}

        //[Fact(DisplayName = "Test Index User Logged in")]
        //public void TestIndexUserLoggedInReturnsView() {
        //    SetupMockWithUser();
        //    var controller = new SessionController(sessionMock.Object, userMock.Object);
        //    var result = controller.Index() as ViewResult;
        //    var model = (SessionsCollectionUserViewModel)result?.Model;
        //    //Assert.Equal("Session", result.ViewName);
        //    Assert.Equal(user, model?.User);
        //    Assert.Equal(2, model.Sessions.Count());

        //    userMock.Verify(s => s.GetCurrentUser());
        //    sessionMock.Verify(s => s.GetAll());
        //}

        //[Fact(DisplayName = "Test Details Session 0 Returns Session")]
        //public void TestDetailsReturnsView() {
        //    SetupMockWithUser();
        //    var controller = new SessionController(sessionMock.Object, userMock.Object);
        //    var result = controller.Details(0) as ViewResult;
        //    var model = result.Model as SessionUserViewModel;

        //    Assert.Equal(user, model.User);
        //    Assert.Equal(sessions.First(), model.Session);

        //    userMock.Verify(s => s.GetCurrentUser());
        //    sessionMock.Verify(s => s.GetById(0));
        //}



        //[Fact(DisplayName = "Test Register Session 0 Returns View")]
        //public void TestRegisterValidSessionReturnsView() { 
        //    var controller = new SessionController(sessionMock.Object, userMock.Object);
        //    var result = controller.Register(0) as ViewResult;
            

        //    Assert.NotNull(result);
        //    Assert.Equal("Test1", result.ViewData["Name"]);

        //    sessionMock.Verify(s => s.GetById(0));
        //}



        [Fact(DisplayName = "Test Register Session 2 Returns NotFound")]
        public void TestRegisterInvalidSessionReturnsNotFound() {
            var controller = new SessionController(sessionMock.Object, userMock.Object);
            var result = controller.Register(2);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
           

            sessionMock.Verify(s => s.GetById(2));
        }




        //[Fact(DisplayName = "Test Unregister Session 0 Returns View")]
        //public void TestUnregisterValidSessionReturnsView() {
        //    var controller = new SessionController(sessionMock.Object, userMock.Object);
        //    var result = controller.Unregister(0) as ViewResult;


        //    Assert.NotNull(result);
        //    Assert.Equal("Test1", result.ViewData["Name"]);

        //    sessionMock.Verify(s => s.GetById(0));
        //}



        [Fact(DisplayName = "Test Unregister Session 2 Returns NotFound")]
        public void TestUnregisterInvalidSessionReturnsNotFound() {
            var controller = new SessionController(sessionMock.Object, userMock.Object);
            var result = controller.Unregister(2);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);


            sessionMock.Verify(s => s.GetById(2));
        }



        //#HELP TempData mocken voor tests



        #region Setup

        private void SetupMockData() {
            sessions = new List<Session>();
            sessions.Add(new Session( new User("Dzhem", "Aptula"), new DateTime(2020, 2, 20, 20, 0, 0), new DateTime(2020, 2, 20, 21, 0, 0), "Test1", "B2014"));
            sessions.Add(new Session( new User("Nick", "Lersberghe"), new DateTime(2020, 2, 21, 20, 0, 0), new DateTime(2020, 2, 21, 21, 0, 0), "Test2", "D3001"));
            user = new User();
        }

        private void SetupMock() {
            sessionMock = new Mock<ISessionService>();
            sessionMock.Setup(repo => repo.GetAll()).Returns(sessions);
            sessionMock.Setup(s => s.GetById(0)).Returns(sessions.First());
            sessionMock.Setup(s => s.GetById(2)).Returns((Session)null);

            //serviceMock.Setup(s=> s.GetByTime())

            userMock = new Mock<IUserService>();


        }

        private void SetupMockNoUser() {
            userMock.Setup(s => s.GetCurrentUser()).Returns((User)null);
        }

        private void SetupMockWithUser() {
            userMock.Setup(s => s.GetCurrentUser()).Returns(user);
        }

        #endregion

    }




}
