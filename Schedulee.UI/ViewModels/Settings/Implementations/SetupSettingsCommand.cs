using Schedulee.Core.Extensions.PubSub;
using Schedulee.Core.Managers;
using Schedulee.Core.Messages;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Settings.Implementations
{
    internal class SetupSettingsCommand : Command
    {
        private readonly ISecureSettingsManager _secureSettings;
        private readonly SettingsViewModel _viewModel;

        public SetupSettingsCommand(SettingsViewModel viewModel, ISecureSettingsManager secureSettings)
        {
            _viewModel = viewModel;
            _secureSettings = secureSettings;
            this.Subscribe<SessionStateChangedMessage>(OnSessionStateChanged);
        }

        public override void Execute(object parameter)
        {
            var account = _secureSettings.GetAccount()?.User;
            _viewModel.Username = account?.Email;
            _viewModel.FirstName = account?.FirstName;
            _viewModel.LastName = account?.LastName;
            _viewModel.Location = account?.Location;
            _viewModel.SetTravelTime = account?.SetTravelTime ?? 0;
            _viewModel.StaleMonitor.StartMonitoring();
        }

        private void OnSessionStateChanged(SessionStateChangedMessage sessionStateChangedMessage)
        {
            Execute(null);
        }
    }
}