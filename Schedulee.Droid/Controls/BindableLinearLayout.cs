using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using PropertyChanged;

namespace Schedulee.Droid.Controls
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BindableLinearLayout : LinearLayout, IBindableObject
    {
        protected BindableLinearLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected BindableLinearLayout(Context context) : base(context)
        {
        }

        protected BindableLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected BindableLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected BindableLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private object _bindingContext;

        public object BindingContext
        {
            get => _bindingContext;
            set
            {
                if(_bindingContext != value)
                {
                    _bindingContext = value;
                    OnBindingContextChanged();
                }
            }
        }

        protected virtual void OnBindingContextChanged()
        {
        }
    }
}