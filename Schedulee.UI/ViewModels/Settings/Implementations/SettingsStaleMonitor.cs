using System.Collections.Generic;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Settings.Implementations
{
    internal class SettingsStaleMonitor : BaseStaleMonitor
    {
        public SettingsStaleMonitor(SettingsViewModel viewModel) : base(viewModel)
        {
            
        }

        public override IEnumerable<string> Properties => new[]
                                                          {
                                                              nameof(ISettingsViewModel.FirstName),
                                                              nameof(ISettingsViewModel.LastName),
                                                              nameof(ISettingsViewModel.Location),
                                                              nameof(ISettingsViewModel.SetTravelTime)
                                                          };
    }
}