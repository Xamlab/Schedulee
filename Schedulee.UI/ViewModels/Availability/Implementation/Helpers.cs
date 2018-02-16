using System.Collections.Generic;
using System.Linq;
using Schedulee.Core.Models;
using Schedulee.UI.Resources.Strings.Availability;

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
        }

        private static string GetFormattedDaysOfWeek(UserAvailablity availablity)
        {
            if(availablity?.DaysOfWeek?.Any() != true) return "";
            return string.Join(", ", availablity.DaysOfWeek.Select(day => DaysOfWeek[day]));
        }

        private static string GetFormattedTimePeriods(UserAvailablity availablity)
        {
            if(availablity?.TimePeriods?.Any() != true) return "";
            return string.Join(", ", availablity.TimePeriods.Select(period => $"{period.Start:hh:mm} - {period.End:hh:mm}"));
        }
    }
}