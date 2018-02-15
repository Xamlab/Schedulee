using System.Windows.Input;
using Schedulee.Core.Managers;
using Schedulee.Core.Services;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Authentication;
using Schedulee.UI.ViewModels.Base;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Settings.Implementations
{
    internal class SettingsViewModel : BaseViewModel, ISettingsViewModel, IInternalSaveableViewModel
    {
        public SettingsViewModel(ISecureSettingsManager secureSettings, IApiClient apiClient, IDialogService dialogService)
        {
            SetupCommand = new SetupSettingsCommand(this, secureSettings);
            SaveCommand = new SaveAccountCommand(this, apiClient, secureSettings, dialogService);
            StaleMonitor = new SettingsStaleMonitor(this);
            Validator = new SettingsValidator(this);
        }

        public ICommand SetupCommand { get; }
        public IAsyncCommand SaveCommand { get; }
        public IViewModelValidator Validator { get; }
        public IStaleMonitor StaleMonitor { get; }
        public bool IsSaving { get; set; }
        public bool DidSave { get; set; }
        public string SavingFailureMessage { get; set; }
        public string Username { get; internal set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public int SetTravelTime { get; set; }
    }
} 