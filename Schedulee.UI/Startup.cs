using Schedulee.Core.DI;
using Schedulee.UI.ViewModels.Authentication;
using Schedulee.UI.ViewModels.Authentication.Implementation;

namespace Schedulee.UI
{
    public static class Startup
    {
        public static IDependencyContainer RegisterUIDependencies(this IDependencyContainer container)
        {
            container.Register<ILoginViewModel, LoginViewModel>();
            return container;
        }
    }
}