using System.Linq;
using System.Threading.Tasks;
using Schedulee.Core.Services;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class ToggleDayCommand : BaseAvailabilityManipulationCommand
    {
        private readonly ITimeProvider _timeProvider;

        public ToggleDayCommand(SetAvailabilityViewModel viewModel, ITimeProvider timeProvider, IDialogService dialogService) : base(viewModel, dialogService)
        {
            _timeProvider = timeProvider;
        }

        protected override async Task ExecuteCoreAsync(object parameter)
        {
            if(parameter == null || !(parameter is DayOfWeekViewModel dayOfWeek))
            {
                await DialogService.ShowNotificationAsync(Strings.InvalidDayOfWeekError, CommonStrings.Ok);
                return;
            }

            dayOfWeek.IsSelected = !dayOfWeek.IsSelected;
            if(dayOfWeek.IsSelected && await Helpers.CheckIntersection(ViewModel, _timeProvider, DialogService))
            {
                dayOfWeek.IsSelected = false;
            }

            ViewModel.AtLeastOneDayOfWeekIsSelected = ViewModel.DaysOfWeek.Any(day => day.IsSelected);
        }
    }
}