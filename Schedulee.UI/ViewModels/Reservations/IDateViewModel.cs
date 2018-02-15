using System;
using System.Collections.Generic;
using Schedulee.Core.Models;

namespace Schedulee.UI.ViewModels.Reservations
{
    public interface IDateViewModel
    {
        IEnumerable<Reservation> Reservations { get; }
        DateTime Date { get; }
        string DayOfWeek { get; }
        string Day { get; }
    }
}