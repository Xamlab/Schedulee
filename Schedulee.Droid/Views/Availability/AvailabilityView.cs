using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Droid.Controls;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    public class AvailabilityView : BindableLinearLayout
    {
        private TextView _formattedDaysOfWeek;
        private TextView _formattedPeroids;
        private IAvailabilityViewModel _viewModel;

        public AvailabilityView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public AvailabilityView(Context context) : base(context)
        {
            Create(context);
        }

        public AvailabilityView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public AvailabilityView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public AvailabilityView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        private void Create(Context context)
        {
            var inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
            var rootView = inflater.Inflate(Resource.Layout.availability_item_view, this);
            _formattedDaysOfWeek = rootView.FindViewById<TextView>(Resource.Id.availability_formatted_days_of_week_text);
            _formattedPeroids = rootView.FindViewById<TextView>(Resource.Id.availability_formatted_periods_text);
        }

        protected override void OnBindingContextChanged()
        {
            if(BindingContext == null || !(BindingContext is IAvailabilityViewModel viewModel)) return;
            _viewModel = viewModel;

            this.SetBinding(() => _viewModel.FormattedDaysOfWeek, () => _formattedDaysOfWeek.Text, BindingMode.OneTime);
            this.SetBinding(() => _viewModel.FormattedTimePeriods, () => _formattedPeroids.Text, BindingMode.OneTime);
        }
    }
}