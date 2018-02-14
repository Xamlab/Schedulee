using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Schedulee.UI.ViewModels.Base.Implementation
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected bool SetField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if(EqualityComparer<T>.Default.Equals(field, newValue)) return false;

            field = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}