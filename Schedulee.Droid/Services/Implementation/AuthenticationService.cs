using System.Threading.Tasks;
using Plugin.CurrentActivity;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.Droid.Views.Authentication;
using Schedulee.Droid.Views.Base;

namespace Schedulee.Droid.Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        public const int LoginRequestCode = 1;
        public const string TokenKey = "Token";
        private TaskCompletionSource<Token> _loginTask;
        private BaseAuthRequiredActivity _currentActivity;

        public Task<Token> Authenticate()
        {
            _loginTask = new TaskCompletionSource<Token>();
            _currentActivity = (BaseAuthRequiredActivity) CrossCurrentActivity.Current.Activity;
            _currentActivity.LoginCompleted += CurrentActivityOnLoginCompleted;
            _currentActivity.StartActivityForResult(typeof(LoginActivity), LoginRequestCode);
            return _loginTask.Task;
        }

        private void CurrentActivityOnLoginCompleted(object sender, Token token)
        {
            _loginTask.TrySetResult(token);
            _currentActivity.LoginCompleted -= CurrentActivityOnLoginCompleted;
        }
    }
}