using System;
using Acr.UserDialogs;
using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Schedulee.Core.DI;
using Schedulee.Core.Extensions.PubSub;
using Schedulee.Core.Managers;
using Schedulee.Core.Messages;

namespace Schedulee.Droid
{
	//You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        private IDependencyContainer _container;
        private IAuthenticationManager _authManager;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            UserDialogs.Init(() => CrossCurrentActivity.Current.Activity);
            //A great place to initialize Xamarin.Insights and Dependency Services!
            _container = Startup.SetupContainer();
            _authManager = _container.Resolve<IAuthenticationManager>();
            _authManager.RestoreSession();
            this.Subscribe<SessionStateChangedMessage>(OnSessionStateChanged);
        }

        private void OnSessionStateChanged(SessionStateChangedMessage state)
        {
            if(state.State == SessionState.LoggedOut)
            {
                _authManager.SignIn();
            }
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}