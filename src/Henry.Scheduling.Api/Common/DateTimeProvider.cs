using System;

namespace Henry.Scheduling.Api.Common
{
    /// <summary>
    /// Appears something similar is now in .NET 8.  Review it: https://grantwinney.com/how-to-use-timeprovider-and-faketimeprovider/
    /// </summary>
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
