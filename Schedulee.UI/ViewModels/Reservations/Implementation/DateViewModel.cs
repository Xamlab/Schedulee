﻿using System;
using System.Collections.Generic;
using Schedulee.Core.Models;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    public class DateViewModel : IDateViewModel
    {
        public IEnumerable<Reservation> Reservations { get; internal set; }
        public DateTime Date { get; internal set; }
        public string DayOfWeek { get; internal set; }
        public string Day { get; internal set; }
    }
}