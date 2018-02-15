using Schedulee.Core.DI;
using Schedulee.Core.Services;
using Schedulee.Core.Services.Implementation;

namespace Schedulee.Core
{
    public static class Startup
    {
        public static IDependencyContainer RegisterCoreDependencies(this IDependencyContainer container)
        {
            container.Register<IApiClient, FirebaseApiClient>();
            return container;
        }
    }
}