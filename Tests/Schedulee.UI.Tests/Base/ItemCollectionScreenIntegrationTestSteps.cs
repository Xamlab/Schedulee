using Schedulee.Core.Extensions;
using Schedulee.UI.ViewModels.Base.Implementation;
using Shouldly;
using TechTalk.SpecFlow;

namespace Schedulee.UI.Tests.Base
{
    [Binding, Scope(Feature = "Item Collection Screen")]
    public class ItemCollectionScreenIntegrationTestSteps : BaseCollectionIntegrationTestSteps
    {
        private MockCollectionViewModel _viewModel;

        private class MockCollectionViewModel : BaseCollectionViewModel<string>
        {
            public MockCollectionViewModel()
            {
                LoadCommand = new MockLoadableCommand(this);
            }
        }

        [Given(@"I have navigated to an Item Collection Screen for the first time")]
        public void GivenIHaveNavigatedToAnItemCollectionScreenForTheFirstTime()
        {
            _viewModel = new MockCollectionViewModel();
        }

        [When(@"the loading operation completes")]
        public void WhenTheLoadingOperationCompletes()
        {
            _viewModel.LoadCommand.ExecuteAsync(null).Wait();
        }

        [Then(@"I should see a list of objects")]
        public void ThenIShouldSeeAListOfObjects()
        {
            _viewModel.Items.ShouldNotBeNull();
            _viewModel.Items.Count().ShouldBe(MockItems.Count);
            var items = _viewModel.Items.ToArray();

            for(int i = 0; i < MockItems.Count; i++)
            {
                items[i].ShouldBe(MockItems[i]);
            }
        }
    }
}