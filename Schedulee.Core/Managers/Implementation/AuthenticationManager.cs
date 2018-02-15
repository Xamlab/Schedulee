using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Schedulee.Core.Extensions.PubSub;
using Schedulee.Core.Messages;
using Schedulee.Core.Services;

namespace Schedulee.Core.Managers.Implementation
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private SessionState _state;
        private readonly IAuthenticationService _authService;
        private readonly ISecureSettingsManager _secureStorage;
        private Task _signInTask;
        private readonly object _signInLocker = new object();
        private readonly object _signOutLocker = new object();
        private readonly object _restoreLocker = new object();

        public AuthenticationManager(IAuthenticationService authService,
                                     ISecureSettingsManager secureStorage)
        {
            _secureStorage = secureStorage;
            _authService = authService;
        }

        public SessionState State
        {
            get => _state;
            private set
            {
                if(_state != value)
                {
                    _state = value;
                    this.Publish(new SessionStateChangedMessage(_state));
                }
            }
        }

        public Task SignIn()
        {
            lock(_signInLocker)
            {
                if(_signInTask != null) return _signInTask;
                _signInTask = SignInInternal();
                return _signInTask;
            }
        }

        public void SignOut()
        {
            lock(_signOutLocker)
            {
                _secureStorage.Clear();
                State = SessionState.LoggedOut;
            }
        }

        public void RestoreSession()
        {
            lock(_restoreLocker)
            {
                var account = _secureStorage.GetAccount();
                if(account?.AccessToken != null)
                {
                    State = SessionState.LoggedIn;
                }
                else
                {
                    State = SessionState.LoggedOut;
                }
            }
        }

        private async Task SignInInternal()
        {
            try
            {
                var result = await _authService.Authenticate();

                if(result != null)
                {
                    _secureStorage.SetAccount(result);
                    State = SessionState.LoggedIn;
                }
                else
                {
                    State = SessionState.LoggedOut;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                _signInTask = null;
            }
        }
    }
}