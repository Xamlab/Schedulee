using System;
using System.Collections.Generic;
using System.Text;
using Schedulee.Core.Models;

namespace Schedulee.UI.ViewModels.Reservations
{
    public interface IDateViewModel
    {
        IEnumerable<Reservation> Reservations { get; }
        DateTime Date { get; }
        string Month { get; }
        string Day { get; }
    }
}
