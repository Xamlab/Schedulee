namespace Schedulee.UI.ViewModels.Base
{
    public interface ISaveableViewModel : IValidateableViewModel, IStaleMonitorViewModel
    {
        bool IsSaving { get; }
        IAsyncCommand SaveCommand { get; }
        bool DidSave { get; }
        string SavingFailureMessage { get; }
    }
}