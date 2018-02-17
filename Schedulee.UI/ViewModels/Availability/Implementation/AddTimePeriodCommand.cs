using System;
using Schedulee.Core.Models;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class AddTimePeriodCommand : BaseAvailabilityManipulationCommand
    {
        public AddTimePeriodCommand(SetAvailabilityViewModel viewModel, IDialogService dialogService) : base(viewModel, dialogService)
        {
        }

        protected override async void ExecuteCore(object parameter)
        {
            //if(parameter == null || !(parameter is TimePeriod timePeriod))
            //{
            //    await DialogService.ShowNotificationAsync(Strings.InvalidTimePeriodError, CommonStrings.Ok);
            //    return;
            //}
            var now = DateTime.Now;
            var timePeriod = new TimePeriod(now, now.AddHours(2));
            ViewModel.TimePeriods.Add(new TimePeriodViewModel
                                      {
                                          Start = timePeriod.Start,
                                          End = timePeriod.End,
                                          FormattedTimePeriod = $"{timePeriod.Start:HH:mm} - {timePeriod.End:HH:mm}"
                                      });
        }
    }
}