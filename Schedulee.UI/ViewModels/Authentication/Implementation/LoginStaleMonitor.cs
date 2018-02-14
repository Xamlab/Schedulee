using System.Collections.Generic;
using System.ComponentModel;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Authentication.Implementation
{
    internal class LoginStaleMonitor : BaseStaleMonitor
    {
        public LoginStaleMonitor(INotifyPropertyChanged viewModel) : base(viewModel)
        {
        }

        public override IEnumerable<string> Properties => new[]
                                                          {
                                                              nameof(ILoginViewModel.Email),
                                                              nameof(ILoginViewModel.Password)
                                                          };
    }
}
