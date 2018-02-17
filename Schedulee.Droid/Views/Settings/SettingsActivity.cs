﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Core.DI.Implementation;
using Schedulee.Droid.Controls;
using Schedulee.Droid.Extensions;
using Schedulee.Droid.Views.Base;
using Schedulee.Droid.Views.Reservations;
using Schedulee.UI.Resources.Strings.Settings;
using Schedulee.UI.ViewModels.Settings;

namespace Schedulee.Droid.Views.Settings
{
    [Activity(Label = "Settings",
        Theme = "@style/AppTheme.ActionBar",
        ParentActivity = typeof(ReservationsActivity),
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingsActivity : BaseAuthRequiredActivity
    {
        private ISettingsViewModel _viewModel;
        private EntryView _usernameEntry;
        private EntryView _firstNameEntry;
        private EntryView _lastNameEntry;
        private EntryView _locationEntry;
        private EntryView _travelTimeEntry;
        private Button _saveButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            BindingContext = _viewModel = ServiceLocater.Instance.Resolve<ISettingsViewModel>();
            _viewModel.SetupCommand.Execute(null);

            SetContentView(Resource.Layout.activity_settings);

            _usernameEntry = FindViewById<EntryView>(Resource.Id.settings_username_entry);
            _usernameEntry.BindingContext = _viewModel;
            _usernameEntry.ValidationIds = new[] {nameof(ISettingsViewModel.Username)};
            _usernameEntry.Title = Strings.Username;
            _usernameEntry.Entry.Enabled = false;
            _usernameEntry.SetErrorTextAppearance(Resource.Style.ErrorTextStyle);
            _usernameEntry.SetHintTextAppearance(Resource.Style.HintTextStyle);

            _firstNameEntry = FindViewById<EntryView>(Resource.Id.settings_first_name_entry);
            _firstNameEntry.BindingContext = _viewModel;
            _firstNameEntry.ValidationIds = new[] {nameof(ISettingsViewModel.FirstName)};
            _firstNameEntry.Title = Strings.FirstName;
            _firstNameEntry.SetErrorTextAppearance(Resource.Style.ErrorTextStyle);
            _firstNameEntry.SetHintTextAppearance(Resource.Style.HintTextStyle);

            _lastNameEntry = FindViewById<EntryView>(Resource.Id.settings_last_name_entry);
            _lastNameEntry.BindingContext = _viewModel;
            _lastNameEntry.ValidationIds = new[] {nameof(ISettingsViewModel.LastName)};
            _lastNameEntry.Title = Strings.LastName;
            _lastNameEntry.SetErrorTextAppearance(Resource.Style.ErrorTextStyle);
            _lastNameEntry.SetHintTextAppearance(Resource.Style.HintTextStyle);

            _locationEntry = FindViewById<EntryView>(Resource.Id.settings_location_entry);
            _locationEntry.BindingContext = _viewModel;
            _locationEntry.ValidationIds = new[] {nameof(ISettingsViewModel.Location)};
            _locationEntry.Title = Strings.Location;
            _locationEntry.SetErrorTextAppearance(Resource.Style.ErrorTextStyle);
            _locationEntry.SetHintTextAppearance(Resource.Style.HintTextStyle);

            _travelTimeEntry = FindViewById<EntryView>(Resource.Id.settings_travel_time_entry);
            _travelTimeEntry.BindingContext = _viewModel;
            _travelTimeEntry.ValidationIds = new[] {nameof(ISettingsViewModel.SetTravelTime)};
            _travelTimeEntry.Title = Strings.SetTravelTime;
            _travelTimeEntry.Entry.InputType = InputTypes.NumberVariationNormal | InputTypes.ClassNumber;
            _travelTimeEntry.SetErrorTextAppearance(Resource.Style.ErrorTextStyle);
            _travelTimeEntry.SetHintTextAppearance(Resource.Style.HintTextStyle);

            _saveButton = FindViewById<Button>(Resource.Id.settings_save_button);
            Overlay = FindViewById<View>(Resource.Id.settings_loading_overlay);
            Progress = FindViewById<ProgressBar>(Resource.Id.settings_loading_progress);

            this.SetBindingEx(() => _viewModel.Username, () => _usernameEntry.Entry.Text, BindingMode.OneWay);
            this.SetBindingEx(() => _viewModel.FirstName, () => _firstNameEntry.Entry.Text, BindingMode.TwoWay);
            this.SetBindingEx(() => _viewModel.LastName, () => _lastNameEntry.Entry.Text, BindingMode.TwoWay);
            this.SetBindingEx(() => _viewModel.Location, () => _locationEntry.Entry.Text, BindingMode.TwoWay);
            this.SetBindingEx(() => _viewModel.SetTravelTime, () => _travelTimeEntry.Entry.Text, BindingMode.TwoWay);
            this.SetBindingEx(() => _viewModel.IsSaving, () => IsLoading, BindingMode.OneWay);
            _saveButton.SetCommand(nameof(Button.Click), _viewModel.SaveCommand);

            LoadingMessage = Strings.Saving;
        }

        public override bool NavigateUpTo(Intent upIntent)
        {
            Finish();
            return true;
        }
    }
}