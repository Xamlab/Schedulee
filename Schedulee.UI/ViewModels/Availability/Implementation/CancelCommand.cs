using System.ComponentModel;
using Schedulee.UI.Resources.Strings.Common;
using Schedulee.UI.Services;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class CancelCommand : BaseAvailabilityManipulationCommand
    {
        public CancelCommand(SetAvailabilityViewModel viewModel, IDialogService dialogService) : base(viewModel, dialogService)
        {
        }

        protected override async void ExecuteCore(object parameter)
        {
            var shouldDiscard = await DialogService.ShowConfirmationDialogAsync(Strings.DiscardConfirmation, Strings.Yes, Strings.No);
            if(!shouldDiscard) return;
            Helpers.EndAddingAvailableTimePeriod(ViewModel);
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName == nameof(SetAvailabilityViewModel.IsAddingAvailableTimePeriod))
            {
                SetCanExecute(ViewModel.IsAddingAvailableTimePeriod);
            }
        }
    }
}