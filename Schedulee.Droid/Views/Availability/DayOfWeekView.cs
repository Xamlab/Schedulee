using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using PropertyChanged;
using Schedulee.Droid.Controls;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    [AddINotifyPropertyChangedInterface]
    public class DayOfWeekView : BindableLinearLayout
    {
        private TextView _dayOfWeek;
        private LinearLayout _button;
        private IDayOfWeekViewModel _viewModel;

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

        private bool _isSelected;
        public bool IsSelected  
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if(_isSelected != value)
                {
                    _isSelected = value;
                    _button.Selected = _isSelected;
                }
            }
        }

        private void Create(Context context)
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            var rootView = inflater.Inflate(Resource.Layout.day_of_week_button_layout, this);
            _dayOfWeek = rootView.FindViewById<TextView>(Resource.Id.day_of_week_text);
            _button = rootView.FindViewById<LinearLayout>(Resource.Id.day_of_week_button_root_layout);
        }

        protected override void OnBindingContextChanged()
        {
            if (BindingContext == null || !(BindingContext is IDayOfWeekViewModel viewModel)) return;
            _viewModel = viewModel;

            this.SetBinding(() => _viewModel.FormattedDay, () => _dayOfWeek.Text, BindingMode.OneWay);
            this.SetBinding(() => _viewModel.IsSelected, () => IsSelected, BindingMode.OneWay);
        }
    }
}