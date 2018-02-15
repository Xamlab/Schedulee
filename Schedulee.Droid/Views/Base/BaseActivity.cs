using System.ComponentModel;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using Android.Support.V7.App;
using PropertyChanged;

namespace Schedulee.Droid.Views.Base
{
    [AddINotifyPropertyChangedInterface]
    public class BaseActivity : AppCompatActivity, INotifyPropertyChanged
    {
        public object BindingContext { get; set; }
        public bool IsLoading { get; set; }
        public string LoadingMessage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if(propertyName == nameof(IsLoading))
            {
                if(IsLoading)
                {
                    UserDialogs.Instance.ShowLoading(LoadingMessage);
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
        }
    }
}