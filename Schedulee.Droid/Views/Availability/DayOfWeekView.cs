using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using PropertyChanged;
using Schedulee.Droid.Controls;
using Schedulee.Droid.Extensions;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    [AddINotifyPropertyChangedInterface]
    public class DayOfWeekView : BindableLinearLayout
    {
        private TextView _dayOfWeek;
        private LinearLayout _button;
        private IDayOfWeekViewModel _viewModel;
        private readonly List<Binding> _bindings = new List<Binding>();

        public DayOfWeekView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public DayOfWeekView(Context context) : base(context)
        {
            Create(context);
        }

        public DayOfWeekView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public DayOfWeekView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public DayOfWeekView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        private void Create(Context context)
        {
            var inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
            var rootView = inflater.Inflate(Resource.Layout.day_of_week_button_layout, this);
            _dayOfWeek = rootView.FindViewById<TextView>(Resource.Id.day_of_week_text);
            _button = rootView.FindViewById<LinearLayout>(Resource.Id.day_of_week_button_root_layout);
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
            if(BindingContext == null || !(BindingContext is IDayOfWeekViewModel viewModel)) return;
            _viewModel = viewModel;

            _bindings.Add(this.SetBinding(() => _viewModel.FormattedDay, () => _dayOfWeek.Text, BindingMode.OneWay));
            _bindings.Add(this.SetBinding(() => _viewModel.IsSelected, () => _button.Selected, BindingMode.OneWay));
        }
    }
}