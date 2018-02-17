using System.ComponentModel;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Common;
using Schedulee.UI.Services;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class CancelCommand : BaseAvailabilityManipulationCommand
    {
        public CancelCommand(SetAvailabilityViewModel viewModel, IDialogService dialogService) : base(viewModel, dialogService)
        {
        }

        protected override async Task ExecuteCoreAsync(object parameter)
        {
            if(ViewModel.StaleMonitor.IsStale)
            {
                var shouldDiscard = await DialogService.ShowConfirmationDialogAsync(Strings.DiscardConfirmation, Strings.Yes, Strings.No);
                if(!shouldDiscard) return;
            }

            Helpers.EndAddingAvailableTimePeriod(ViewModel);
            ViewModel.InvokeDidCancelAddingTimePeriod();
        }

        protected override void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.ViewModelOnPropertyChanged(sender, args);
            if(args.PropertyName == nameof(SetAvailabilityViewModel.IsAddingAvailableTimePeriod))
            {
                SetCanExecute(ViewModel.IsAddingAvailableTimePeriod);
            }
        }
    }
}