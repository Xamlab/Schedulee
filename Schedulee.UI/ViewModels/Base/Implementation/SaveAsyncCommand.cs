using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Common;
using Schedulee.UI.Services;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    internal abstract class SaveAsyncCommand : AsyncCommand
    {
        private readonly IInternalSaveableViewModel _viewModel;
        private readonly IDialogService _dialogService;

        protected virtual string OperationUnknownFailureMessage => Strings.OperationUnknownFailure;
        protected virtual string OperationNetworkFailureMessage => Strings.OperationNetworkFailure;

        protected SaveAsyncCommand(IInternalSaveableViewModel viewModel, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object param, CancellationToken token = default(CancellationToken))
        {
            SetCanExecute(false);
            _viewModel.SavingFailureMessage = null;

            try
            {
                if(_viewModel.Validator != null && !_viewModel.Validator.Validate())
                {
                    var errors = _viewModel.Validator.GetAllErrorsInString();
                    var message = Strings.FixValidationErrors + "\n" + string.Join("\n", errors);
                    await _dialogService.ShowNotificationAsync(message, Strings.Ok);
                    return;
                }

                if(await ShouldExecuteCore())
                {
                    _viewModel.IsSaving = true;
                    _viewModel.DidSave = await ExecuteCoreAsync(param, token);
                }
            }
            catch(Exception ex)
            {
                if(!HandleException(ex, token)) throw;
            }
            finally
            {
                _viewModel.IsSaving = false;
                SetCanExecute(true);
            }
        }

        protected abstract Task<bool> ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken));

        protected virtual Task<bool> ShouldExecuteCore()
        {
            return Task.FromResult(true);
        }

        public virtual bool HandleException(Exception ex, CancellationToken token)
        {
            //Ignore operation cancellations
            if(ex is OperationCanceledException)
            {
                return true;
            }

            Debug.WriteLine(ex);
            if(ex is WebException)
            {
                Notify(OperationNetworkFailureMessage);
                return true;
            }

            Notify(OperationUnknownFailureMessage);
            return true;
        }

        private async void Notify(string message)
        {
            _viewModel.SavingFailureMessage = message;
            await _dialogService.ShowNotificationAsync(message, Strings.Ok);
        }
    }
}