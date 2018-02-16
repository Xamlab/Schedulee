using System.Windows.Input;
using PropertyChanged;
using Schedulee.Core.Services;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class ReservationsViewModel : BaseCollectionViewModel<IDateViewModel>, IReservationsViewModel
    {
        public ReservationsViewModel(IApiClient apiClient, ITimeProvider timeProvider)
        {
            LoadCommand = new LoadReservationsCommand(this, apiClient, timeProvider);
            SelectDateCommand = new SelectDateCommand(this);
        }

        public ICommand SelectDateCommand { get; }
        public IDateViewModel SelectedDate { get; internal set; }
    }
}