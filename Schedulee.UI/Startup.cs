using Schedulee.Core.DI;
using Schedulee.Core.Managers;
using Schedulee.UI.Managers.Implementation;
using Schedulee.UI.ViewModels.Authentication;
using Schedulee.UI.ViewModels.Authentication.Implementation;
using Schedulee.UI.ViewModels.Reservations;
using Schedulee.UI.ViewModels.Reservations.Implementation;
using Schedulee.UI.ViewModels.Settings;
using Schedulee.UI.ViewModels.Settings.Implementations;

namespace Schedulee.UI
{
    public static class Startup
    {
        public static IDependencyContainer RegisterUIDependencies(this IDependencyContainer container)
        {
            container.RegisterSingleton<ISecureSettingsManager, SecureSettingsManager>();
            container.Register<ILoginViewModel, LoginViewModel>();
            container.Register<IReservationsViewModel, ReservationsViewModel>();
            container.Register<ISettingsViewModel, SettingsViewModel>();
            container.Register<IReservationDetailsViewModel, ReservationDetailsViewModel>();
            return container;
        }
    }
}