using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itenso.TimePeriod;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal static class Helpers
    {
        internal static readonly List<string> DaysOfWeek = new List<string>
                                                           {
                                                               Strings.Monday,
                                                               Strings.Tuesday,
                                                               Strings.Wednesday,
                                                               Strings.Thursday,
                                                               Strings.Friday,
                                                               Strings.Saturday,
                                                               Strings.Sunday
                                                           };

        internal static bool CheckPeriodsIntersection(List<TimePeriod> periods)
        {
            return false;
        }

        internal static IAvailabilityViewModel MapUserAvailabilityToAvailabilityViewModel(UserAvailablity availablity)
        {
            return new AvailabilityViewModel
                   {
                       Availablity = availablity,
                       FormattedDaysOfWeek = GetFormattedDaysOfWeek(availablity),
                       FormattedTimePeriods = GetFormattedTimePeriods(availablity)
                   };
        }

        internal static void EndAddingAvailableTimePeriod(SetAvailabilityViewModel viewModel)
        {
            viewModel.TimePeriods.Clear();
            foreach(var day in viewModel.DaysOfWeek)
            {
                ((DayOfWeekViewModel) day).IsSelected = false;
            }

            viewModel.StaleMonitor.StartMonitoring();
            viewModel.IsAddingAvailableTimePeriod = false;
            viewModel.AtLeastOneDayOfWeekIsSelected = false;
        }

        internal static async Task<bool> CheckIntersection(SetAvailabilityViewModel viewModel,
                                                           ITimeProvider timeProvider,
                                                           IDialogService dialogService,
                                                           TimePeriod newPeriod = null)
        {
            var timeRanges = new TimePeriodCollection();

            var startOfWeek = StartOfWeek(timeProvider.DateTimeNow.Date, DayOfWeek.Monday);
            foreach(var availability in viewModel.Items)
            {
                foreach(var dayOfWeek in availability.Availablity.DaysOfWeek)
                {
                    var timePeriods = availability.Availablity.TimePeriods;
                    AddTimePeriods(timePeriods, startOfWeek, timeRanges, dayOfWeek);
                }
            }

            foreach(var day in viewModel.DaysOfWeek.Where(day => day.IsSelected))
            {
                var timePeriods = viewModel.TimePeriods.Select(model => new TimePeriod(model.Start, model.End)).ToList();
                if(newPeriod != null)
                    timePeriods.Add(newPeriod);
                AddTimePeriods(timePeriods, startOfWeek, timeRanges, day.Day);
            }

            var periodIntersector = new TimePeriodIntersector<TimeRange>();
            var intersectedPeriods = periodIntersector.IntersectPeriods(timeRanges);
            var isIntersecting = intersectedPeriods.Any();
            if(isIntersecting)
            {
                await dialogService.ShowNotificationAsync(Strings.TimePeriodsIntersectingValidationError, CommonStrings.Ok);
            }

            return isIntersecting;
        }

        private static string GetFormattedDaysOfWeek(UserAvailablity availablity)
        {
            if(availablity?.DaysOfWeek?.Any() != true) return "";
            return string.Join(", ", availablity.DaysOfWeek.Select(day => DaysOfWeek[day]));
        }

        private static string GetFormattedTimePeriods(UserAvailablity availablity)
        {
            if(availablity?.TimePeriods?.Any() != true) return "";
            return string.Join(", ", availablity.TimePeriods.Select(period => $"{period.Start:HH:mm} - {period.End:HH:mm}"));
        }

        private static void AddTimePeriods(IEnumerable<TimePeriod> timePeriods, DateTime startOfWeek, TimePeriodCollection timeRanges, int dayOfWeek)
        {
            foreach(var timePeriod in timePeriods)
            {
                var startDate = startOfWeek.Add(new TimeSpan(dayOfWeek, timePeriod.Start.Hour, timePeriod.Start.Minute, timePeriod.Start.Second));
                var endDate = startOfWeek.Add(new TimeSpan(dayOfWeek, timePeriod.End.Hour, timePeriod.End.Minute, timePeriod.End.Second));
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