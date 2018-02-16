using System;
using System.Collections.Generic;
using PropertyChanged;
using Schedulee.Core.Models;
using Schedulee.UI.ViewModels.Base.Implementation;

namespace Schedulee.UI.ViewModels.Reservations.Implementation
{
    [AddINotifyPropertyChangedInterface]
    public class DateViewModel : BaseViewModel, IDateViewModel
    {
        public IList<Reservation> Reservations { get; internal set; }
        public DateTime Date { get; internal set; }
        public string DayOfWeek { get; internal set; }
        public string Day { get; internal set; }
        public bool IsSelected { get; set; }
    }
}