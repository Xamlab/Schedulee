using System.ComponentModel;

namespace Schedulee.Droid.Controls
{
    public interface IBindableObject : INotifyPropertyChanged
    {
        object BindingContext { get; set; }
    }
}