using System.Windows.Input;
using Schedulee.UI.ViewModels.Base;

namespace Schedulee.UI.ViewModels.Reservations
{
    public interface IReservationsViewModel : ICollectionViewModel<IDateViewModel>
    {
        ICommand SelectDateCommand { get; }
        IDateViewModel SelectedDate { get; }
        IAsyncCommand LogOutCommand { get; }
    }
}