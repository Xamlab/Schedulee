using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Managers;
using Schedulee.Droid.Views.Base;
using Schedulee.UI.Resources.Strings.Reservations;
using Schedulee.UI.ViewModels.Reservations;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Schedulee.Droid.Views.Reservations
{
    [Activity(Label = "Reservations", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ReservationsActivity : BaseAuthRequiredActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private DrawerLayout _drawerLayout;
        private TextView _userName;
        private IReservationsViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            BindingContext = _viewModel = ServiceLocater.Instance.Resolve<IReservationsViewModel>();

            SetContentView(Resource.Layout.activity_reservations);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            _drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            this.SetBinding(() => _viewModel.IsLoading, () => IsLoading, BindingMode.OneWay);
            LoadingMessage = Strings.Loading;
        }

        protected override void OnResume()
        {
            base.OnResume();
            _viewModel.LoadCommand.Execute(null);
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

        protected override void UpdateUser()
        {
            base.UpdateUser();
            if(_userName == null)
            {
                var headerView = FindViewById<NavigationView>(Resource.Id.nav_view).GetHeaderView(0);
                _userName = headerView.FindViewById<TextView>(Resource.Id.menu_header_name_text);
            }

            var account = SecureSettings.GetAccount();
            _userName.Text = account == null ? "" : $"{account.User.FirstName} {account.User.LastName}";
        }
    }
}