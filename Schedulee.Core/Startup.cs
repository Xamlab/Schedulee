using Schedulee.Core.DI;

namespace Schedulee.Core
{
    public static class Startup
    {
        public static IDependencyContainer RegisterCoreDependencies(this IDependencyContainer container)
        {
            return container;
        }
    }
}