using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Newtonsoft.Json;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Models;
using Schedulee.Droid.Views.Base;
using Schedulee.UI.ViewModels.Reservations;

namespace Schedulee.Droid.Views.Reservations
{
    [Activity(Label = "Summary", Theme = "@style/AppTheme.ActionBar", ParentActivity = typeof(ReservationsActivity))]
    public class ReservationDetailActivity : BaseActivity
    {
        private IReservationDetailsViewModel _viewModel;
        private TextView _formattedFullDate;
        private TextView _formattedClient;
        private TextView _formattedTimePeriod;
        private TextView _phoneNumber;
        private TextView _location;
        private TextView _formattedVat;
        private TextView _formattedNetPrice;
        private TextView _formattedTotal;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            BindingContext = _viewModel = ServiceLocater.Instance.Resolve<IReservationDetailsViewModel>();
            SetContentView(Resource.Layout.reservation_detail_layout);
            var reservationJson = Intent.GetStringExtra(ReservationsActivity.ReservationDetailKey);
            var reservation = JsonConvert.DeserializeObject<Reservation>(reservationJson);
            _viewModel.Reservation = reservation;
            _viewModel.SetupCommand.Execute(null);

            _formattedFullDate = FindViewById<TextView>(Resource.Id.formatted_full_date_text);
            _formattedClient = FindViewById<TextView>(Resource.Id.formatted_client_text);
            _formattedTimePeriod = FindViewById<TextView>(Resource.Id.formatted_time_period_text);
            _phoneNumber = FindViewById<TextView>(Resource.Id.phone_number_text);
            _location = FindViewById<TextView>(Resource.Id.location_text);
            _formattedVat = FindViewById<TextView>(Resource.Id.vat_text);
            _formattedNetPrice = FindViewById<TextView>(Resource.Id.net_price_text);
            _formattedTotal = FindViewById<TextView>(Resource.Id.total_text);

            this.SetBinding(() => _viewModel.FormattedFullDate, () => _formattedFullDate.Text, BindingMode.OneWay);
            this.SetBinding(() => _viewModel.FormattedClient, () => _formattedClient.Text, BindingMode.OneWay);
            this.SetBinding(() => _viewModel.FormattedTimePeriod, () => _formattedTimePeriod.Text, BindingMode.OneWay);
            this.SetBinding(() => _viewModel.PhoneNumber, () => _phoneNumber.Text, BindingMode.OneWay);
            this.SetBinding(() => _viewModel.Location, () => _location.Text, BindingMode.OneWay);
            this.SetBinding(() => _viewModel.FormattedVat, () => _formattedVat.Text, BindingMode.OneWay);
            this.SetBinding(() => _viewModel.FormattedNetPrice, () => _formattedNetPrice.Text, BindingMode.OneWay);
            this.SetBinding(() => _viewModel.FormattedTotal, () => _formattedTotal.Text, BindingMode.OneWay);
        }

        public override bool NavigateUpTo(Intent upIntent)
        {
            Finish();
            return true;
        }
    }
}