using System;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.Core.Managers;
using Schedulee.UI.Resources.Strings.Reservations;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base.Implementation;
using CommonStrings = Schedulee.UI.Resources.Strings.Common.Strings;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    internal class LogOutCommand : AsyncCommand
    {
        private readonly IAuthenticationManager _authManager;
        private readonly IDialogService _dialogService;

        public LogOutCommand(IAuthenticationManager authManager, IDialogService dialogService)
        {
            _authManager = authManager;
            _dialogService = dialogService;
        }

        public override async Task ExecuteAsync(object parameter, CancellationToken token = default(CancellationToken))
        {
            try
            {
                if(!await _dialogService.ShowConfirmationDialogAsync(Strings.LogOutConfirmation, CommonStrings.Yes, CommonStrings.No)) return;
                _authManager.SignOut();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                await _dialogService.ShowNotificationAsync(Strings.LogOutFailure, CommonStrings.Ok);
            }
        }
    }
}