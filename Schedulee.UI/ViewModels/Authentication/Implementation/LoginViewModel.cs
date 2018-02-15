using System;
using PropertyChanged;
using Schedulee.Core.Models;
using Schedulee.Core.Services;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Authentication.Implementation
{
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel : BaseViewModel, ILoginViewModel, IInternalSaveableViewModel
    {
        public LoginViewModel(IApiClient apiClient, IDialogService dialogService)
        {
            SaveCommand = new LoginCommand(this, apiClient, dialogService);
            StaleMonitor = new LoginStaleMonitor(this);
            Validator = new LoginValidator(this);
            StaleMonitor.StartMonitoring();
        }

        public IAsyncCommand SaveCommand { get; }
        public IStaleMonitor StaleMonitor { get; }
        public IViewModelValidator Validator { get; }
        public string Email { get; set; }
        public string Password { get; set; }
        public event EventHandler<Token> LoginCompleted;
        public bool IsSaving { get; set; }
        public bool DidSave { get; set; }
        public string SavingFailureMessage { get; set; }

        internal void InvokLoginCompleted(Token token)
        {
            LoginCompleted?.Invoke(this, token);
        }
    }
}