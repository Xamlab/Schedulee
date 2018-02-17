using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class DeleteTimePeriodCommand : BaseAvailabilityManipulationCommand
    {
        public DeleteTimePeriodCommand(SetAvailabilityViewModel viewModel, IDialogService dialogService) : base(viewModel, dialogService)
        {
        }

        protected override async Task ExecuteCoreAsync(object parameter)
        {
            if(parameter == null || !(parameter is ITimePeriodViewModel timePeriod))
            {
                await DialogService.ShowNotificationAsync(Strings.InvalidTimePeriodError, CommonStrings.Ok);
                return;
            }

            ViewModel.TimePeriods.Remove(timePeriod);
        }
    }
}