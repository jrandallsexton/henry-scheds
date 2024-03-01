using System;

namespace Henry.Scheduling.Api.Common.Exceptions
{
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException() { }

        public InvalidCommandException(string message) :
            base(message)
        { }
    }
}
