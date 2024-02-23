using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Exceptions
{
    public class OccasionNotCorrectException : Exception
    {
        public OccasionNotCorrectException()
        {
        }

        public OccasionNotCorrectException(string message)
            : base(message)
        {
        }

        public OccasionNotCorrectException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
