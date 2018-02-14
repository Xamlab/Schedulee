using System.Collections.Generic;
using PropertyChanged;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class BaseCollectionViewModel<T> : BaseLoadableViewModel, IInternalCollectionViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}
