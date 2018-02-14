namespace Schedulee.Core.DI.Implementation
{
    public static class ServiceLocater
    {
        public static IDependencyContainer Instance { get; private set; }

        public static void Create(IDependencyContainer container)
        {
            Instance = container;
        }
    }
}