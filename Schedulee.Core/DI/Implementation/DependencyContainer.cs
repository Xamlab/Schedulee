using Grace.DependencyInjection;
using Grace.DependencyInjection.Lifestyle;

namespace Schedulee.Core.DI.Implementation
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly DependencyInjectionContainer _container;

        public DependencyContainer(DependencyInjectionContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Locate<T>();
        }

        public void Register<T, TImpl>() where TImpl : T
        {
            _container.Add(block => block.Export<TImpl>().As<T>());
        }

        public void Register<T>()
        {
            _container.Add(block => block.Export<T>());
        }

        public void RegisterSingleton<T, TImpl>() where TImpl : T
        {
            _container.Add(block => block.Export<TImpl>().As<T>().UsingLifestyle(new SingletonLifestyle()));
        }

        public void RegisterSingleton<T>(T implementation)
        {
            _container.Add(block => block.ExportInstance(implementation).As<T>());
        }

        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}
