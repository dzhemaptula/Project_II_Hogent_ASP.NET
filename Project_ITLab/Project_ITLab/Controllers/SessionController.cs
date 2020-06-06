using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_ITLab.Data.IServices;
using Project_ITLab.Models;
using Project_ITLab.Models.Domain;
using Project_ITLab.Models.Exceptions;
using Project_ITLab.Models.ViewModels;

namespace Project_ITLab.Controllers {
    [Authorize]
    public class SessionController : Controller {
        private readonly ISessionService SessionService;
        private readonly IUserService UserService;

        public SessionController(ISessionService sessionService, IUserService userService) {
            this.SessionService = sessionService;
            this.UserService = userService;
        }
        /// <summary>
        /// Shows session calendar.
        /// </summary>
        /// <param name="id">State. 0= is all sessions, 1= Registered, 2= Not Registered</param>
        /// <returns>View with all Sessions</returns>
        [AllowAnonymous]
        public IActionResult Index(int id = 0) {

            //FIXED
            //if (UserService.GetCurrentUser() == null)
            //    return RedirectToAction("LogIn", "User");
            //var model = SessionService.GetAll();
            var username = User.Identity.Name;
            User user = null;
            if (username != null) {
                user = UserService.GetByUsername(username);

            }



            return View(new SessionsCollectionUserViewModel(SessionService.GetAll(), user, id));
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Shows the detail screen for a single session
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        [AllowAnonymous]
        public IActionResult Details(int id) {

            var username = User.Identity.Name;
            User user = null;
            if (username != null)
                user = UserService.GetByUsername(username);

            var session = SessionService.GetById(id);

            if (session.IsAttendable()||session.IsRunning())
                if (user != null && session.HasLeaderOrHelper(user))
                    return RedirectToAction("Attend", "Session", new { id });

            var sessionUser = new SessionUserViewModel(session, user);
            return View(sessionUser);
        }

        /// <summary>
        /// Goes to the registration confirmation screen.
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        public IActionResult Register(int id) {
            var session = SessionService.GetById(id);
            if (session == null)
                return NotFound();
            ViewData[nameof(session.Name)] = session.Name;
            return RegisterConfirmed(id);
        }

        /// <summary>
        /// Confirms the registration.
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        [HttpPost, ActionName("Register")]
        public IActionResult RegisterConfirmed(int id) {
            try {
                var username = User.Identity.Name;
                User user = null;
                if (username != null)
                    user = UserService.GetByUsername(username);

                Session session = SessionService.GetById(id);

                if (SessionService.GetById(id) == null)//Technically redundant, always be ware for nullpointers.
                    throw new ArgumentException("De sessie werd niet gevonden. Contacteer een hoofdverantwoordelijke als dit probleem zich blijft voordoen.");

                if (session.HasLeaderOrHelper(user))
                    return RedirectToAction(nameof(Details), new { id });


                SessionService.RegisterForSession(user, id);

                TempData["message"] = $"Je bent geregistreerd door sessie {session.Name}.";
            } catch (Exception e) {
                TempData["error"] = e.Message;
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// Goes to the Start confirmation page
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        [Authorize(Policy = "Admins")]
        public IActionResult Start(int id)
        {
            Session session = SessionService.GetById(id);
            if (session == null)
                return NotFound();
            ViewData[nameof(session.Name)] = session.Name;
            ViewData["startTime"] = session.StartTime.ToString("HH:mm");
            return View();
        }

        /// <summary>
        /// Confirms the opening of the session
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Redirect to Attend page</returns>
        [Authorize(Policy = "Admins")]
        [HttpPost, ActionName("Start")]
        public IActionResult StartConfirmed(int id)
        {
            //#TODO
            //controle
            try
            {
                SessionService.StartSession(id);
                TempData["message"] = $"De sessie werd succesvol gestart.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction(nameof(Attend), new { id });
        }

        /// <summary>
        /// Goes to the Open confirmation page
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        [Authorize(Policy = "Admins")]
        public IActionResult Open(int id) {
            Session session = SessionService.GetById(id);
            if (session == null)
                return NotFound();
            ViewData[nameof(session.Name)] = session.Name;
            ViewData["startTime"] = session.StartTime.ToString("HH:mm");
            return View();
        }

        /// <summary>
        /// Confirms the opening of the session
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Redirect to Attend page</returns>
        [Authorize(Policy = "Admins")]
        [HttpPost, ActionName("Open")]
        public IActionResult OpenConfirmed(int id) {
            //#TODO
            //controle
            try {
                SessionService.OpenSession(id);
                TempData["message"] = $"De sessie werd succesvol opengesteld.";
            } catch (Exception e) {
                TempData["error"] = e.Message;
            }
            return RedirectToAction(nameof(Attend), new { id });
        }

        /// <summary>
        /// Goes to the Close confirmation page
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        [Authorize(Policy = "Admins")]
        public IActionResult Close(int id)
        {
            Session session = SessionService.GetById(id);
            if (session == null)
                return NotFound();
            ViewData[nameof(session.Name)] = session.Name;
            ViewData["endTime"] = session.EndTime.ToString("HH:mm");
            return View();
        }

        /// <summary>
        /// Confirms the closing of the session
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Redirect to Attend page</returns>
        [Authorize(Policy = "Admins")]
        [HttpPost, ActionName("Close")]
        public IActionResult CloseConfirmed(int id)
        {
            //#TODO
            //controle
            try
            {
                SessionService.CloseSession(id);
                TempData["message"] = $"De sessie werd succesvol gelsoten.";
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// Shows the Attend page
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        [Authorize(Policy = "Admins")]
        public IActionResult Attend(int id) {
            var username = User.Identity.Name;
            User user = null;
            if (username != null)
                user = UserService.GetByUsername(username);

            Session currentSession = SessionService.GetById(id);

            //if (currentUser == null)
            //    return RedirectToAction("LogIn", "User");

            if (!currentSession.HasLeaderOrHelper(user))
                return NotFound();

            return View(new AttendViewModel(currentSession, user));

        }
        /// <summary>
        /// Sets the given person as Attending
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <param name="avm">Model containing user data</param>
        /// <returns>Returns to the Attend page</returns>
        [Authorize(Policy = "Admins")]
        [HttpPost, ActionName("Attend")]
        public IActionResult AttendPerson(int id, AttendViewModel avm)  {
            User attendingUser = null;
            try {
                Session sessionToAttend = SessionService.GetById(id);
                if (SessionService.GetById(id) == null)
                    throw new ArgumentException("De sessie werd niet gevonden. Contacteer een hoofdverantwoordelijke als dit probleem zich blijft voordoen.");

                if (avm.CardNumber == null)
                {
                    
                    attendingUser = UserService.GetByUsername(avm.Username);
                    
                }
                else
                {
                    attendingUser = UserService.GetByCardNumber(avm.CardNumber);
                }
                SessionService.AttendSession(attendingUser, id, true);

            } catch (Exception e) {
                TempData["error"] = e.Message;
            }

            return RedirectToAction(nameof(Attend), new { id });
        }

        /// <summary>
        /// Shows the Unregister confirmation page
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        public IActionResult Unregister(int id) {
            Session session = SessionService.GetById(id);
            if (session == null)
                return NotFound();
            ViewData[nameof(session.Name)] = session.Name;
            return UnregisterConfirmed(id);
        }

        /// <summary>
        /// Confirms the unregistration of the User, shows Details page
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <returns>Page</returns>
        [HttpPost, ActionName("Unregister")]
        public IActionResult UnregisterConfirmed(int id) {
            //#TODO
            //controle check if user is in unregistered
            try {

                var username = User.Identity.Name;
                User user = null;
                if (username != null)
                    user = UserService.GetByUsername(username);

                Session session = SessionService.GetById(id);
                if (SessionService.GetById(id) == null)
                    throw new ArgumentException("De sessie werd niet gevonden. Contacteer een hoofdverantwoordelijke als dit probleem zich blijft voordoen.");

                SessionService.UnregisterForSession(user, id);
                TempData["message"] = $"Je hebt je succesvol uitgeschreven voor {session.Name}.";
            } catch (Exception e) {
                TempData["error"] = e.Message;
            }
            return RedirectToAction(nameof(Details), new { id });
        }


        /// <summary>
        /// Adds feedback to the session. 
        /// </summary>
        /// <param name="id">ID of the session</param>
        /// <param name="svm">Viewmodel containing text, etc</param>
        /// <returns>Redirect to the Detail page for the session</returns>
        [HttpPost, ActionName("AddFeedback")]
        public IActionResult AddFeedbackConfirmed(int id, SessionUserViewModel svm) {

            try {
                User user = UserService.GetByUsername(User.Identity.Name);
                SessionService.AddFeedback(id, user, svm.FeedbackText);
                TempData["feedback"] = "Wij hebben je feedback goed ontvangen en zullen ze zo snel mogelijk verwerken.";
            } catch (Exception e) {
                Console.WriteLine(e);
                TempData["error"] = e.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

    }
}
