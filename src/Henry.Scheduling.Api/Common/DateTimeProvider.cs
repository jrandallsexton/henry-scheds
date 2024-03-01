﻿using System;

namespace Henry.Scheduling.Api.Common
{
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