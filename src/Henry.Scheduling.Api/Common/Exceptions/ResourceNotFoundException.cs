using System;

namespace Henry.Scheduling.Api.Common.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() { }

        public ResourceNotFoundException(string message) :
            base(message)
        { }
    }
}
