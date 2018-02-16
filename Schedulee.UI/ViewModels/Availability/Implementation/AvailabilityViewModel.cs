using PropertyChanged;
using Schedulee.Core.Models;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class AvailabilityViewModel : BaseViewModel, IAvailabilityViewModel
    {
        public UserAvailablity Availablity { get; internal set; }
        public string FormattedDaysOfWeek { get; internal set; }
        public string FormattedTimePeriods { get; internal set; }
    }
}