using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Schedulee.Core.Managers;
using Schedulee.Core.Models;
using Schedulee.Core.Providers;
using User = Firebase.Auth.User;

namespace Schedulee.Core.Services.Implementation
{
    public class FirebaseApiClient : IApiClient
    {
        private readonly FirebaseClient _client;
        private readonly ISecureSettingsManager _secureSettings;
        private readonly IAuthenticationManager _authManager;
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private Token _tempToken;

        public FirebaseApiClient(ISecureSettingsManager secureSettings,
                                 IAuthenticationManager authManager,
                                 IConfigurationProvider configuration)
        {
            _authManager = authManager;
            _secureSettings = secureSettings;
            _client = new FirebaseClient(configuration.BaseUrl, new FirebaseOptions
                                                                {
                                                                    AuthTokenAsyncFactory = AuthTokenAsyncFactory
                                                                });
            _firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(configuration.FirebaseApiKey));
        }

        public async Task<Token> LoginAsync(string username, string password, CancellationToken token = default(CancellationToken))
        {
            //Get the token
            var result = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(username, password);

            //Create the current user and the local token 
            var currentUser = new Models.User
                              {
                                  Id = result.User.LocalId,
                                  Email = result.User.Email
                              };
            _tempToken = new Token
                         {
                             Created = result.Created,
                             ExpiresIn = result.ExpiresIn,
                             AccessToken = result.FirebaseToken,
                             RefreshToken = result.RefreshToken,
                             User = currentUser
                         };

            //See if we have current user information
            currentUser = await _client.Child("users").Child(currentUser.Id).OnceSingleAsync<Models.User>();

            //If we don't populate the current user information
            if(currentUser == null)
            {
                //As we don't have create user method, we'll need to manually set user properties to some default values
                currentUser = new Models.User
                              {
                                  Id = result.User.LocalId,
                                  Email = result.User.Email,
                                  FirstName = "John",
                                  LastName = "Smith",
                                  Location = "Kongeleveien 29, 35055 Krokstadelva",
                                  SetTravelTime = 30
                              };
                await _client.Child("users").Child(currentUser.Id).PutAsync(currentUser);
            }

            _tempToken.User = currentUser;

            //Create sample data if necessary
            await BootstrapDataAsync(currentUser, token);
            var resultToken = _tempToken;
            _tempToken = null;
            return resultToken;
        }

        public async Task<IEnumerable<Reservation>> FetchReservationsAsync(CancellationToken token = default(CancellationToken))
        {
            var user = _secureSettings.GetAccount()?.User;
            if(user == null) return null;
            var reservations = await _client.Child("reservations").Child(user.Id).OnceAsync<List<Reservation>>();
            return reservations?.Select(reservation => reservation.Object).First();
        }

        public async Task SaveAccountAsync(string firstName, string lastName, string location, int setTimeInterval, CancellationToken token = default(CancellationToken))
        {
            var user = _secureSettings.GetAccount()?.User;
            if(user == null) throw new UnauthorizedAccessException();
            await _client.Child("users").Child(user.Id).PutAsync(new Models.User
                                                                 {
                                                                     Id = user.Id,
                                                                     FirstName = firstName,
                                                                     LastName = lastName,
                                                                     Location = location,
                                                                     SetTravelTime = setTimeInterval
                                                                 });
        }

        public async Task BootstrapDataAsync(Models.User user, CancellationToken token = default(CancellationToken))
        {
            var reservation = await _client.Child("reservations").Child(user.Id).OnceSingleAsync<Reservation>();
            if(reservation == null)
            {
                var sampleReservations = GetSampleReservations();
                await _client.Child("reservations").Child(user.Id).PostAsync(sampleReservations);
            }
        }

        private List<Reservation> GetSampleReservations()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Schedulee.Core.SampleReservations.json";
            using(Stream stream = assembly.GetManifestResourceStream(resourceName))
            using(StreamReader reader = new StreamReader(stream))
            {
                string reservationsJson = reader.ReadToEnd();
                var reservations = JsonConvert.DeserializeObject<List<Reservation>>(reservationsJson);
                var random = new Random();
                foreach(var reservation in reservations)
                {
                    reservation.End = reservation.Start.AddHours(random.Next(1, 5));
                }

                return reservations;
            }
        }

        private async Task<string> AuthTokenAsyncFactory()
        {
            try
            {
                var token = _tempToken ?? _secureSettings.GetAccount();
                if(token == null || token.IsExpired && string.IsNullOrEmpty(token.RefreshToken))
                {
                    await _authManager.SignIn();
                    token = _secureSettings.GetAccount();
                    return token.AccessToken;
                }

                if(!token.IsExpired) return token.AccessToken;
                var result = await _firebaseAuthProvider.RefreshAuthAsync(new FirebaseAuth
                                                                          {
                                                                              Created = token.Created,
                                                                              ExpiresIn = token.ExpiresIn,
                                                                              FirebaseToken = token.AccessToken,
                                                                              RefreshToken = token.RefreshToken,
                                                                              User = new User
                                                                                     {
                                                                                         LocalId = token.User.Id
                                                                                     }
                                                                          });
                var newToken = new Token
                               {
                                   Created = result.Created,
                                   ExpiresIn = result.ExpiresIn,
                                   AccessToken = result.FirebaseToken,
                                   RefreshToken = result.RefreshToken,
                                   User = token.User
                               };
                _secureSettings.SetAccount(newToken);
                return newToken.AccessToken;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}