using System;
using System.Collections.Generic;
using System.ComponentModel;
using Schedulee.Core.Models;

namespace Schedulee.UI.ViewModels.Reservations
{
    public interface IDateViewModel : INotifyPropertyChanged
    {
        IList<Reservation> Reservations { get; }
        DateTime Date { get; }
        string DayOfWeek { get; }
        string Day { get; }
        bool IsSelected { get; set; }
    }
}