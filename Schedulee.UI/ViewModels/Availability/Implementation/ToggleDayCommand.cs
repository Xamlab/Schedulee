using System.Linq;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class ToggleDayCommand : BaseAvailabilityManipulationCommand
    {
        public ToggleDayCommand(SetAvailabilityViewModel viewModel, IDialogService dialogService) : base(viewModel, dialogService)
        {
        }

        protected override async Task ExecuteCoreAsync(object parameter)
        {
            if(parameter == null || !(parameter is DayOfWeekViewModel dayOfWeek))
            {
                await DialogService.ShowNotificationAsync(Strings.InvalidDayOfWeekError, CommonStrings.Ok);
                return;
            }

            dayOfWeek.IsSelected = !dayOfWeek.IsSelected;
            ViewModel.AtLeasetOneDayOfWeekIsSelected = ViewModel.DaysOfWeek.Any(day => day.IsSelected);
        }
    }
}