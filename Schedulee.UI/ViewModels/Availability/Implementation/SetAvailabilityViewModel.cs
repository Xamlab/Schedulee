﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PropertyChanged;
using Schedulee.Core.Services;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base;
using Schedulee.UI.ViewModels.Base.Implementation;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class SetAvailabilityViewModel : BaseCollectionViewModel<IAvailabilityViewModel>, ISetAvailabilityViewModel, IInternalSaveableViewModel
    {
        public SetAvailabilityViewModel(IApiClient apiClient, IDialogService dialogService, ITimeProvider timeProvider)
        {
            TimePeriods = new ObservableCollection<ITimePeriodViewModel>();

            LoadCommand = new LoadUserAvailabilitiesCommand(this, apiClient);
            StaleMonitor = new SetAvailabilityStaleMonitor(this);
            Validator = new SetAvailabilityValidator(this);
            AddTimeAvailableCommand = new AddTimeAvailableCommand(this, dialogService);
            AddTimePeriodCommand = new AddTimePeriodCommand(this, timeProvider, dialogService);
            DeleteTimePeriodCommand = new DeleteTimePeriodCommand(this, dialogService);
            ToggleDayCommand = new ToggleDayCommand(this, dialogService);
            CancelCommand = new CancelCommand(this, dialogService);
            SaveCommand = new CreateAvailabilityCommand(this, apiClient, dialogService);
        }

        [DependsOn(nameof(IsSaving), nameof(IsLoading))]
        public bool InProgress => IsSaving || IsLoading;

        public ICommand AddTimeAvailableCommand { get; }
        public ICommand AddTimePeriodCommand { get; }
        public ICommand DeleteTimePeriodCommand { get; }
        public ICommand ToggleDayCommand { get; }
        public ICommand CancelCommand { get; }
        public IAsyncCommand SaveCommand { get; }
        public IViewModelValidator Validator { get; }
        public IStaleMonitor StaleMonitor { get; }

        public IList<IDayOfWeekViewModel> DaysOfWeek { get; internal set; }
        public IList<ITimePeriodViewModel> TimePeriods { get; }
        internal bool IsAddingAvailableTimePeriod { get; set; }
        public bool IsSaving { get; set; }
        public bool DidSave { get; set; }
        public string SavingFailureMessage { get; set; }
        internal bool AtLeasetOneDayOfWeekIsSelected { get; set; }
    }
}