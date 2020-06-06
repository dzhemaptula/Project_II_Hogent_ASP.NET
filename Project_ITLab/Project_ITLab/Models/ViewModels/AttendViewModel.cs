using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Project_ITLab.Models.Domain;

namespace Project_ITLab.Models.ViewModels
{
    public class AttendViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(13, ErrorMessage = "{0} may not contain more than 13 characters")]
        public string CardNumber { get; set; }


        public Session Session { get; }
        public User User { get; }

        public AttendViewModel(Session session, User user)
        {
            Session = session;
            User = user;
        }
        public AttendViewModel() { }
    }
}
