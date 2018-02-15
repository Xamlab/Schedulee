using Android.App;
using Android.OS;
using Android.Text;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using Schedulee.Core.DI.Implementation;
using Schedulee.Droid.Controls;
using Schedulee.UI.Resources.Strings.Authentication;
using Schedulee.UI.ViewModels.Authentication;

namespace Schedulee.Droid.Views
{
    [Activity(Label = "Login", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class LoginActivity : Activity
    {
        private EntryView _emailEntry;
        private EntryView _passwordEntry;
        private Button _loginButton;
        private ILoginViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            _viewModel = ServiceLocater.Instance.Resolve<ILoginViewModel>();

            _emailEntry = FindViewById<EntryView>(Resource.Id.login_email_entry);
            _emailEntry.Entry.InputType = InputTypes.TextVariationEmailAddress;
            _emailEntry.BindingContext = _viewModel;
            _emailEntry.ValidationIds = new[] {nameof(ILoginViewModel.Email)};
            _emailEntry.Title = Strings.Email;
            this.SetBinding(() => _viewModel.Email, () => _emailEntry.Entry.Text, BindingMode.TwoWay);

            _passwordEntry = FindViewById<EntryView>(Resource.Id.login_password_entry);
            _passwordEntry.Entry.InputType = InputTypes.TextVariationWebPassword;
            _passwordEntry.BindingContext = _viewModel;
            _passwordEntry.ValidationIds = new[] { nameof(ILoginViewModel.Password) };
            _passwordEntry.Title = Strings.Password;
            this.SetBinding(() => _viewModel.Password, () => _passwordEntry.Entry.Text, BindingMode.TwoWay);

            _loginButton = FindViewById<Button>(Resource.Id.login_button);
            _loginButton.SetCommand(nameof(Button.Click), _viewModel.SaveCommand);

        }
    }
}