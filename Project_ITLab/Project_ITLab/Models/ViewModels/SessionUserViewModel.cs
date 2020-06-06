using Project_ITLab.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.ViewModels
{
    public class SessionUserViewModel
    {
        public User? User { get;}
        public Session Session { get;}

        [Required]
        [StringLength(500, ErrorMessage = "{0} may not contain more than 500 characters")]
        public string FeedbackText { get; set; }

        public SessionUserViewModel(Session session, User user)
        {
            User = user;
            Session = session;
        }
        public SessionUserViewModel() { }
    }
}
