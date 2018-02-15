using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Managers;
using Schedulee.Droid.Views.Authentication;
using Schedulee.Droid.Views.Reservations;

namespace Schedulee.Droid.Views.Main
{
    [Activity(
        Theme = "@style/Theme.Splash",
        MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           StartActivity(typeof(ReservationsActivity));

            Finish();
        }
    }
}