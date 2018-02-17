using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Helpers;
using PropertyChanged;
using Schedulee.Droid.Extensions;

namespace Schedulee.Droid.Views.Base
{
    [AddINotifyPropertyChangedInterface]
    public class BaseActivity : AppCompatActivity, INotifyPropertyChanged
    {
        public object BindingContext { get; set; }
        public bool IsLoading { get; set; }
        public string LoadingMessage { get; set; }

        protected View Overlay { get; set; }
        protected ProgressBar Progress { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public List<Binding> Bindings { get; } = new List<Binding>();

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if(propertyName == nameof(IsLoading))
            {
                UpdateLoadingView();
            }
        }

        protected virtual void UpdateLoadingView()
        {
            if(Overlay != null && Progress != null)
            {
                if(IsLoading)
                {
                    this.ShowOverlay(Overlay);
                    Progress.Visibility = ViewStates.Visible;
                }
                else
                {
                    this.HideOverlay(Overlay);
                    Progress.Visibility = ViewStates.Gone;
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Bindings.Clear();
        }
    }
}