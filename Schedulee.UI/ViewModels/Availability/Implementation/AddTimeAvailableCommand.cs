using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base.Implementation;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class AddTimeAvailableCommand : AsyncCommand
    {
        private readonly SetAvailabilityViewModel _viewModel;
        private readonly IDialogService _dialogService;

        public AddTimeAvailableCommand(SetAvailabilityViewModel viewModel, IDialogService dialogService) : base(!viewModel.IsAddingAvailableTimePeriod)
        {
            _dialogService = dialogService;
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        public override async Task ExecuteAsync(object parameter, CancellationToken token = default(CancellationToken))
        {
            if(_viewModel.IsAddingAvailableTimePeriod)
            {
                await _dialogService.ShowNotificationAsync(Strings.ActiveAddTimeAvailableError, CommonStrings.Ok);
                return;
            }

            _viewModel.IsAddingAvailableTimePeriod = true;
            _viewModel.DaysOfWeek = _viewModel.DaysOfWeek ?? Enumerable.Range(0, 7).Select(day => new DayOfWeekViewModel
                                                                                                  {
                                                                                                      Day = day,
                                                                                                      FormattedDay = Helpers.DaysOfWeek[day]
                                                                                                  }).ToList<IDayOfWeekViewModel>();
            _viewModel.StaleMonitor.StartMonitoring();
            _viewModel.InvokeDidBeginAddingTimePeriod();
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName == nameof(SetAvailabilityViewModel.IsAddingAvailableTimePeriod))
            {
                SetCanExecute(!_viewModel.IsAddingAvailableTimePeriod);
            }
        }
    }
}