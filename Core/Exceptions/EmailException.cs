using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    [Serializable]
    internal class EmailInvalidException : Exception
    {
        public EmailInvalidException() : base("Email is invalid.")
        {
        }

        public EmailInvalidException(string message)
            : base(message)
        {
        }

        public EmailInvalidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class PhoneInvalidException : Exception
    {
        public PhoneInvalidException() :base("Phone is invalid.")
        {
        }

        public PhoneInvalidException(string message)
            : base(message)
        {
        }

        public PhoneInvalidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class PhoneAndEmailInvalidException : Exception
    {
        public PhoneAndEmailInvalidException() : base("Both Phone and Email cannot be empty.")
        {
        }

        public PhoneAndEmailInvalidException(string message)
            : base(message)
        {
        }

        public PhoneAndEmailInvalidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
