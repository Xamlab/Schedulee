using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Droid.Controls;
using Schedulee.Droid.Extensions;
using Schedulee.UI.ViewModels.Availability;

namespace Schedulee.Droid.Views.Availability
{
    public class TimePeriodView : BindableLinearLayout
    {
        private TextView _formattedPeroids;
        private ImageButton _deleteButton;
        private ITimePeriodViewModel _viewModel;
        private readonly Action<ITimePeriodViewModel> _deleteAction;
        private readonly List<Binding> _bindings = new List<Binding>();

        public TimePeriodView(Context context, Action<ITimePeriodViewModel> deleteAction) : base(context)
        {
            _deleteAction = deleteAction;
            Create(context);
        }

        private void Create(Context context)
        {
            var inflater = (LayoutInflater) context.GetSystemService(Context.LayoutInflaterService);
            var rootView = inflater.Inflate(Resource.Layout.time_period_item_view, this);
            _formattedPeroids = rootView.FindViewById<TextView>(Resource.Id.time_period_formatted_periods_text);
            _deleteButton = rootView.FindViewById<ImageButton>(Resource.Id.delete_time_period_button);
            _deleteButton.Click += DeleteButtonOnClick;
        }

        private void DeleteButtonOnClick(object sender, EventArgs eventArgs)
        {
            _deleteAction?.Invoke(BindingContext as ITimePeriodViewModel);
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
            if(BindingContext == null || !(BindingContext is ITimePeriodViewModel viewModel)) return;
            _viewModel = viewModel;

            _bindings.Add(this.SetBinding(() => _viewModel.FormattedTimePeriod, () => _formattedPeroids.Text, BindingMode.OneTime));
        }
    }
}