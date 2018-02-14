using System.ComponentModel;

namespace Schedulee.UI.ViewModels.Base
{
    public interface IValidateableViewModel : INotifyPropertyChanged
    {
        IViewModelValidator Validator { get; }
    }
}
