using System.Collections.Generic;

namespace Schedulee.UI.ViewModels.Base
{
    public interface ICollectionViewModel<out T> : ILoadableViewModel
    {
        IEnumerable<T> Items { get; }
    }
}
