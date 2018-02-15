using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Newtonsoft.Json;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Extensions.PubSub;
using Schedulee.Core.Managers;
using Schedulee.Core.Messages;
using Schedulee.Core.Models;
using Schedulee.Droid.Services.Implementation;

namespace Schedulee.Droid.Views.Base
{
    public class BaseAuthRequiredActivity : BaseActivity
    {
        protected ISecureSettingsManager SecureSettings { get; private set; }
        protected IAuthenticationManager AuthManager { get; private set; }

        public event EventHandler<Token> LoginCompleted;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AuthManager = ServiceLocater.Instance.Resolve<IAuthenticationManager>();
            SecureSettings = ServiceLocater.Instance.Resolve<ISecureSettingsManager>();
            if(AuthManager.State != SessionState.LoggedIn)
            {
                AuthManager.SignIn();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            UpdateUser();
            this.Subscribe<SessionStateChangedMessage>(OnSessionStateChanged);
        }

        protected override void OnStop()
        {
            base.OnStop();
            this.Unsubscribe<SessionStateChangedMessage>();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == AuthenticationService.LoginRequestCode)
            {
                var token = JsonConvert.DeserializeObject<Token>(data.GetStringExtra(AuthenticationService.TokenKey));
                LoginCompleted?.Invoke(this, token);
            }
        }

        protected virtual void UpdateUser()
        {
        }

        protected virtual void OnSessionStateChanged(SessionStateChangedMessage sessionStateChangedMessage)
        {
            UpdateUser();
        }
    }
}