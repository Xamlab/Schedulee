using Schedulee.Core.DI;
using Schedulee.Core.Managers;
using Schedulee.UI.Managers.Implementation;
using Schedulee.UI.ViewModels.Authentication;
using Schedulee.UI.ViewModels.Authentication.Implementation;
using Schedulee.UI.ViewModels.Reservations;
using Schedulee.UI.ViewModels.Reservations.Implementation;

namespace Schedulee.UI
{
    public static class Startup
    {
        public static IDependencyContainer RegisterUIDependencies(this IDependencyContainer container)
        {
            container.RegisterSingleton<ISecureSettingsManager, SecureSettingsManager>();
            container.Register<ILoginViewModel, LoginViewModel>();
            container.Register<IReservationsViewModel, ReservationsViewModel>();
            return container;
        }
    }
}