using System;
using System.ComponentModel.DataAnnotations;

namespace Project_ITLab.Models.ViewModels
{
    public class CreateSessionViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} may not contain more than 50 characters")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Start time")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End time")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        
    }
}
