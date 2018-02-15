using System.Collections.Generic;

namespace Schedulee.UI.ViewModels.Base
{
    public interface ICollectionViewModel<T> : ILoadableViewModel
    {
        IList<T> Items { get; }
    }
}