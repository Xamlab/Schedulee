using System.ComponentModel;

namespace Schedulee.UI.ViewModels.Availability
{
    public interface IDayOfWeekViewModel : INotifyPropertyChanged
    {
        int Day { get; }
        string FormattedDay { get; }
        bool IsSelected { get; }
    }
}