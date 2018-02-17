using System.Windows.Input;
using PropertyChanged;
using Schedulee.Core.Managers;
using Schedulee.Core.Services;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class ReservationsViewModel : BaseCollectionViewModel<IDateViewModel>, IReservationsViewModel
    {
        public ReservationsViewModel(IApiClient apiClient,
                                     ITimeProvider timeProvider,
                                     IAuthenticationManager authManager,
                                     IDialogService dialogService)
        {
            LoadCommand = new LoadReservationsCommand(this, apiClient, timeProvider);
            SelectDateCommand = new SelectDateCommand(this);
            LogOutCommand = new LogOutCommand(authManager, dialogService);
        }

        public ICommand SelectDateCommand { get; }
        public IDateViewModel SelectedDate { get; internal set; }
        public IAsyncCommand LogOutCommand { get; }
    }
}