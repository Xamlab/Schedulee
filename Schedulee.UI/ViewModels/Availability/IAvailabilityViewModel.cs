using Schedulee.Core.Models;

namespace Schedulee.UI.ViewModels.Availability
{
    public interface IAvailabilityViewModel
    {
        UserAvailablity Availablity { get; }
        string FormattedDaysOfWeek { get; }
        string FormattedTimePeriods { get; }
    }
}