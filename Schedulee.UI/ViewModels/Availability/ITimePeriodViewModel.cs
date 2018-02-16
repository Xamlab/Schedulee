using System;
using System.Collections.Generic;
using System.Text;

namespace Schedulee.UI.ViewModels.Availability
{
    public interface ITimePeriodViewModel
    {
        string FormattedTimePeriod { get; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
    }
}
