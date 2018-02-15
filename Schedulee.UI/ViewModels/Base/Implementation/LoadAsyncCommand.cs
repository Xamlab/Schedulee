using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Common;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    internal abstract class LoadAsyncCommand : AsyncCommand
    {
        private readonly ILoadableViewModel _viewModel;
        private CancellationTokenSource _loadCancellation;

        protected LoadAsyncCommand(ILoadableViewModel viewModel)
        {
            _viewModel = viewModel;
            viewModel.IsLoaded = false;
            viewModel.IsEmpty = false;
        }

        public override async Task ExecuteAsync(object param, CancellationToken token = default(CancellationToken))
        {
            if(LoadMode == LoadMode.CancelCurrent && _viewModel.IsLoading) return;
            if(LoadMode == LoadMode.CancelPrevious)
            {
                _loadCancellation?.Cancel();
            }

            _loadCancellation = new CancellationTokenSource();
            var compoundToken = CancellationTokenSource.CreateLinkedTokenSource(_loadCancellation.Token, token);

            _viewModel.IsLoaded = false;
            _viewModel.IsLoading = true;
            _viewModel.LoadFailureMessage = null;

            try
            {
                await ExecuteCoreAsync(param, compoundToken.Token);
                _viewModel.IsLoaded = true;
            }
            catch(Exception ex)
            {
                if(!HandleException(ex)) throw;
            }
            finally
            {
                _viewModel.IsLoading = false;
            }
        }

        protected virtual LoadMode LoadMode => LoadMode.CancelPrevious;

        protected abstract Task ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken));

        protected virtual bool HandleException(Exception ex)
        {
            //Ignore operation cancellations
            if(ex is OperationCanceledException)
            {
                return true;
            }

            Debug.WriteLine(ex);
            if(ex is WebException)
            {
                _viewModel.LoadFailureMessage = Strings.OperationNetworkFailure;
                return true;
            }

            _viewModel.LoadFailureMessage = Strings.OperationUnknownFailure;
            return true;
        }
    }
}