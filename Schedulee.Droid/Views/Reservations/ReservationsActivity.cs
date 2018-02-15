﻿using System.Linq;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Managers;
using Schedulee.Droid.Views.Base;
using Schedulee.UI.ViewModels.Reservations;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Schedulee.Droid.Views.Reservations
{
    [Activity(Label = "Reservations", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ReservationsActivity : BaseAuthRequiredActivity, NavigationView.IOnNavigationItemSelectedListener, View.IOnClickListener
    {
        private DrawerLayout _drawerLayout;
        private TextView _userName;
        private IReservationsViewModel _viewModel;

        private RecyclerView _reservationsHeadeRecyclerView;
        private ObservableRecyclerAdapter<IDateViewModel, CachingViewHolder> _reservationsHeaderAdapter;
        private LinearLayoutManager _layoutManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _viewModel = ServiceLocater.Instance.Resolve<IReservationsViewModel>();

            SetContentView(Resource.Layout.activity_reservations);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            _drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            _reservationsHeadeRecyclerView = FindViewById<RecyclerView>(Resource.Id.reservation_dates_recyclerView);
            _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
            _reservationsHeadeRecyclerView.SetLayoutManager(_layoutManager);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Load();
        }

        private async void Load()
        {
            await _viewModel.LoadCommand.ExecuteAsync(null);
            _reservationsHeaderAdapter = _viewModel.Items.GetRecyclerAdapter(BindHeaderViewHolder, Resource.Layout.reservation_date_layout);
            _reservationsHeadeRecyclerView.SetAdapter(_reservationsHeaderAdapter);
        }

        private void BindHeaderViewHolder(CachingViewHolder holder, IDateViewModel viewModel, int position)
        {
            var dayOfWeek = holder.FindCachedViewById<TextView>(Resource.Id.day_of_week_text);
            var day = holder.FindCachedViewById<TextView>(Resource.Id.day_text);
            var reservationExistIdicator = holder.FindCachedViewById<View>(Resource.Id.reservation_indicator);
            var rootLayout = holder.FindCachedViewById<LinearLayout>(Resource.Id.reservation_date_layout_linear_layout);

            holder.DeleteBinding(rootLayout);

            dayOfWeek.Text = viewModel.DayOfWeek;
            day.Text = viewModel.Day;
            reservationExistIdicator.Visibility = viewModel.Reservations?.Any() == true ? ViewStates.Visible : ViewStates.Invisible;

            rootLayout.SetTag(Resource.Integer.date_selection_key, position);
            rootLayout.SetOnClickListener(this);

            var backgroundColorBinding = new Binding<bool, Drawable>(viewModel,
                                                                     () => viewModel.IsSelected,
                                                                     rootLayout,
                                                                     () => rootLayout.Background,
                                                                     BindingMode.OneWay).ConvertSourceToTarget(ConvertIsSelectedToBackgroundColor);

            holder.SaveBinding(rootLayout, backgroundColorBinding);
        }

        private Drawable ConvertIsSelectedToBackgroundColor(bool isSelected)
        {
            return new ColorDrawable(isSelected ? Color.ParseColor("#50D2C2") : Color.White);
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

        public void OnClick(View view)
        {
            var index = (int) view.GetTag(Resource.Integer.date_selection_key);
            var date = _viewModel.Items[index];
            _viewModel.SelectDateCommand.Execute(date);
        }
    }
}