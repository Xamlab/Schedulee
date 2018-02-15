using System.Threading.Tasks;
using Schedulee.Core.Models;
using Schedulee.Core.Services;

namespace Schedulee.Core.Tests.Managers
{
    public class MockAuthenticationService : IAuthenticationService
    {
        public Token Token { get; set; }

        public Task<Token> Authenticate()
        {
            return Task.FromResult(Token);
        }
    }
}