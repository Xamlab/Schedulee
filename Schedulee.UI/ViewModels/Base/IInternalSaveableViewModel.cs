namespace Schedulee.UI.ViewModels.Base
{
    internal interface IInternalSaveableViewModel : ISaveableViewModel
    {
        new bool IsSaving { get; set; }
        new bool DidSave { get; set; }
        new string SavingFailureMessage { get; set; }
    }
}