using System.ComponentModel;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base.Implementation;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class BaseAvailabilityManipulationCommand : Command
    {
        protected SetAvailabilityViewModel ViewModel { get; }
        protected IDialogService DialogService { get; }

        public BaseAvailabilityManipulationCommand(SetAvailabilityViewModel viewModel, IDialogService dialogService, bool canExecute = true)
            : base(canExecute && viewModel.IsAddingAvailableTimePeriod)
        {
            DialogService = dialogService;
            ViewModel = viewModel;
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        public override async void Execute(object parameter)
        {
            if(!ViewModel.IsAddingAvailableTimePeriod)
            {
                await DialogService.ShowNotificationAsync(Strings.NoAddTimeAvailableStarted, CommonStrings.Ok);
                return;
            }

            ExecuteCore(parameter);
        }

        protected virtual void ExecuteCore(object parameter)
        {
        }

        protected virtual void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName == nameof(SetAvailabilityViewModel.IsAddingAvailableTimePeriod))
            {
                UpdateCanExecute();
            }
        }

        protected virtual bool UpdateCanExecute()
        {
            SetCanExecute(ViewModel.IsAddingAvailableTimePeriod);
            return ViewModel.IsAddingAvailableTimePeriod;
        }
    }
}