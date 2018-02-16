using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base.Implementation;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class CreateAvailabilityCommand : SaveAsyncCommand
    {
        private readonly SetAvailabilityViewModel _viewModel;
        private readonly IApiClient _apiClient;
        private readonly IDialogService _dialogService;

        public CreateAvailabilityCommand(SetAvailabilityViewModel viewModel,
                                         IApiClient apiClient,
                                         IDialogService dialogService) : base(viewModel, dialogService, viewModel.IsAddingAvailableTimePeriod)
        {
            _dialogService = dialogService;
            _apiClient = apiClient;
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        public override async Task ExecuteAsync(object param, CancellationToken token = default(CancellationToken))
        {
            if (!_viewModel.IsAddingAvailableTimePeriod)
            {
                await _dialogService.ShowNotificationAsync(Strings.NoAddTimeAvailableStarted, CommonStrings.Ok);
                return;
            }

            await base.ExecuteAsync(param, token);
        }

        protected override async Task<bool> ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken))
        {
            var availability = new UserAvailablity
                               {
                                   Id = Guid.NewGuid().ToString(),
                                   TimePeriods = _viewModel.TimePeriods.Select(period => new TimePeriod(period.Start, period.End)).ToArray(),
                                   DaysOfWeek = _viewModel.DaysOfWeek.Where(day => day.IsSelected).Select(day => day.Day).ToArray()
                               };
            await _apiClient.CreateAvailabilityAsync(availability, token);
            _viewModel.Items.Add(Helpers.MapUserAvailabilityToAvailabilityViewModel(availability));
            Helpers.EndAddingAvailableTimePeriod(_viewModel);
            return true;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName == nameof(SetAvailabilityViewModel.IsAddingAvailableTimePeriod))
            {
                SetCanExecute(_viewModel.IsAddingAvailableTimePeriod);
            }
        }
    }
}