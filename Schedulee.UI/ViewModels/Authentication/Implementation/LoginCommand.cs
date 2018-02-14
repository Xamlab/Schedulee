using System.Threading;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Authentication;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Authentication.Implementation
{
    internal class LoginCommand : SaveAsyncCommand
    {
        private readonly LoginViewModel _viewModel;

        protected override string OperationUnknownFailureMessage => Strings.LoginFailed;

        public LoginCommand(LoginViewModel viewModel, IDialogService dialogService) : base(viewModel, dialogService)
        {
            _viewModel = viewModel;
        }

        protected override async Task<bool> ExecuteCoreAsync(object param, CancellationToken token = new CancellationToken())
        {
            return true;
        }
    }
}
