using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Schedulee.Core.Models;
using Schedulee.Core.Providers;

namespace Schedulee.Core.Services.Implementation
{
    public class FirebaseApiClient : IApiClient
    {
        private readonly IConfigurationProvider _configuration;

        public FirebaseApiClient(IConfigurationProvider configuration)
        {
            _configuration = configuration;
        }

        public async Task<Token> LoginAsync(string username, string password, CancellationToken token = default(CancellationToken))
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(_configuration.FirebaseApiKey));
            var result = await authProvider.SignInWithEmailAndPasswordAsync(username, password);
            return new Token
                   {
                       Created = result.Created,
                       ExpiresIn = result.ExpiresIn,
                       AccessToken = result.FirebaseToken,
                       RefreshToken = result.RefreshToken,
                       User = new Models.User
                              {
                                  Id = result.User.LocalId,
                                  Email = result.User.Email,
                                  FirstName = result.User.FirstName,
                                  LastName = result.User.LastName,
                                  ProfilePictureUrl = result.User.PhotoUrl
                              }
                   };
        }
    }
}