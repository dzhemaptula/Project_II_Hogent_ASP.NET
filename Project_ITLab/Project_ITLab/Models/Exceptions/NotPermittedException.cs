using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_ITLab.Models.Exceptions
{
    public class NotPermittedException : Exception
    {

        public NotPermittedException()
        {
        }

        public NotPermittedException(string message)
            : base(message)
        {
        }

    }
}
