using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base.Implementation;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal abstract class BaseAvailabilityManipulationCommand : AsyncCommand
    {
        protected SetAvailabilityViewModel ViewModel { get; }
        protected IDialogService DialogService { get; }

        protected BaseAvailabilityManipulationCommand(SetAvailabilityViewModel viewModel, IDialogService dialogService, bool canExecute = true)
            : base(canExecute && viewModel.IsAddingAvailableTimePeriod)
        {
            DialogService = dialogService;
            ViewModel = viewModel;
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        public override async Task ExecuteAsync(object parameter, CancellationToken token = default(CancellationToken))
        {
            if(!ViewModel.IsAddingAvailableTimePeriod)
            {
                await DialogService.ShowNotificationAsync(Strings.NoAddTimeAvailableStarted, CommonStrings.Ok);
                return;
            }

            await ExecuteCoreAsync(parameter);
        }

        protected abstract Task ExecuteCoreAsync(object parameter);

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