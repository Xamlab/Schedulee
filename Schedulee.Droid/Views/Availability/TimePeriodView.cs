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
    public class TimePeriodView : BindableLinearLayout
    {
        private TextView _formattedPeroids;
        private ITimePeriodViewModel _viewModel;
        
        public TimePeriodView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public TimePeriodView(Context context) : base(context)
        {
            Create(context);
        }

        public TimePeriodView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public TimePeriodView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public TimePeriodView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        private void Create(Context context)
        {
            var inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
            var rootView = inflater.Inflate(Resource.Layout.time_period_item_view, this);
            _formattedPeroids = rootView.FindViewById<TextView>(Resource.Id.time_period_formatted_periods_text);
        }

        protected override void OnBindingContextChanged()
        {
            if(BindingContext == null || !(BindingContext is ITimePeriodViewModel viewModel)) return;
            _viewModel = viewModel;

            this.SetBinding(() => _viewModel.FormattedTimePeriod, () => _formattedPeroids.Text, BindingMode.OneTime);
        }
    }
}