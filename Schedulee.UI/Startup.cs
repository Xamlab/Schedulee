using Schedulee.Core.DI;

namespace Schedulee.UI
{
    public static class Startup
    {
        public static IDependencyContainer RegisterUIDependencies(this IDependencyContainer container)
        {
            return container;
        }
    }
}