using PropertyChanged;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal abstract class BaseLoadableViewModel : BaseViewModel, ILoadableViewModel
    {
        public IAsyncCommand LoadCommand { get; protected set; }

        public bool IsLoading { get; set; }

        public string LoadFailureMessage { get; set; }

        public bool IsLoaded { get; set; }

        public bool IsEmpty { get; set; }
    }
}