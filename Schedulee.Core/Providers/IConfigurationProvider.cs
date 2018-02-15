namespace Schedulee.Core.Providers
{
    public interface IConfigurationProvider
    {
        string AppId { get; }
        string BaseUrl { get; }
        string FirebaseApiKey { get; }
    }
}