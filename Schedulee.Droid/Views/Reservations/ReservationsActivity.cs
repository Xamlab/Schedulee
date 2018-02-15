using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Managers;
using Schedulee.Droid.Views.Base;

namespace Schedulee.Droid.Views.Reservations
{
    [Activity(Label = "Reservations", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ReservationsActivity : BaseAuthRequiredActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private DrawerLayout _drawerLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_reservations);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            _drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            var id = menuItem.ItemId;
            switch(id)
            {
                case Resource.Id.main_menu_logout:
                    var authManager = ServiceLocater.Instance.Resolve<IAuthenticationManager>();
                    authManager.SignOut();
                    break;
            }
            return true;
        }
    }
}