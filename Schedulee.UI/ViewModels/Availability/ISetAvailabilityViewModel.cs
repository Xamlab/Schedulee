using System.Collections.Generic;
using System.Windows.Input;
using Schedulee.UI.ViewModels.Base;

namespace Schedulee.UI.ViewModels.Availability
{
    public interface ISetAvailabilityViewModel : ISaveableViewModel, ICollectionViewModel<IAvailabilityViewModel>
    {
        IList<IDayOfWeekViewModel> DaysOfWeek { get; }
        IList<ITimePeriodViewModel> TimePeriods  { get; }
        bool IsIntersecting { get; }
        ICommand AddTimeAvailableCommand { get; }
        ICommand AddTimePeriodCommand { get; }
        ICommand DeleteTimePeriodCommand { get; }
        ICommand ToggleDayCommand { get; }
        ICommand CancelCommand { get; }
    }
}