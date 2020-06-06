using Project_ITLab.Data.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.ViewModels
{
    public class SessionUserServiceViewModel
    {
        public ISessionService SessionService { get; }
        public IUserService UserService { get; }
        public SessionUserServiceViewModel(ISessionService sessionService, IUserService userService)
        {
            SessionService = sessionService;
            UserService = userService;
        }
    }
}
