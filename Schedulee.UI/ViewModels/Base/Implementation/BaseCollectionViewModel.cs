using System.Collections.Generic;
using PropertyChanged;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class BaseCollectionViewModel<T> : BaseLoadableViewModel, IInternalCollectionViewModel<T>
    {
        public IList<T> Items { get; set; }
    }
}