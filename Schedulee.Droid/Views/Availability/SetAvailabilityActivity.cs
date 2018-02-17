using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Core.DI.Implementation;
using Schedulee.Droid.Views.Base;
using Schedulee.Droid.Views.Reservations;
using Schedulee.UI.Resources.Strings.Availability;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    [Activity(Label = "Set Availability", Theme = "@style/AppTheme.ActionBar", ParentActivity = typeof(ReservationsActivity))]
    public class SetAvailabilityActivity : BaseAuthRequiredActivity
    {
        private ISetAvailabilityViewModel _viewModel;
        private AvailabilitiesView _availabilities;
        private TimePeriodsView _timePeriods;
        private DaysOfWeekView _daysOfWeek;
        private TextView _addTimePeriod;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            BindingContext = _viewModel = ServiceLocater.Instance.Resolve<ISetAvailabilityViewModel>();

            SetContentView(Resource.Layout.activity_set_availability);

            _availabilities = FindViewById<AvailabilitiesView>(Resource.Id.set_availability_availabilities_view);
            _availabilities.BindingContext = _viewModel;
            this.SetBinding(() => _viewModel.Items, () => _availabilities.Items, BindingMode.OneWay)
                .ConvertSourceToTarget(list => list as IEnumerable<IAvailabilityViewModel>);

            _daysOfWeek = FindViewById<DaysOfWeekView>(Resource.Id.set_availability_days_of_week_view);
            _daysOfWeek.BindingContext = _viewModel;
            this.SetBinding(() => _viewModel.DaysOfWeek, () => _daysOfWeek.Items, BindingMode.OneWay)
                .ConvertSourceToTarget(list => list as IEnumerable<IDayOfWeekViewModel>);
            _daysOfWeek.ItemClicked += DaysOfWeekOnItemClicked;

            _addTimePeriod = FindViewById<TextView>(Resource.Id.add_time_period_button);
            _addTimePeriod.SetCommand(nameof(TextView.Click), _viewModel.AddTimePeriodCommand);

            this.SetBinding(() => _viewModel.IsLoading, () => IsLoading, BindingMode.OneWay);
            LoadingMessage = Strings.Loading;

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
                _viewModel.AddTimeAvailableCommand.Execute(null);
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
    }
}