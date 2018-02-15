using Schedulee.Core.Services;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    internal class ReservationsViewModel : BaseCollectionViewModel<IDateViewModel>, IReservationsViewModel
    {
        public ReservationsViewModel(IApiClient apiClient)
        {
            LoadCommand = new LoadReservationsCommand(this, apiClient);
        }

        public IDateViewModel SelectedDate { get; set; }
    }
}