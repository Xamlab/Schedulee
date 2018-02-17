using System.ComponentModel;
using System.Threading.Tasks;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class AddTimePeriodCommand : BaseAvailabilityManipulationCommand
    {
        private readonly ITimeProvider _timeProvider;

        public AddTimePeriodCommand(SetAvailabilityViewModel viewModel, ITimeProvider timeProvider, IDialogService dialogService)
            : base(viewModel, dialogService, viewModel.AtLeastOneDayOfWeekIsSelected)
        {
            _timeProvider = timeProvider;
        }

        protected override async Task ExecuteCoreAsync(object parameter)
        {
            if(parameter == null || !(parameter is TimePeriod timePeriod))
            {
                await DialogService.ShowNotificationAsync(Strings.InvalidTimePeriodError, CommonStrings.Ok);
                return;
            }

            if(!ViewModel.AtLeastOneDayOfWeekIsSelected)
            {
                await DialogService.ShowNotificationAsync(Strings.TimePeriodRequiredValidationError, CommonStrings.Ok);
                return;
            }

            var formattedTimePeriod = $"{timePeriod.Start:HH:mm} - {timePeriod.End:HH:mm}";

            if(timePeriod.Start >= timePeriod.End)
            {
                await DialogService.ShowNotificationAsync(Strings.StartTimeGreaterEndTime, CommonStrings.Ok, formattedTimePeriod);
                return;
            }

            if(await Helpers.CheckIntersection(ViewModel, _timeProvider, DialogService, timePeriod)) return;

            ViewModel.TimePeriods.Add(new TimePeriodViewModel
                                      {
                                          Start = timePeriod.Start,
                                          End = timePeriod.End,
                                          FormattedTimePeriod = formattedTimePeriod
                                      });
        }

        protected override void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.ViewModelOnPropertyChanged(sender, args);
            if(args.PropertyName == nameof(SetAvailabilityViewModel.AtLeastOneDayOfWeekIsSelected))
            {
                UpdateCanExecute();
            }
        }

        protected override bool UpdateCanExecute()
        {
            var canExecute = base.UpdateCanExecute() && ViewModel.AtLeastOneDayOfWeekIsSelected;
            SetCanExecute(canExecute);
            return canExecute;
        }
    }
}