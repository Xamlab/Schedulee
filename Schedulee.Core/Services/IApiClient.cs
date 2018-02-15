using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.Core.Models;

namespace Schedulee.Core.Services
{
    public interface IApiClient
    {
        Task<Token> LoginAsync(string username, string password, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<Reservation>> FetchReservationsAsync(CancellationToken token = default(CancellationToken));
    }
}