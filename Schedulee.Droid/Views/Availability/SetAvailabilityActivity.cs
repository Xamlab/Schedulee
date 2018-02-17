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
using Schedulee.Core.Models;
using Schedulee.Droid.Extensions;
using Schedulee.Droid.Views.Base;
using Schedulee.Droid.Views.Reservations;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Availability;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.Droid.Views.Availability
{
    [Activity(Label = "Set Availability",
        Theme = "@style/AppTheme.ActionBar",
        ParentActivity = typeof(ReservationsActivity),
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SetAvailabilityActivity : BaseAuthRequiredActivity
    {
        private ISetAvailabilityViewModel _viewModel;
        private IDialogService _dialogService;
        private AvailabilitiesView _availabilities;
        private TimePeriodsView _timePeriods;
        private DaysOfWeekView _daysOfWeek;
        private Button _addTimeAvailableButton;
        private View _overlay;
        private ConstraintLayout _addAvailabilityView;
        private TextView _addTimePeriodButton;
        private TextView _cancelButton;
        private TimePeriod _newPeriod;
        private Button _saveButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            BindingContext = _viewModel = ServiceLocater.Instance.Resolve<ISetAvailabilityViewModel>();
            _viewModel.SetupCommand.Execute(null);

            _dialogService = ServiceLocater.Instance.Resolve<IDialogService>();
            SetContentView(Resource.Layout.activity_set_availability);

            _overlay = FindViewById<View>(Resource.Id.set_availability_overlay);
            _overlay.Clickable = true;
            _addAvailabilityView = FindViewById<ConstraintLayout>(Resource.Id.set_availability_add_availability_view);

            _availabilities = FindViewById<AvailabilitiesView>(Resource.Id.set_availability_availabilities_view);
            _availabilities.BindingContext = _viewModel;
            this.SetBindingEx(() => _viewModel.Items, () => _availabilities.Items, BindingMode.OneWay)
                .ConvertSourceToTarget(list => list as IEnumerable<IAvailabilityViewModel>);

            _daysOfWeek = FindViewById<DaysOfWeekView>(Resource.Id.set_availability_days_of_week_view);
            _daysOfWeek.BindingContext = _viewModel;
            this.SetBindingEx(() => _viewModel.DaysOfWeek, () => _daysOfWeek.Items, BindingMode.OneWay)
                .ConvertSourceToTarget(list => list as IEnumerable<IDayOfWeekViewModel>);
            _daysOfWeek.ItemClicked += DaysOfWeekOnItemClicked;

            _addTimeAvailableButton = FindViewById<Button>(Resource.Id.set_availability_add_time_available_button);
            _addTimeAvailableButton.SetCommand(nameof(Button.Click), _viewModel.AddTimeAvailableCommand);
            _viewModel.DidBeginAddingTimePeriod += OnDidBeginAddingTimePeriod;
            _viewModel.DidCancelAddingTimePeriod += OnDidCancelOrCreateAvailability;
            _viewModel.DidCreateTimeAvailability += OnDidCancelOrCreateAvailability;

            Overlay = FindViewById<View>(Resource.Id.set_availability_loading_overlay);
            Progress = FindViewById<ProgressBar>(Resource.Id.set_availability_loading_progress);
            this.SetBindingEx(() => _viewModel.InProgress, () => IsLoading, BindingMode.OneWay);
            LoadingMessage = Strings.Loading;

            _addTimePeriodButton = FindViewById<TextView>(Resource.Id.set_availability_add_time_period_button);
            _addTimePeriodButton.Click += AddTimePeriodButtonOnClick;

            _cancelButton = FindViewById<TextView>(Resource.Id.set_availability_cancel_button);
            _cancelButton.SetCommand(nameof(Button.Click), _viewModel.CancelCommand);

            _saveButton = FindViewById<Button>(Resource.Id.set_availability_save_button);
            _saveButton.SetCommand(nameof(Button.Click), _viewModel.SaveCommand);

            _timePeriods = FindViewById<TimePeriodsView>(Resource.Id.set_availability_time_periods_view);
            _timePeriods.BindingContext = _viewModel;
            this.SetBindingEx(() => _viewModel.TimePeriods, () => _timePeriods.Items, BindingMode.OneWay)
                .ConvertSourceToTarget(list => list as IEnumerable<ITimePeriodViewModel>);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _daysOfWeek.ItemClicked -= DaysOfWeekOnItemClicked;
            _viewModel.DidBeginAddingTimePeriod -= OnDidBeginAddingTimePeriod;
            _viewModel.DidCancelAddingTimePeriod -= OnDidCancelOrCreateAvailability;
            _viewModel.DidCreateTimeAvailability -= OnDidCancelOrCreateAvailability;
            _addTimePeriodButton.Click -= AddTimePeriodButtonOnClick;
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

        private void DaysOfWeekOnItemClicked(object sender, EventArgs args)
        {
            _viewModel.ToggleDayCommand.Execute(sender);
        }

        private void OnDidBeginAddingTimePeriod(object sender, EventArgs eventArgs)
        {
            ShowAddTimeAvailableView();
        }

        private void OnDidCancelOrCreateAvailability(object sender, EventArgs eventArgs)
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

        private async void AddTimePeriodButtonOnClick(object sender, EventArgs eventArgs)
        {
            if(!_viewModel.AddTimePeriodCommand.CanExecute(null)) return;
            _viewModel.AddTimePeriodCommand.Execute(new TimePeriod(DateTime.Now, DateTime.Now.AddHours(1)));
            return;
            var shouldContinue = await _dialogService.ShowConfirmationDialogAsync(Strings.TimePeriod,
                                                                                  Strings.PickStartTime,
                                                                                  CommonStrings.Ok, CommonStrings.Cancel);
            if(!shouldContinue) return;
            _newPeriod = new TimePeriod();
            var hour = DateTime.Now.Date.Hour;
            var min = DateTime.Now.Date.Minute;
            var timePickingDialog = new TimePickerDialog(this, OnStartTimePicked, hour, min, false);
            timePickingDialog.Show();
        }

        private async void OnStartTimePicked(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            _newPeriod.Start = new DateTime(1, 1, 1, e.HourOfDay, e.Minute, 0);
            var shouldContinue = await _dialogService.ShowConfirmationDialogAsync(Strings.TimePeriod,
                                                                                  Strings.PickEndTime,
                                                                                  CommonStrings.Ok, CommonStrings.Cancel);
            if(!shouldContinue)
            {
                _newPeriod = null;
                return;
            }

            var hour = DateTime.Now.Date.Hour;
            var min = DateTime.Now.Date.Minute;
            var endTimePickingDialog = new TimePickerDialog(this, OnEndTimePicked, hour, min, false);
            endTimePickingDialog.Show();
        }

        private void OnEndTimePicked(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            _newPeriod.End = new DateTime(1, 1, 1, e.HourOfDay, e.Minute, 0);
            _viewModel.AddTimePeriodCommand.Execute(_newPeriod);
            _newPeriod = null;
        }
    }
}