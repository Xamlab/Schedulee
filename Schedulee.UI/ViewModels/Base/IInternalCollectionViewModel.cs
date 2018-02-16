using System.Collections.Generic;

namespace Schedulee.UI.ViewModels.Base
{
    internal interface IInternalCollectionViewModel<T> : ICollectionViewModel<T>
    {
        new IList<T> Items { get; set; }
    }
}