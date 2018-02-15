using System.Threading;
using System.Threading.Tasks;
using Schedulee.Core.Services;
using Schedulee.UI.Resources.Strings.Authentication;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Authentication.Implementation
{
    internal class LoginCommand : SaveAsyncCommand
    {
        private readonly LoginViewModel _viewModel;
        private readonly IApiClient _apiClient;

        protected override string OperationUnknownFailureMessage => Strings.LoginFailed;

        public LoginCommand(LoginViewModel viewModel, IApiClient apiClient, IDialogService dialogService) : base(viewModel, dialogService)
        {
            _apiClient = apiClient;
            _viewModel = viewModel;
        }

        protected override async Task<bool> ExecuteCoreAsync(object param, CancellationToken token = new CancellationToken())
        {
            var result = await _apiClient.LoginAsync(_viewModel.Email, _viewModel.Password, token);
            if(result == null) return false;
            _viewModel.StaleMonitor.StartMonitoring();
            _viewModel.InvokLoginCompleted(result);
            return true;
        }
    }
}