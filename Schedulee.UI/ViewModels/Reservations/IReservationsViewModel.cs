using Schedulee.UI.ViewModels.Base;

namespace Schedulee.UI.ViewModels.Reservations
{
    public interface IReservationsViewModel : ICollectionViewModel<IDateViewModel>
    {
        IDateViewModel SelectedDate { get; set; }
    }
}