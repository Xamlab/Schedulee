using Schedulee.UI.ViewModels.Base;

namespace Schedulee.UI.ViewModels.Settings
{
    public interface ISettingsViewModel : ISaveableViewModel, IViewModelRequiresSetup
    {
        string Username { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Location { get; set; }
        int SetTravelTime { get; set; }
    }
}