using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedulee.UI.ViewModels.Base
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object param, CancellationToken token = default(CancellationToken));
    }
}
