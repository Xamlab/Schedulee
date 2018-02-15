using System;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.UI.Resources.Strings.Common;
using Schedulee.UI.ViewModels.Base;
using Schedulee.UI.ViewModels.Base.Implementation;
using Shouldly;
using TechTalk.SpecFlow;

namespace Schedulee.UI.Tests.Base
{
    [Binding, Scope(Feature = "Loadable Screen")]
    public class LoadableScreenIntegrationTestSteps : BaseTestSteps
    {
        private ILoadableViewModel _viewModel;
        private static CancellationTokenSource _loadCancellation;
        private static CancellationTokenSource _forceCancellation;

        private static bool _shouldFail;

        private class MockAsyncCommand : LoadAsyncCommand
        {
            private readonly ILoadableViewModel _viewModel;

            public MockAsyncCommand(ILoadableViewModel viewModel) : base(viewModel)
            {
                _viewModel = viewModel;
                _loadCancellation = new CancellationTokenSource();
                _forceCancellation = new CancellationTokenSource();
            }

            protected override async Task ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken))
            {
                try
                {
                    var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(_loadCancellation.Token, token);
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
            }
        }

        private class MockLoadableViewModel : BaseLoadableViewModel
        {
            public MockLoadableViewModel()
            {
                LoadCommand = new MockAsyncCommand(this);
            }
        }

        protected override void ConfigureServices()
        {
            Container.Register<ILoadableViewModel, MockLoadableViewModel>();
        }

        [BeforeScenario]
        public static void Cleanup()
        {
            _shouldFail = false;
        }

        //Background operation is in progress
        [Given(@"I have navigated to a loadable screen")]
        public void GivenIHaveNavigatedToALoadableScreen()
        {
            _viewModel = Container.Resolve<ILoadableViewModel>();
        }

        [When(@"the screen is loaded and the background operation is in progress")]
        public void WhenTheScreenIsLoadedAndTheBackgroundOperationIsInProgress()
        {
            _viewModel.LoadCommand.ExecuteAsync(null, _forceCancellation.Token);
        }

        [Then(@"a loading indicator is shown while operation is in progress")]
        public void ThenALoadingIndicatorIsShownWhileOperationIsInProgress()
        {
            _viewModel.IsLoading.ShouldBe(true);
            _loadCancellation.Cancel();
        }

        //Background operation completes successfully
        [Given(@"the screen is loaded and the background operation is in progress")]
        public void GivenTheScreenIsLoadedAndTheBackgroundOperationIsInProgress()
        {
            _viewModel.LoadCommand.ExecuteAsync(null, _forceCancellation.Token);
        }

        [When(@"the background operation completes successfully")]
        public void WhenTheBackgroundOperationFinishes()
        {
            _loadCancellation.Cancel();
        }

        [Then(@"the loading indicator is hidden")]
        public void ThenTheLoadingIndicatorIsHidden()
        {
            _viewModel.IsLoading.ShouldBe(false);
        }

        [Then(@"the content is shown")]
        public void ThenTheContentIsShown()
        {
            _viewModel.IsLoaded.ShouldBe(true);
        }

        //Background operation fails
        [When(@"the background operation fails")]
        public void WhenTheBackgroundOperationFails()
        {
            _shouldFail = true;
            _loadCancellation.Cancel();
        }

        [Then(@"an error specific to the failure is displayed detailing the issue")]
        public void ThenAnErrorSpecificToTheFailureIsDisplayedDetailingTheIssue()
        {
            _viewModel.LoadFailureMessage.ShouldBe(Strings.OperationUnknownFailure);
        }

        [Then(@"the content is hidden")]
        public void ThenTheContentIsHidden()
        {
            _viewModel.IsLoaded.ShouldBe(false);
        }

        //Background operation is canceled
        [When(@"the background operation is canceled")]
        public void WhenTheBackgroundOperationIsCanceled()
        {
            _forceCancellation.Cancel();
        }

        [Then(@"the background operation is stopped")]
        public void ThenTheBackgroundOperationIsStopped()
        {
            _viewModel.IsLoading.ShouldBe(false);
        }
    }
}