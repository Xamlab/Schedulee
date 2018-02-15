using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Schedulee.Core.Models;

namespace Schedulee.Core.Services.Implementation
{
    public class FirebaseApiClient : IApiClient
    {
        private const string FirebaseApiKey = "AIzaSyABr3ZoAR5KqrXUei_K5G1qvjmuKmuxHJQ";

        public async Task<Token> LoginAsync(string username, string password, CancellationToken token = default(CancellationToken))
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
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