using System.Threading;
using System.Threading.Tasks;
using Schedulee.Core.Managers;
using Schedulee.Core.Services;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Settings.Implementations
{
    internal class SaveAccountCommand : SaveAsyncCommand
    {
        private readonly SettingsViewModel _viewModel;
        private readonly IApiClient _apiClient;
        private readonly ISecureSettingsManager _secureSettings;

        public SaveAccountCommand(SettingsViewModel viewModel,
                                  IApiClient apiClient,
                                  ISecureSettingsManager secureSettings,
                                  IDialogService dialogService) : base(viewModel, dialogService)
        {
            _secureSettings = secureSettings;
            _apiClient = apiClient;
            _viewModel = viewModel;
        }

        protected override async Task<bool> ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken))
        {
            await _apiClient.SaveAccountAsync(_viewModel.FirstName, _viewModel.LastName, _viewModel.Location, _viewModel.SetTravelTime, token);
            var account = _secureSettings.GetAccount();
            var user = account.User;
            user.FirstName = _viewModel.FirstName;
            user.LastName = _viewModel.LastName;
            user.Location = _viewModel.Location;
            user.SetTravelTime = _viewModel.SetTravelTime;
            _secureSettings.SetAccount(account);
            return true;
        }
    }
}