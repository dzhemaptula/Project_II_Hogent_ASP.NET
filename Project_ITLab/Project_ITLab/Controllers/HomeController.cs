using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project_ITLab.Data.IServices;
using Project_ITLab.Models;
using Project_ITLab.Models.Domain;
using Project_ITLab.Models.ViewModels;

namespace Project_ITLab.Controllers {
    public class HomeController : Controller {
        private readonly ISessionService SessionService;
        private readonly IUserService UserService;

        public HomeController(ISessionService sessionService, IUserService userService) {
            this.SessionService = sessionService;
            this.UserService = userService;
        }

        public IActionResult Index() {
            if (UserService.GetCurrentUser() == null) 
                return RedirectToAction("LogIn", "User");
            
            return View(new SessionsCollectionUserViewModel(SessionService.GetAll(), UserService.GetCurrentUser()));
        }



        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
