using Schedulee.Core.Models;
using Schedulee.UI.ViewModels.Base;

namespace Schedulee.UI.ViewModels.Reservations
{
    public interface IReservationDetailsViewModel : IViewModelRequiresSetup
    {
        Reservation Reservation { get; set; }
        string FormattedFullDate { get; }
        string FormattedClient { get; }
        string FormattedTimePeriod { get; }
        string PhoneNumber { get; }
        string Location { get; }
        string FormattedVat { get; }
        string FormattedNetPrice { get; }
        string FormattedTotal { get; }
    }
}