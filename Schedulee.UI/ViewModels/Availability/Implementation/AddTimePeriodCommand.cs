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
            : base(viewModel, dialogService, viewModel.AtLeasetOneDayOfWeekIsSelected)
        {
            _timeProvider = timeProvider;
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
            if(parameter == null || !(parameter is TimePeriod timePeriod))
            {
                await DialogService.ShowNotificationAsync(Strings.InvalidTimePeriodError, CommonStrings.Ok);
                return;
            }

            if(await CheckIntersection(timePeriod)) return;

            ViewModel.TimePeriods.Add(new TimePeriodViewModel
                                      {
                                          Start = timePeriod.Start,
                                          End = timePeriod.End,
                                          FormattedTimePeriod = $"{timePeriod.Start:HH:mm} - {timePeriod.End:HH:mm}"
                                      });
        }

        protected override void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.ViewModelOnPropertyChanged(sender, args);
            if(args.PropertyName == nameof(SetAvailabilityViewModel.AtLeasetOneDayOfWeekIsSelected))
            {
                UpdateCanExecute();
            }
        }

        protected override bool UpdateCanExecute()
        {
            return base.UpdateCanExecute() && ViewModel.AtLeasetOneDayOfWeekIsSelected;
        }
        private async Task<bool> CheckIntersection(TimePeriod newPeriod)
        {
            var timeRanges = new TimePeriodCollection();

            var startOfWeek = StartOfWeek(_timeProvider.DateTimeNow.Date, DayOfWeek.Monday);
            foreach(var availability in ViewModel.Items)
            {
                foreach(var dayOfWeek in availability.Availablity.DaysOfWeek)
                {
                    var timePeriods = availability.Availablity.TimePeriods;
                    AddTimePeriods(timePeriods, startOfWeek, timeRanges);
                }
            }

            foreach(var day in ViewModel.DaysOfWeek.Where(day => day.IsSelected))
            {
                var timePeriods = ViewModel.TimePeriods.Select(model => new TimePeriod(model.Start, model.End)).ToList();
                timePeriods.Add(newPeriod);
                AddTimePeriods(timePeriods, startOfWeek, timeRanges);
            }

            var periodIntersector = new TimePeriodIntersector<TimeRange>();
            var intersectedPeriods = periodIntersector.IntersectPeriods(timeRanges);
            var isIntersecting = intersectedPeriods.Any();
            if(isIntersecting)
            {
                await DialogService.ShowNotificationAsync(Strings.TimePeriodsIntersectingValidationError, CommonStrings.Ok);
            }

            return isIntersecting;
        }

        private static void AddTimePeriods(IEnumerable<TimePeriod> timePeriods, DateTime startOfWeek, TimePeriodCollection timeRanges)
        {
            foreach(var timePeriod in timePeriods)
            {
                var startDate = new DateTime(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day,
                                             timePeriod.Start.Hour, timePeriod.Start.Minute, timePeriod.Start.Second);
                var endDate = new DateTime(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day,
                                           timePeriod.End.Hour, timePeriod.End.Minute, timePeriod.End.Second);
                timeRanges.Add(new TimeRange(startDate, endDate, true));
            }
        }

        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

    }
}