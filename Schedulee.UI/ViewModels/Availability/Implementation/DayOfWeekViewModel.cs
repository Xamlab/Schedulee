using PropertyChanged;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    [AddINotifyPropertyChangedInterface]
    internal class DayOfWeekViewModel : BaseViewModel, IDayOfWeekViewModel
    {
        public int Day { get; internal set; }
        public string FormattedDay { get; internal set; }
        public bool IsSelected { get; internal set; }

        public override bool Equals(object other)
        {
            if(other == null || !(other is IDayOfWeekViewModel viewModel)) return false;
            return Day == viewModel.Day && string.Equals(FormattedDay, viewModel.FormattedDay) && IsSelected == viewModel.IsSelected;
        }

        protected bool Equals(DayOfWeekViewModel other)
        {
            return Day == other.Day && string.Equals(FormattedDay, other.FormattedDay) && IsSelected == other.IsSelected;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Day;
                hashCode = (hashCode * 397) ^ (FormattedDay != null ? FormattedDay.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsSelected.GetHashCode();
                return hashCode;
            }
        }
    }
}