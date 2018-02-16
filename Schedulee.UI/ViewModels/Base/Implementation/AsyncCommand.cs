using System.Threading;
using System.Threading.Tasks;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    public abstract class AsyncCommand : Command, IAsyncCommand
    {
        public AsyncCommand(bool canExecute = true) : base(canExecute)
        {
        }

        public override async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public abstract Task ExecuteAsync(object parameter, CancellationToken token = default(CancellationToken));
    }
}