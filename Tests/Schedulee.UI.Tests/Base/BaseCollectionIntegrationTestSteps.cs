using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.Tests.Base
{
    public class BaseCollectionIntegrationTestSteps
    {
        protected static readonly List<string> MockItems;

        internal class MockLoadableCommand : LoadAsyncCommand
        {
            private readonly BaseCollectionViewModel<string> _viewModel;

            public MockLoadableCommand(BaseCollectionViewModel<string> viewModel) : base(viewModel)
            {
                _viewModel = viewModel;
            }

            protected override Task ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken))
            {
                _viewModel.Items = MockItems;
                return Task.CompletedTask;
            }
        }

        static BaseCollectionIntegrationTestSteps()
        {
            MockItems = Enumerable.Range(1, 10).Select(i => $"Item {i}").ToList();
        }
    }
}
