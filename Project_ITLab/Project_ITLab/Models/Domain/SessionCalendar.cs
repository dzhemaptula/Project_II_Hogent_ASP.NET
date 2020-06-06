using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.Domain
{
    public class SessionCalendar
    {
        public int SessionCalendarId { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public SessionCalendar(DateTime startDate, DateTime endDate)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public SessionCalendar() { }
    }
}
