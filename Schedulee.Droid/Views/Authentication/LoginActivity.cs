using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Newtonsoft.Json;
using Schedulee.Core.DI.Implementation;
using Schedulee.Core.Models;
using Schedulee.Droid.Controls;
using Schedulee.Droid.Services.Implementation;
using Schedulee.Droid.Views.Base;
using Schedulee.UI.Resources.Strings.Authentication;
using Schedulee.UI.ViewModels.Authentication;

namespace Schedulee.Droid.Views.Authentication
{
    [Activity(Label = "Login",
        Theme = "@style/AppTheme.NoActionBar")]
    public class LoginActivity : BaseActivity
    {
        private EntryView _emailEntry;
        private EntryView _passwordEntry;
        private Button _loginButton;
        private ILoginViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen,
                            WindowManagerFlags.Fullscreen);

            SetContentView(Resource.Layout.activity_login);
            BindingContext = _viewModel = ServiceLocater.Instance.Resolve<ILoginViewModel>();
            _viewModel.LoginCompleted += ViewModelOnLoginCompleted;
            _emailEntry = FindViewById<EntryView>(Resource.Id.login_email_entry);
            _emailEntry.Entry.InputType = InputTypes.TextVariationEmailAddress | InputTypes.ClassText;
            _emailEntry.Entry.SetTextColor(Color.White);
            _emailEntry.Entry.SetBackgroundResource(Resource.Drawable.edit_text_default);

            _emailEntry.BindingContext = _viewModel;
            _emailEntry.ValidationIds = new[] {nameof(ILoginViewModel.Email)};
            _emailEntry.Title = Strings.Email;
            _emailEntry.SetErrorTextAppearance(Resource.Style.ErrorTextStyle);
            _emailEntry.SetHintTextAppearance(Resource.Style.HintTextStyle);
            this.SetBinding(() => _viewModel.Email, () => _emailEntry.Entry.Text, BindingMode.TwoWay);

            _passwordEntry = FindViewById<EntryView>(Resource.Id.login_password_entry);
            _passwordEntry.PasswordVisibilityToggleEnabled = true;
            _passwordEntry.Entry.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
            _passwordEntry.Entry.SetTextColor(Color.White);
            _passwordEntry.Entry.SetBackgroundResource(Resource.Drawable.edit_text_default);
            _passwordEntry.BindingContext = _viewModel;
            _passwordEntry.ValidationIds = new[] {nameof(ILoginViewModel.Password)};
            _passwordEntry.Title = Strings.Password;
            this.SetBinding(() => _viewModel.Password, () => _passwordEntry.Entry.Text, BindingMode.TwoWay);
            _passwordEntry.SetErrorTextAppearance(Resource.Style.ErrorTextStyle);
            _passwordEntry.SetHintTextAppearance(Resource.Style.HintTextStyle);
            _loginButton = FindViewById<Button>(Resource.Id.login_button);
            _loginButton.SetCommand(nameof(Button.Click), _viewModel.SaveCommand);

            this.SetBinding(() => _viewModel.IsSaving, () => IsLoading, BindingMode.OneWay);
            LoadingMessage = Strings.LoggingIn;
        }

        public override void OnBackPressed()
        {
        }

        private void ViewModelOnLoginCompleted(object sender, Token token)
        {
            var intent = new Intent();
            intent.PutExtra(AuthenticationService.TokenKey, JsonConvert.SerializeObject(token));
            SetResult(Result.Ok, intent);
            _viewModel.LoginCompleted -= ViewModelOnLoginCompleted;
            Finish();
        }
    }
}