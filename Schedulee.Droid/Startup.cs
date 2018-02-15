using Grace.DependencyInjection;
using Schedulee.Core;
using Schedulee.Core.DI;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Services;
using Schedulee.Droid.Services.Implementation;
using Schedulee.UI;
using Schedulee.UI.Services;

namespace Schedulee.Droid
{
    public static class Startup
    {
        public static IDependencyContainer SetupContainer()
        {
            var graceContainer = new DependencyInjectionContainer();
            var container = new DependencyContainer(graceContainer);
            graceContainer.Add(block => block.ExportInstance(container).As<IDependencyContainer>());
            container.RegisterCoreDependencies()
                     .RegisterUIDependencies()
                     .RegisterDroidDependencies();
            ServiceLocater.Create(container);
            return container;
        }

        public static IDependencyContainer RegisterDroidDependencies(this IDependencyContainer container)
        {
            container.Register<IDialogService, DialogService>();
            container.Register<ISecureStorageService, SecureStorageService>();
            container.Register<IAuthenticationService, AuthenticationService>();
            return container;
        }
    }
}