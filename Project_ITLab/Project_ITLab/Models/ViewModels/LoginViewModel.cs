using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.ViewModels {
    public class LoginViewModel {
        [Required]
        [StringLength(50, ErrorMessage = "{0} may not contain more than 50 characters")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
