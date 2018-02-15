using Schedulee.Core.Services;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    internal class ReservationsViewModel : BaseCollectionViewModel<IDateViewModel>, IReservationsViewModel
    {
        public ReservationsViewModel(IApiClient apiClient, ITimeProvider timeProvider)
        {
            LoadCommand = new LoadReservationsCommand(this, apiClient, timeProvider);
        }

        public IDateViewModel SelectedDate { get; set; }
    }
}