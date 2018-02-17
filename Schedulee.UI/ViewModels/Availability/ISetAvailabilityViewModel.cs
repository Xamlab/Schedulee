using System;
using System.Collections.Generic;
using Schedulee.UI.ViewModels.Base;

namespace Schedulee.UI.ViewModels.Availability
{
    public interface ISetAvailabilityViewModel : ISaveableViewModel, ICollectionViewModel<IAvailabilityViewModel>, IViewModelRequiresSetup
    {
        IList<IDayOfWeekViewModel> DaysOfWeek { get; }
        IList<ITimePeriodViewModel> TimePeriods { get; }
        bool InProgress { get; }
        IAsyncCommand AddTimeAvailableCommand { get; }
        IAsyncCommand AddTimePeriodCommand { get; }
        IAsyncCommand DeleteTimePeriodCommand { get; }
        IAsyncCommand ToggleDayCommand { get; }
        IAsyncCommand CancelCommand { get; }
        bool IsAddingAvailableTimePeriod { get; }
        event EventHandler DidBeginAddingTimePeriod;
        event EventHandler DidCancelAddingTimePeriod;
        event EventHandler DidCreateTimeAvailability;
    }
}