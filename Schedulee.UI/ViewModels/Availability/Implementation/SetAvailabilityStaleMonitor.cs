using System.Collections.Generic;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class SetAvailabilityStaleMonitor : BaseStaleMonitor
    {
        public SetAvailabilityStaleMonitor(SetAvailabilityViewModel viewModel) : base(viewModel)
        {
        }

        public override IEnumerable<string> Collections => new[]
                                                           {
                                                               nameof(ISetAvailabilityViewModel.DaysOfWeek),
                                                               nameof(ISetAvailabilityViewModel.TimePeriods)
                                                           };
    }
}