using System.Collections.ObjectModel;
using System.Linq;

namespace Schedulee.Core.Extensions
{
    public class Grouping<S, T> : ObservableCollection<T>
    {
        public S Key { get; }

        public Grouping(IGrouping<S, T> group)
            : base(group)
        {
            Key = group.Key;
        }
    }
}