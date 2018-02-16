using Schedulee.Core.Models;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    internal class SetupReservationDetailsCommand : Command
    {
        private readonly ReservationDetailsViewModel _viewModel;

        public SetupReservationDetailsCommand(ReservationDetailsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            var reservation = _viewModel.Reservation;
            _viewModel.FormattedFullDate = reservation?.Start.ToString("F").ToUpper() ?? "";
            _viewModel.FormattedClient = reservation?.Client != null ? $"{reservation.Client.FirstName} {reservation.Client.LastName}" : "";
            _viewModel.FormattedTimePeriod = reservation != null ? $"{reservation.Start:HH:mm} {reservation.End:HH:mm}" : "";
            _viewModel.PhoneNumber = reservation?.Client?.PhoneNumber ?? "";
            _viewModel.Location = reservation?.Client?.Location ?? "";

            double NetPrice(Reservation res)
            {
                return res.End.Subtract(res.Start).TotalHours * res.RatePerHour;
            }

            _viewModel.FormattedNetPrice = reservation != null ? NetPrice(reservation).ToString("C2") : "";
            _viewModel.FormattedVat = reservation != null ? (NetPrice(reservation) * 0.25).ToString("C2") : "";
            _viewModel.FormattedTotal = reservation != null ? (NetPrice(reservation) * 1.25).ToString("C2") : "";
        }
    }
}