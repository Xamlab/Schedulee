using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Schedulee.Core.Extensions;
using Schedulee.Core.Services;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class LoadUserAvailabilitiesCommand : LoadAsyncCommand
    {
        private readonly SetAvailabilityViewModel _viewModel;
        private readonly IApiClient _apiClient;

        public LoadUserAvailabilitiesCommand(SetAvailabilityViewModel viewModel, IApiClient apiClient) : base(viewModel)
        {
            _apiClient = apiClient;
            _viewModel = viewModel;
        }

        protected override async Task ExecuteCoreAsync(object param, CancellationToken token = default(CancellationToken))
        {
            var result = await _apiClient.FetchUserAvailablities(token);
            var items = new ObservableCollection<IAvailabilityViewModel>();
            if(result != null)
            {
                items.AddRangeGeneric(result.Select(Helpers.MapUserAvailabilityToAvailabilityViewModel));
            }

            _viewModel.Items = items;
        }
    }
}