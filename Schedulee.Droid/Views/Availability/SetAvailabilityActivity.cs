using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Constraints;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Core.DI.Implementation;
using Schedulee.Droid.Extensions;
using Schedulee.Droid.Views.Base;
using Schedulee.Droid.Views.Reservations;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    [Activity(Label = "Set Availability",
        Theme = "@style/AppTheme.ActionBar",
        ParentActivity = typeof(ReservationsActivity),
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SetAvailabilityActivity : BaseAuthRequiredActivity
    {
        private ISetAvailabilityViewModel _viewModel;
        private AvailabilitiesView _availabilities;
        private TimePeriodsView _timePeriods;
        private DaysOfWeekView _daysOfWeek;
        private Button _addTimeAvailableButton;
        private View _overlay;
        private ConstraintLayout _addAvailabilityView;
        private TextView _addTimePeriodButton;
        private TextView _cancelButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            BindingContext = _viewModel = ServiceLocater.Instance.Resolve<ISetAvailabilityViewModel>();
            SetContentView(Resource.Layout.activity_set_availability);

            _overlay = FindViewById<View>(Resource.Id.set_availability_overlay);
            _overlay.Clickable = true;
            _addAvailabilityView = FindViewById<ConstraintLayout>(Resource.Id.set_availability_add_availability_view);

            _availabilities = FindViewById<AvailabilitiesView>(Resource.Id.set_availability_availabilities_view);
            _availabilities.BindingContext = _viewModel;
            this.SetBinding(() => _viewModel.Items, () => _availabilities.Items, BindingMode.OneWay)
                .ConvertSourceToTarget(list => list as IEnumerable<IAvailabilityViewModel>);

            _daysOfWeek = FindViewById<DaysOfWeekView>(Resource.Id.set_availability_days_of_week_view);
            _daysOfWeek.BindingContext = _viewModel;
            this.SetBinding(() => _viewModel.DaysOfWeek, () => _daysOfWeek.Items, BindingMode.OneWay)
                .ConvertSourceToTarget(list => list as IEnumerable<IDayOfWeekViewModel>);
            _daysOfWeek.ItemClicked += DaysOfWeekOnItemClicked;

            _addTimeAvailableButton = FindViewById<Button>(Resource.Id.set_availability_add_time_available_button);
            _addTimeAvailableButton.SetCommand(nameof(Button.Click), _viewModel.AddTimeAvailableCommand);
            _viewModel.DidBeginAddingTimePeriod += OnDidBeginAddingTimePeriod;
            _viewModel.DidCancelAddingTimePeriod += OnDidCancelAddingTimePeriod;

            Overlay = FindViewById<View>(Resource.Id.set_availability_loading_overlay);
            Progress = FindViewById<ProgressBar>(Resource.Id.set_availability_loading_progress);
            this.SetBinding(() => _viewModel.IsLoading, () => IsLoading, BindingMode.OneWay);
            LoadingMessage = Strings.Loading;

            _addTimePeriodButton = FindViewById<TextView>(Resource.Id.add_time_period_button);
            _addTimePeriodButton.SetCommand(nameof(TextView.Click), _viewModel.AddTimePeriodCommand);

            _cancelButton = FindViewById<TextView>(Resource.Id.set_availability_cancel_button);
            _cancelButton.SetCommand(nameof(Button.Click), _viewModel.CancelCommand);

            _timePeriods = FindViewById<TimePeriodsView>(Resource.Id.set_availability_time_periods_view);
            _timePeriods.BindingContext = _viewModel;
            this.SetBinding(() => _viewModel.TimePeriods, () => _timePeriods.Items, BindingMode.OneWay)
                .ConvertSourceToTarget(list => list as IEnumerable<ITimePeriodViewModel>);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Load();
        }

        private async void Load()
        {
            if(!_viewModel.IsLoaded)
            {
                await _viewModel.LoadCommand.ExecuteAsync(null);
            }
        }

        public override bool NavigateUpTo(Intent upIntent)
        {
            Finish();
            return true;
        }

        private void DaysOfWeekOnItemClicked(object sender, EventArgs eventArgs)
        {
            _viewModel.ToggleDayCommand.Execute(sender);
        }

        private void OnDidBeginAddingTimePeriod(object sender, EventArgs eventArgs)
        {
            ShowAddTimeAvailableView();
        }

        private void OnDidCancelAddingTimePeriod(object sender, EventArgs eventArgs)
        {
            HideAddTimeAvailableView();
        }

        private void ShowAddTimeAvailableView()
        {
            this.ShowOverlay(_overlay);
            _addAvailabilityView.Visibility = ViewStates.Visible;
            var slideInAddAnimationView = AnimationUtils.LoadAnimation(this, Resource.Animation.set_availability_add_availability_slide_in_animation);
            _addAvailabilityView.StartAnimation(slideInAddAnimationView);
        }

        private void HideAddTimeAvailableView()
        {
            this.HideOverlay(_overlay);
            var slideInAddAnimationView = AnimationUtils.LoadAnimation(this, Resource.Animation.set_availability_add_availability_slide_out_animation);
            _addAvailabilityView.StartAnimation(slideInAddAnimationView);
            _overlay.Visibility = ViewStates.Invisible;
            _addAvailabilityView.Visibility = ViewStates.Invisible;
        }
    }
}