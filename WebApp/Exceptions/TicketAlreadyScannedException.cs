using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Exceptions
{
    public class TicketAlreadyScannedException : Exception
    {
        public TicketAlreadyScannedException()
        {
        }

        public TicketAlreadyScannedException(string message)
            : base(message)
        {
        }

        public TicketAlreadyScannedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
