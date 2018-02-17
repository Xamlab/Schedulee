using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PropertyChanged;
using Schedulee.Core.Services;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class SetAvailabilityViewModel : BaseCollectionViewModel<IAvailabilityViewModel>, ISetAvailabilityViewModel, IInternalSaveableViewModel
    {
        public SetAvailabilityViewModel(IApiClient apiClient, IDialogService dialogService, ITimeProvider timeProvider)
        {
            TimePeriods = new ObservableCollection<ITimePeriodViewModel>();
            SetupCommand = new SetupSetAvailabilityViewModelCommand(this);
            LoadCommand = new LoadUserAvailabilitiesCommand(this, apiClient);
            StaleMonitor = new SetAvailabilityStaleMonitor(this);
            Validator = new SetAvailabilityValidator(this);
            AddTimeAvailableCommand = new AddTimeAvailableCommand(this, dialogService);
            AddTimePeriodCommand = new AddTimePeriodCommand(this, timeProvider, dialogService);
            DeleteTimePeriodCommand = new DeleteTimePeriodCommand(this, dialogService);
            ToggleDayCommand = new ToggleDayCommand(this, timeProvider, dialogService);
            CancelCommand = new CancelCommand(this, dialogService);
            SaveCommand = new CreateAvailabilityCommand(this, apiClient, dialogService);
        }

        public ICommand SetupCommand { get; }
        public IAsyncCommand AddTimeAvailableCommand { get; }
        public IAsyncCommand AddTimePeriodCommand { get; }
        public IAsyncCommand DeleteTimePeriodCommand { get; }
        public IAsyncCommand ToggleDayCommand { get; }
        public IAsyncCommand CancelCommand { get; }
        public IAsyncCommand SaveCommand { get; }
        public IViewModelValidator Validator { get; }
        public IStaleMonitor StaleMonitor { get; }

        public bool InProgress { get; internal set; }

        public IList<IDayOfWeekViewModel> DaysOfWeek { get; internal set; }
        public IList<ITimePeriodViewModel> TimePeriods { get; }
        public bool IsAddingAvailableTimePeriod { get; internal set; }
        public bool IsSaving { get; set; }
        public bool DidSave { get; set; }
        public string SavingFailureMessage { get; set; }
        internal bool AtLeastOneDayOfWeekIsSelected { get; set; }
        public event EventHandler DidBeginAddingTimePeriod;
        public event EventHandler DidCancelAddingTimePeriod;
        public event EventHandler DidCreateTimeAvailability;

        internal void InvokeDidBeginAddingTimePeriod()
        {
            DidBeginAddingTimePeriod?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeDidCancelAddingTimePeriod()
        {
            DidCancelAddingTimePeriod?.Invoke(this, EventArgs.Empty);
        }

        internal void InvokeDidSaveTimeAvailability()
        {
            DidCreateTimeAvailability?.Invoke(this, EventArgs.Empty);
        }
    }
}