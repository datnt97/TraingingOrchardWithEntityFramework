using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.Utilities.Exceptions
{
    using System;

    public class eTwebException : Exception
    {
        public eTwebException()
        {
        }

        public eTwebException(string message)
            : base(message)
        {
        }

        public eTwebException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
