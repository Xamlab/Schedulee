using System;

namespace Schedulee.Core.Services.Implementation
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime DateTimeNow => DateTime.Now;
        public DateTimeOffset DateTimeOffsetNow => DateTimeOffset.Now;
    }
}