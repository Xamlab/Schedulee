namespace Schedulee.Core.Providers.Implementation
{
    public class DevelopmentConfigurationProvider : IConfigurationProvider
    {
        public string AppId => "com.xamlab.schedulee";
        public string BaseUrl => "https://schedulee-1c0ce.firebaseio.com/";
        public string FirebaseApiKey => "AIzaSyABr3ZoAR5KqrXUei_K5G1qvjmuKmuxHJQ";
    }
}