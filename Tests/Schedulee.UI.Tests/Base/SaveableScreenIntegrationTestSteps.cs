using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Common;
using Schedulee.UI.Services;
using Schedulee.UI.ViewModels.Base;
using Schedulee.UI.ViewModels.Base.Implementation;
using Shouldly;
using TechTalk.SpecFlow;

namespace Schedulee.UI.Tests.Base
{
    [Binding, Scope(Feature = "Saveable Screen")]
    public class SaveableScreenIntegrationTestSteps : BaseTestSteps
    {
        private ISaveableViewModel _viewModel;
        private static CancellationTokenSource _saveCancellation;
        private static CancellationTokenSource _forceCancellation;
        private static bool _shouldFail;
        private static bool _isValid;

        private class MockAsyncSaveCommand : SaveAsyncCommand
        {
            private readonly IInternalSaveableViewModel _viewModel;

            public MockAsyncSaveCommand(IInternalSaveableViewModel viewModel, IDialogService dialogService)
                : base(viewModel, dialogService)
            {
                _viewModel = viewModel;
                _saveCancellation = new CancellationTokenSource();
                _forceCancellation = new CancellationTokenSource();
            }

            protected override async Task<bool> ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken))
            {
                try
                {
                    var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(_saveCancellation.Token, token);
                    await Task.Delay(TimeSpan.FromMinutes(10), tokenSource.Token);
                    //If the timespan was expired it means the test wasn't canceled
                    //Which means that there's problem in test execution so we need to fail the test.
                    throw new Exception();
                }
                catch(OperationCanceledException)
                {
                    if(_shouldFail) throw new Exception();
                    if(token.IsCancellationRequested) throw;
                }

                return true;
            }
        }

        private class MockSaveableViewModel : BaseViewModel, IInternalSaveableViewModel
        {
            public MockSaveableViewModel(IDialogService dialogService)
            {
                _shouldFail = false;
                _isValid = true;
                SaveCommand = new MockAsyncSaveCommand(this, dialogService);
                Validator = new MockValidator();
                StaleMonitor = new BaseStaleMonitor(this);
            }

            public bool IsSaving { get; set; }
            public IAsyncCommand SaveCommand { get; }
            public IStaleMonitor StaleMonitor { get; }
            public bool DidSave { get; set; }
            public string SavingFailureMessage { get; set; }
            public IViewModelValidator Validator { get; }
        }

        private class MockValidator : IViewModelValidator
        {
            public IEnumerable GetErrors(string propertyName)
            {
                return null;
            }

            public bool HasErrors => !_isValid;
#pragma warning disable CS0067
            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
#pragma warning restore CS0067

            public bool Validate()
            {
                return _isValid;
            }

            public IList GetAllErrors(string[] propertyNames = null)
            {
                return new List<string>();
            }

            public List<string> GetAllErrorsInString()
            {
                return new List<string>();
            }

            public List<string> GetErrorsInString(string propertyName)
            {
                return new List<string>();
            }
        }

        protected override void ConfigureServices()
        {
            Container.Register<ISaveableViewModel, MockSaveableViewModel>();
        }

        //User tries to perform save operation while there are validation errors
        [Given(@"I have navigated to a saveable screen")]
        public void GivenIHaveNavigatedToASaveableScreen()
        {
            _viewModel = Container.Resolve<ISaveableViewModel>();
        }

        [Given(@"I have entered invalid data at least in one of the fields")]
        public void GivenIHaveEnteredInvalidDataAtLeastInOneOfTheFields()
        {
            _isValid = false;
        }

        [When(@"I try to save the data")]
        public void WhenITryToSaveTheData()
        {
            _viewModel.SaveCommand.ExecuteAsync(null, _forceCancellation.Token);
        }

        [Then(@"a message is shown warning the user to fix validation errors before trying to save")]
        public void ThenAMessageIsShownWarningTheUserToFixValidationErrorsBeforeTryingToSave()
        {
            //TODO:Implement mock validator
            var errors = _viewModel.Validator.GetAllErrorsInString();
            DialogService.DialogMessage.ShouldBe(Strings.FixValidationErrors + "\n" + string.Join("\n", errors));
        }

        //User triggers save process
        [Then(@"a loading indicator is shown while saving is in progress")]
        public void ThenALoadingIndicatorIsShownWhileSavingIsInProgress()
        {
            _viewModel.IsSaving.ShouldBe(true);
            _saveCancellation.Cancel();
        }

        //Saving process completes successfully
        [Given(@"I've already triggered saving process")]
        public void GivenIVeAlreadyTriggeredSavingProcess()
        {
            _viewModel.SaveCommand.ExecuteAsync(null, _forceCancellation.Token);
        }

        [When(@"the saving process completes successfully")]
        public void WhenTheSavingProcessCompletesSuccessfully()
        {
            _saveCancellation.Cancel();
        }

        [Then(@"the saving loading indicator is hidden")]
        public void ThenTheSavingLoadingIndicatorIsHidden()
        {
            _viewModel.IsSaving.ShouldBe(false);
        }

        [Then(@"the saveable screen should be dismissed")]
        public void ThenTheSaveableScreenShouldBeDismissed()
        {
            _viewModel.DidSave.ShouldBe(true);
        }

        //Saving process fails
        [When(@"the saving process fails")]
        public void WhenTheSavingProcessFails()
        {
            _shouldFail = true;
            _saveCancellation.Cancel();
        }

        [Then(@"a message is shown detailing the failure reason")]
        public void ThenAMessageIsShownDetailingTheFailureReason()
        {
            DialogService.DialogMessage.ShouldBe(Strings.OperationUnknownFailure);
        }

        //Saving process is canceled
        [When(@"the saving is canceled")]
        public void WhenTheSavingIsCanceled()
        {
            _forceCancellation.Cancel();
        }

        [Then(@"the saving is stopped and the loading indicator is hidden")]
        public void ThenTheSavingIsStoppedAndTheLoadingIndicatorIsHidden()
        {
            _viewModel.IsSaving.ShouldBe(false);
        }
    }
}
