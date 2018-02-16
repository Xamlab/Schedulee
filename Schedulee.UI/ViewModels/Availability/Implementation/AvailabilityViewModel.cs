using Schedulee.Core.Models;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class AvailabilityViewModel : IAvailabilityViewModel
    {
        public UserAvailablity Availablity { get; internal set; }
        public string FormattedDaysOfWeek { get; internal set; }
        public string FormattedTimePeriods { get; internal set; }
    }
}