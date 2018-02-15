using System.Threading.Tasks;
using Schedulee.Core.Models;

namespace Schedulee.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Token> Authenticate();
    }
}