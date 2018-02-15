using System;
using System.Collections.Generic;
using System.Text;

namespace Schedulee.Core.Models
{
    public class Reservation
    {
        public string Id { get; set; }
        public Client Client { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public double RatePerHour { get; set; }
    }
}
