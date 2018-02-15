using Schedulee.Core.DI;
using Schedulee.Core.Managers;
using Schedulee.Core.Managers.Implementation;
using Schedulee.Core.Providers;
using Schedulee.Core.Providers.Implementation;
using Schedulee.Core.Services;
using Schedulee.Core.Services.Implementation;

namespace Schedulee.Core
{
    public static class Startup
    {
        public static IDependencyContainer RegisterCoreDependencies(this IDependencyContainer container)
        {
            container.Register<IApiClient, FirebaseApiClient>();
            container.RegisterSingleton<IAuthenticationManager, AuthenticationManager>();
            container.Register<IConfigurationProvider, DevelopmentConfigurationProvider>();
            return container;
        }
    }
}