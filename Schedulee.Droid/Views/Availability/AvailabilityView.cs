using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Droid.Controls;
using Schedulee.Droid.Extensions;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    public class AvailabilityView : BindableLinearLayout
    {
        private TextView _formattedDaysOfWeek;
        private TextView _formattedPeroids;
        private IAvailabilityViewModel _viewModel;
        private readonly List<Binding> _bindings = new List<Binding>();

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

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            _bindings.ClearBindings();
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            UpdateBindings();
        }

        protected override void OnBindingContextChanged()
        {
            UpdateBindings();
        }

        private void UpdateBindings()
        {
            _bindings.ClearBindings();
            if(BindingContext == null || !(BindingContext is IAvailabilityViewModel viewModel)) return;
            _viewModel = viewModel;

            _bindings.Add(this.SetBinding(() => _viewModel.FormattedDaysOfWeek, () => _formattedDaysOfWeek.Text, BindingMode.OneTime));
            _bindings.Add(this.SetBinding(() => _viewModel.FormattedTimePeriods, () => _formattedPeroids.Text, BindingMode.OneTime));
        }
    }
}