using Android.App;
using Android.OS;
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
        private ILoginViewModel _viewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);
            _viewModel = ServiceLocater.Instance.Resolve<ILoginViewModel>();
            _emailEntry = FindViewById<EntryView>(Resource.Id.email_entry);
            _emailEntry.BindingContext = _viewModel;
            _emailEntry.ValidationIds = new[] {nameof(ILoginViewModel.Email)};
            _emailEntry.Title = Strings.Email;

            this.SetBinding(() => _viewModel.Email, () => _emailEntry.Entry.Text, BindingMode.TwoWay);
        }
    }
}