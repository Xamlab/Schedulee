using System.ComponentModel;

namespace Schedulee.UI.ViewModels.Base
{
    public interface ILoadableViewModel : INotifyPropertyChanged
    {
        bool IsLoading { get; set; }
        string LoadFailureMessage { get; set; }
        IAsyncCommand LoadCommand { get; }
        bool IsLoaded { get; set; }
        bool IsEmpty { get; set; }
    }
}