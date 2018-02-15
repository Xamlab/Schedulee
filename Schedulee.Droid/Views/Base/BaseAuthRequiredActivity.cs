using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Newtonsoft.Json;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Managers;
using Schedulee.Core.Models;
using Schedulee.Droid.Services.Implementation;

namespace Schedulee.Droid.Views.Base
{
    public class BaseAuthRequiredActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var authManager = ServiceLocater.Instance.Resolve<IAuthenticationManager>();
            if(authManager.State != SessionState.LoggedIn)
            {
                authManager.SignIn();
            }
        }

        public event EventHandler<Token> LoginCompleted;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == AuthenticationService.LoginRequestCode)
            {
                var token = JsonConvert.DeserializeObject<Token>(data.GetStringExtra(AuthenticationService.TokenKey));
                LoginCompleted?.Invoke(this, token);
            }
        }
    }
}