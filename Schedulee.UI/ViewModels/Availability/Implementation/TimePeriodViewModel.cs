using System;

namespace Schedulee.UI.ViewModels.Availability.Implementation
{
    internal class TimePeriodViewModel : ITimePeriodViewModel
    {
        public string FormattedTimePeriod { get; internal set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public override bool Equals(object other)
        {
            if(other == null || !(other is ITimePeriodViewModel viewModel)) return false;
            return string.Equals(FormattedTimePeriod, viewModel.FormattedTimePeriod)
                   && Start.Equals(viewModel.Start)
                   && End.Equals(viewModel.End);
        }

        protected bool Equals(TimePeriodViewModel other)
        {
            return string.Equals(FormattedTimePeriod, other.FormattedTimePeriod)
                   && Start.Equals(other.Start)
                   && End.Equals(other.End);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FormattedTimePeriod != null ? FormattedTimePeriod.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Start.GetHashCode();
                hashCode = (hashCode * 397) ^ End.GetHashCode();
                return hashCode;
            }
        }
    }
}