using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
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
            this.SetBinding(() => _viewModel.IsLoading, () => IsLoading, BindingMode.OneWay);
            LoadingMessage = Strings.Loading;
        }

        protected override void OnResume()
        {
            base.OnResume();
            if(!_viewModel.IsLoaded) _viewModel.LoadCommand.Execute(null);
        }

        public override bool NavigateUpTo(Intent upIntent)
        {
            Finish();
            return true;
        }
    }
}