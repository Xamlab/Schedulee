using System;
using System.ComponentModel;

namespace Schedulee.UI.ViewModels.Availability
{
    public interface ITimePeriodViewModel : INotifyPropertyChanged
    {
        string FormattedTimePeriod { get; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
    }
}