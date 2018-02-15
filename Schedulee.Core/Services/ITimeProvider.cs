using System;
using System.Collections.Generic;
using System.Text;

namespace Schedulee.Core.Services
{
    public interface ITimeProvider
    {
        DateTime DateTimeNow { get; }
        DateTimeOffset DateTimeOffsetNow { get; }
    }
}
