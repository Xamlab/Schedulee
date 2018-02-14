using System.Windows.Input;

namespace Schedulee.UI.ViewModels.Base
{
    public interface IViewModelRequiresSetup
    {
        ICommand SetupCommand { get; }
    }
}
