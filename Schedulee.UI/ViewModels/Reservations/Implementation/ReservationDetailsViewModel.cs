using System.Windows.Input;
using Schedulee.Core.Models;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    internal class ReservationDetailsViewModel : BaseViewModel, IReservationDetailsViewModel
    {
        public ReservationDetailsViewModel()
        {
            SetupCommand = new SetupReservationDetailsCommand(this);
        }

        public ICommand SetupCommand { get; }
        public Reservation Reservation { get; set; }
        public string FormattedFullDate { get; internal set; }
        public string FormattedClient { get; internal set; }
        public string FormattedTimePeriod { get; internal set; }
        public string PhoneNumber { get; internal set; }
        public string Location { get; internal set; }
        public string FormattedVat { get; internal set; }
        public string FormattedNetPrice { get; internal set; }
        public string FormattedTotal { get; internal set; }
    }
}