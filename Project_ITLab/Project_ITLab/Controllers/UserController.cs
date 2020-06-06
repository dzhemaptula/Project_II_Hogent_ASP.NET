using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_ITLab.Data.IServices;
using Project_ITLab.Models.Exceptions;
using Project_ITLab.Models.ViewModels;

namespace Project_ITLab.Controllers {
    //[Authorize]
    public class UserController : Controller {
        private readonly IUserService UserService;
        private readonly SignInManager<IdentityUser> signInManager;

        public UserController(IUserService userService, SignInManager<IdentityUser> signIn) {
            this.UserService = userService;
            signInManager = signIn;
        }

        [Authorize]
        public IActionResult Login(string actionName, int? id) {
            return id == null ? RedirectToAction(actionName, "Session") : RedirectToAction(actionName, "Session", new { id });
        }

        public async Task<IActionResult> LogOut() {
            await signInManager.SignOutAsync();
            TempData["message"] = "Succesfully logged out";
            return RedirectToAction("Index", "Session");
        }
    }
}