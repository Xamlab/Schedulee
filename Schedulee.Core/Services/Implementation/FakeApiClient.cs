//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Schedulee.Core.Models;

//namespace Schedulee.Core.Services.Implementation
//{
//    public class FakeApiClient : IApiClient
//    {
//        public Task<Token> LoginAsync(string username, string password, CancellationToken token = default(CancellationToken))
//        {
//            return Task.FromResult(new Token()
//                                   {
//                                       AccessToken = "Token",
//                                       Created = DateTime.Now,
//                                       ExpiresIn = (int)TimeSpan.FromDays(1).TotalSeconds,
//                                       RefreshToken = "Token",
//                                       User = new User()
//                                              {
//                                                  FirstName = "Test",
//                                                  LastName = "Test",
//                                                  Email = "test@xamlab.com",
//                                                  Location = "My Location",
//                                                  SetTravelTime = 10
//                                              }

//                                   });
//        }

//        public Task<IEnumerable<Reservation>> FetchReservationsAsync(CancellationToken token = default(CancellationToken))
//        {
//            return Task.FromResult(new List<Reservation>()
//                                   {
//                                       new Reservation()
//                                       {
//                                           Start = DateTimeOffset.Now.AddHours(1),
//                                           End = DateTimeOffset.Now.AddHours(3),
//                                           Client = new Client()
//                                                    {
//                                                        FirstName = "Test",
//                                                        LastName = "Client",
//                                                        Location = "Client Location",
//                                                        PhoneNumber = "+1 2345 78910"
//                                                    },
//                                           RatePerHour = 25
//                                       }
//                                   } as IEnumerable<Reservation>);
//        }

//        public Task<IEnumerable<UserAvailablity>> FetchUserAvailablities(CancellationToken token = default(CancellationToken))
//        {
//            return Task.FromResult();
//        }

//        public Task SaveAccountAsync(string firstName, string lastName, string location, int setTimeInterval, CancellationToken token = default(CancellationToken))
//        {
//            throw new NotImplementedException();
//        }

//        public Task CreateAvailabilityAsync(UserAvailablity availablity, CancellationToken token = default(CancellationToken))
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

