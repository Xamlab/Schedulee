using System;

namespace Schedulee.UI.ViewModels.Availability
{
    public interface ITimePeriodViewModel
    {
        string FormattedTimePeriod { get; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
    }
}