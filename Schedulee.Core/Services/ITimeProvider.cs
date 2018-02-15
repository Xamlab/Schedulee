using System;

namespace Schedulee.Core.Services
{
    public interface ITimeProvider
    {
        DateTime DateTimeNow { get; }
        DateTimeOffset DateTimeOffsetNow { get; }
    }
}