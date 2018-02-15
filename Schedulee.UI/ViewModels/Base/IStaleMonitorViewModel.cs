using System.ComponentModel;

namespace Schedulee.UI.ViewModels.Base
{
    public interface IStaleMonitorViewModel : INotifyPropertyChanged
    {
        IStaleMonitor StaleMonitor { get; }
    }
}