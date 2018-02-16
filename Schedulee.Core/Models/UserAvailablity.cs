namespace Schedulee.Core.Models
{
    public class UserAvailablity
    {
        public string Id { get; set; }
        public int[] DaysOfWeek { get; set; }
        public TimePeriod[] TimePeriods { get; set; }
    }
}