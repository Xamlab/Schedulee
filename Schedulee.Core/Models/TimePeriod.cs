using System;

namespace Schedulee.Core.Models
{
    public class TimePeriod
    {
        public TimePeriod()
        {
        }

        public TimePeriod(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}