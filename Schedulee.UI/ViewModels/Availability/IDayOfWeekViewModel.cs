namespace Schedulee.UI.ViewModels.Availability
{
    public interface IDayOfWeekViewModel
    {
        int Day { get; }
        string FormattedDay { get; }
        bool IsSelected { get; }
    }
}