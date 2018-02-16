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
using Schedulee.Core.Models;
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

        private RecyclerView _reservationsHeaderRecyclerView;
        private ObservableRecyclerAdapter<IDateViewModel, CachingViewHolder> _reservationsHeaderAdapter;
        private LinearLayoutManager _reservationsHeaderLayoutManager;

        private RecyclerView _reservationsContentRecyclerView;
        private ObservableRecyclerAdapter<Reservation, CachingViewHolder> _reservationsContentAdapter;
        private LinearLayoutManager _reservationsContentLayoutManager;
        private TextView _emptyMessageText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _viewModel = ServiceLocater.Instance.Resolve<IReservationsViewModel>();
            _viewModel.PropertyChanged += OnPropertyChanged;

            SetContentView(Resource.Layout.activity_reservations);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            _drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            _emptyMessageText = FindViewById<TextView>(Resource.Id.reservation_content_empty_message_text);
            this.SetBinding(() => _viewModel.SelectedDate.Reservations, () => _emptyMessageText.Visibility)
                .ConvertSourceToTarget(reservations => reservations?.Any() == true ? ViewStates.Invisible : ViewStates.Visible);

            _reservationsHeaderRecyclerView = FindViewById<RecyclerView>(Resource.Id.reservation_dates_recyclerView);
            _reservationsHeaderLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
            _reservationsHeaderRecyclerView.SetLayoutManager(_reservationsHeaderLayoutManager);

            _reservationsContentRecyclerView = FindViewById<RecyclerView>(Resource.Id.reservation_content_recyclerView);
            _reservationsContentLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _reservationsContentRecyclerView.SetLayoutManager(_reservationsContentLayoutManager);
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
            _reservationsHeaderRecyclerView.SetAdapter(_reservationsHeaderAdapter);
            
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

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IReservationsViewModel.SelectedDate))
            {
                
                if(_viewModel.SelectedDate != null && _viewModel.SelectedDate.Reservations.Count > 0)
                {
                    _reservationsContentAdapter = _viewModel.SelectedDate.Reservations.GetRecyclerAdapter(BindViewHolder, Resource.Layout.reservation_content_layout);
                    _reservationsContentRecyclerView.SetAdapter(_reservationsContentAdapter);
                }
                else
                {
                    _reservationsContentRecyclerView.RemoveAllViews();
                }
            }
        }

        private void BindViewHolder(CachingViewHolder holder, Reservation reservation, int position)
        {
            var startTime = holder.FindCachedViewById<TextView>(Resource.Id.reservation_start_time_text);
            var name = holder.FindCachedViewById<TextView>(Resource.Id.reservation_client_name_text);
            var ratePerHour = holder.FindCachedViewById<TextView>(Resource.Id.reservation_rate_per_hour_text);

            startTime.Text = reservation.Start.ToString("hh:mm");
            name.Text = $"{reservation.Client.FirstName} {reservation.Client.LastName}";
            ratePerHour.Text = $"{reservation.RatePerHour:C}";
        }
    }
}