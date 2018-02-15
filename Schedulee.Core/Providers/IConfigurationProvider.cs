namespace Schedulee.Core.Providers
{
    public interface IConfigurationProvider
    {
        string AppId { get; }
        string FirebaseApiKey { get; }
    }
}